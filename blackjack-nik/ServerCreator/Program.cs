using System;
using System.IO;

namespace ServerCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Blackjack Server Creator";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Blackjack Server Creator ===");
            Console.ResetColor();

            ServerConfig config;
            bool autoStart = false;

            if (args != null && args.Length >= 3)
            {
                try
                {
                    config = new ServerConfig
                    {
                        Name = args[0],
                        Region = args[1],
                        MaxPlayers = int.Parse(args[2]),
                        Description = args.Length > 3 ? args[3] : ""
                    };
                    autoStart = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing arguments: " + ex.Message);
                    config = PromptForConfig();
                }
            }
            else
            {
                config = PromptForConfig();
            }

            var path = SaveConfiguration(config);

            Console.WriteLine();
            Console.WriteLine($"Server configuration saved to: {path}");

            if (!autoStart)
            {
                Console.WriteLine("Press Enter to start hosting this server (debug mode)...");
                Console.ReadLine();
            }

            var host = new ServerHost(config);
            host.Run();

            Console.WriteLine("Server stopped. Press Enter to exit.");
            if (!autoStart)
            {
                Console.ReadLine();
            }
        }

        private static ServerConfig PromptForConfig()
        {
            var config = new ServerConfig();

            config.Name = Prompt("Server Name", "Local Debug Server");
            config.Region = Prompt("Region", "Local Machine");
            config.MaxPlayers = ParseInt(Prompt("Max Players (2-6)", "4"), 2, 6, 4);
            config.Description = Prompt("Description", "Local debug server with computer dealer.");

            return config;
        }

        private static string Prompt(string label, string defaultValue)
        {
            Console.Write($"{label} [{defaultValue}]: ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;
            return input.Trim();
        }

        private static int ParseInt(string text, int min, int max, int fallback)
        {
            if (int.TryParse(text, out var value))
            {
                if (value < min) return min;
                if (value > max) return max;
                return value;
            }

            return fallback;
        }

        private static string SaveConfiguration(ServerConfig config)
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "servers");
            Directory.CreateDirectory(directory);

            var file = Path.Combine(directory, $"{SanitizeFileName(config.Name)}.json");
            File.WriteAllText(file, config.ToJson());
            return file;
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var invalid in Path.GetInvalidFileNameChars())
                name = name.Replace(invalid, '_');
            return name;
        }
    }
}

