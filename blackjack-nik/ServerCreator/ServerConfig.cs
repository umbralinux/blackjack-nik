using System;
using System.Text;

namespace ServerCreator
{
    internal class ServerConfig
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public int MaxPlayers { get; set; }
        public string Description { get; set; }

        public string ToJson()
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine($"  \"name\": \"{Escape(Name)}\",");
            builder.AppendLine($"  \"region\": \"{Escape(Region)}\",");
            builder.AppendLine($"  \"maxPlayers\": {MaxPlayers},");
            builder.AppendLine($"  \"description\": \"{Escape(Description)}\",");
            builder.AppendLine("  \"dealer\": \"Computer\"");
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty)
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        }
    }
}

