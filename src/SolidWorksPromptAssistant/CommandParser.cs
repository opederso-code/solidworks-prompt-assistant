using System;

namespace SolidWorksPromptAssistant
{
    internal enum CommandKind { Invalid, Help, ActiveDocument, SetProperty }

    internal sealed class ParsedCommand
    {
        public CommandKind Kind { get; }
        public string Name { get; }
        public string Value { get; }
        public string Description { get; }

        public ParsedCommand(CommandKind kind, string name = "", string value = "", string description = "")
        {
            Kind = kind;
            Name = name;
            Value = value;
            Description = description;
        }
    }

    internal static class CommandParser
    {
        public static ParsedCommand Parse(string? input)
        {
            var text = (input ?? "").Trim();
            if (text.Equals("help", StringComparison.OrdinalIgnoreCase)) return new ParsedCommand(CommandKind.Help);
            if (text.Equals("active document", StringComparison.OrdinalIgnoreCase)) return new ParsedCommand(CommandKind.ActiveDocument);
            const string prefix = "set property ";
            if (text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                var body = text.Substring(prefix.Length);
                var separator = body.IndexOf('=');
                if (separator > 0)
                {
                    var name = body.Substring(0, separator).Trim();
                    var value = body.Substring(separator + 1).Trim();
                    if (name.Length > 0 && value.Length > 0)
                        return new ParsedCommand(CommandKind.SetProperty, name, value);
                }
            }
            return new ParsedCommand(CommandKind.Invalid, description: "Unknown command. Type 'help' for examples.");
        }
    }
}
