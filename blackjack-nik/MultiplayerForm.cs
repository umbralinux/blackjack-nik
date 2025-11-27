using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;

namespace blackjack_nik
{
    public partial class MultiplayerForm : Form
    {
        private readonly List<ServerInfo> servers = new List<ServerInfo>
        {
            new ServerInfo
            {
                Name = "Debug Bot Server",
                Region = "Local Simulation",
                Status = "Offline (Bot)",
                Players = "0 / 4",
                Description = "A placeholder bot server that runs locally for debugging."
            }
        };

        public MultiplayerForm()
        {
            InitializeComponent();
            LoadServers();
        }

        private void LoadServers()
        {
            lvServers.BeginUpdate();
            lvServers.Items.Clear();
            foreach (var server in servers)
            {
                var item = new ListViewItem(server.Name)
                {
                    Tag = server
                };
                item.SubItems.Add(server.Region);
                item.SubItems.Add(server.Status);
                item.SubItems.Add(server.Players);
                lvServers.Items.Add(item);
            }

            if (lvServers.Items.Count > 0)
            {
                lvServers.Items[0].Selected = true;
            }
            lvServers.EndUpdate();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            btnRefresh.Text = "Scanning...";
            try
            {
                await ScanForServersAsync();
            }
            finally
            {
                btnRefresh.Enabled = true;
                btnRefresh.Text = "Refresh";
            }
        }

        private async Task ScanForServersAsync()
        {
            servers.Clear();
            // Re-add the debug bot server
            servers.Add(new ServerInfo
            {
                Name = "Debug Bot Server",
                Region = "Local Simulation",
                Status = "Offline (Bot)",
                Players = "0 / 4",
                Description = "A placeholder bot server that runs locally for debugging."
            });

            LoadServers(); // Clear UI first

            using (var udpClient = new UdpClient())
            {
                try
                {
                    udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 47777));
                    
                    var cancelToken = new System.Threading.CancellationTokenSource();
                    cancelToken.CancelAfter(2000); // Scan for 2 seconds

                    try
                    {
                        while (!cancelToken.IsCancellationRequested)
                        {
                            var result = await udpClient.ReceiveAsync().WithCancellation(cancelToken.Token);
                            var json = Encoding.UTF8.GetString(result.Buffer);
                            var server = ParseServerJson(json);
                            
                            if (server != null)
                            {
                                // Check for duplicates
                                bool exists = false;
                                foreach (var s in servers)
                                {
                                    if (s.Name == server.Name && s.Region == server.Region)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (!exists)
                                {
                                    server.Status = "Online";
                                    server.Players = $"{server.Players} / {server.MaxPlayers}"; // Format players string
                                    servers.Add(server);
                                    LoadServers();
                                }
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Timeout reached, scanning finished
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Scanning failed: {ex.Message}", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private ServerInfo ParseServerJson(string json)
        {
            try
            {
                // Simple regex parsing since we don't have a JSON library guaranteed
                var name = Regex.Match(json, "\"name\":\\s*\"(.*?)\"").Groups[1].Value;
                var region = Regex.Match(json, "\"region\":\\s*\"(.*?)\"").Groups[1].Value;
                var maxPlayersStr = Regex.Match(json, "\"maxPlayers\":\\s*(\\d+)").Groups[1].Value;
                var description = Regex.Match(json, "\"description\":\\s*\"(.*?)\"").Groups[1].Value;

                if (!string.IsNullOrEmpty(name))
                {
                    return new ServerInfo
                    {
                        Name = name,
                        Region = region,
                        MaxPlayers = maxPlayersStr, // Temporary storage
                        Description = description,
                        Status = "Online"
                    };
                }
            }
            catch { }
            return null;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (lvServers.SelectedItems.Count == 0) return;
            var server = (ServerInfo)lvServers.SelectedItems[0].Tag;

            using (var table = new MultiplayerGameForm(server, GameOptions.FromSettings()))
            {
                table.ShowDialog(this);
            }
        }

        private void lvServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvServers.SelectedItems.Count == 0)
            {
                lblDetails.Text = "Select a server to view details.";
                btnConnect.Enabled = false;
                return;
            }

            var server = (ServerInfo)lvServers.SelectedItems[0].Tag;
            lblDetails.Text = $"{server.Name} ({server.Region})\nStatus: {server.Status}\nPlayers: {server.Players}\n\n{server.Description}";
            btnConnect.Enabled = true;
        }



        // Extension method for cancellation support in older .NET Frameworks if needed, 
        // but since we target 4.8 or similar, we might need a helper wrapper.
        // However, UdpClient.ReceiveAsync() doesn't take a CancellationToken in older .NET.
        // We'll use a simple WhenAny trick.
        


        private void btnHost_Click(object sender, EventArgs e)
        {
            using (var form = new HostGameDialog())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    // Quote arguments to handle spaces
                    string args = $"\"{form.ServerName}\" \"{form.ServerRegion}\" \"{form.MaxPlayers}\" \"{form.Description}\"";
                    
                    // Try to find ServerCreator.exe
                    // 1. Same directory (deployment)
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerCreator.exe");
                    
                    if (!System.IO.File.Exists(path))
                    {
                        // 2. Development path (relative to bin/Debug of main app)
                        // Main app: blackjack-nik/bin/Debug
                        // Server app: blackjack-nik/ServerCreator/bin/Debug
                        path = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\ServerCreator\bin\Debug\ServerCreator.exe"));
                    }

                    if (System.IO.File.Exists(path))
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(path, args);
                            
                            // Add to local list temporarily so user sees it immediately
                            var newServer = new ServerInfo
                            {
                                Name = form.ServerName,
                                Region = form.ServerRegion,
                                Status = "Online (Local)",
                                Players = $"0 / {form.MaxPlayers}",
                                Description = form.Description
                            };
                            servers.Add(newServer);
                            LoadServers();

                            MessageBox.Show($"Server '{form.ServerName}' started successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to start server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Could not find ServerCreator.exe.\nPlease build the ServerCreator project.\nExpected at: {path}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private class HostGameDialog : Form
        {
            public string ServerName { get; private set; }
            public string ServerRegion { get; private set; }
            public int MaxPlayers { get; private set; }
            public string Description { get; private set; }

            private TextBox txtName;
            private TextBox txtRegion;
            private NumericUpDown nudPlayers;
            private TextBox txtDescription;

            public HostGameDialog()
            {
                this.Text = "Host New Game";
                this.Size = new Size(400, 350);
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.BackColor = Color.FromArgb(32, 32, 32);
                this.ForeColor = Color.White;

                var layout = new TableLayoutPanel();
                layout.Dock = DockStyle.Fill;
                layout.Padding = new Padding(20);
                layout.RowCount = 5;
                layout.ColumnCount = 2;
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Name
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Region
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Max Players
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Description
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Buttons

                // Name
                layout.Controls.Add(CreateLabel("Server Name:"), 0, 0);
                txtName = new TextBox { Text = "My Server", Dock = DockStyle.Fill };
                layout.Controls.Add(txtName, 1, 0);

                // Region
                layout.Controls.Add(CreateLabel("Region:"), 0, 1);
                txtRegion = new TextBox { Text = "Local", Dock = DockStyle.Fill };
                layout.Controls.Add(txtRegion, 1, 1);

                // Max Players
                layout.Controls.Add(CreateLabel("Max Players:"), 0, 2);
                nudPlayers = new NumericUpDown { Minimum = 2, Maximum = 6, Value = 4, Dock = DockStyle.Fill };
                layout.Controls.Add(nudPlayers, 1, 2);

                // Description
                layout.Controls.Add(CreateLabel("Description:"), 0, 3);
                txtDescription = new TextBox { Text = "Join my game!", Multiline = true, Dock = DockStyle.Fill, Height = 60 };
                layout.Controls.Add(txtDescription, 1, 3);

                // Buttons
                var btnPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill, AutoSize = true };
                var btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, BackColor = Color.Gray, FlatStyle = FlatStyle.Flat };
                var btnOk = new Button { Text = "Create Server", DialogResult = DialogResult.OK, BackColor = Color.Green, FlatStyle = FlatStyle.Flat };
                
                btnOk.Click += (s, e) =>
                {
                    ServerName = txtName.Text.Trim();
                    ServerRegion = txtRegion.Text.Trim();
                    MaxPlayers = (int)nudPlayers.Value;
                    Description = txtDescription.Text.Trim();
                    if (string.IsNullOrEmpty(ServerName))
                    {
                        MessageBox.Show("Server Name is required.");
                        this.DialogResult = DialogResult.None;
                    }
                };

                btnPanel.Controls.Add(btnOk);
                btnPanel.Controls.Add(btnCancel);
                layout.Controls.Add(btnPanel, 1, 4);

                this.Controls.Add(layout);
                this.AcceptButton = btnOk;
                this.CancelButton = btnCancel;
            }

            private Label CreateLabel(string text)
            {
                return new Label { Text = text, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, AutoSize = true };
            }
        }
    }
    }

    public static class TaskExtensions
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, System.Threading.CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }
            return await task;
        }



}


