using System.Diagnostics;

class CommandProcessor
{
    public void ProcessCommand(string command)
    {
        command = command.Trim();

        if (command == "") return;

        // Split into array of args
        var args = SplitArgs(command);

        // Lookup the command in the builtins
        if (IsBuiltin(args[0]))
        {
            // Run builtin command
            Builtins.Commands[args[0]](args);
        }
        else
        {
            // Try to run a program
            SystemCommandLookup commandLookup = new();

            if (commandLookup.DoesCommandExist(args[0]))
                ProcessLauncher.RunProgram(args);
            else
            {
                Console.Error.WriteLine($"{args[0]}: not found");
            }
        }
    }

    private static bool IsBuiltin(string command)
    {
        if (Builtins.Commands.ContainsKey(command))
            return true;
        return false;
    }

    private static string[] SplitArgs(string command)
    {
        List<string> args = [];
        var currentArg = new System.Text.StringBuilder();
        bool inSingleQuote = false;
        bool inDoubleQuote = false;
        bool escaped = false;

        for (int i = 0; i < command.Length; i++)
        {
            char c = command[i];

            if (escaped)
            {
                // Only certain escapes are special inside double quotes
                if (inDoubleQuote)
                {
                    if (c == '"' || c == '\\' || c == '$' || c == '`' || c == '\n')
                    {
                        // backslash escapes these: add the char without the backslash
                        currentArg.Append(c);
                    }
                    else
                    {
                        // others are literal: keep the backslash too
                        currentArg.Append('\\');
                        currentArg.Append(c);
                    }
                }
                else
                {
                    // outside double quotes (or in single quotes, which can't happen with escaped==true)
                    currentArg.Append(c);
                }

                escaped = false;
                continue;
            }

            if (c == '\\')
            {
                // backslash outside single quotes always begins an escape
                if (inSingleQuote)
                {
                    // literal inside single quotes
                    currentArg.Append(c);
                }
                else
                {
                    escaped = true;
                }
                continue;
            }

            if (c == '\'')
            {
                if (inDoubleQuote)
                {
                    // literal single quote inside double quotes
                    currentArg.Append(c);
                }
                else
                {
                    inSingleQuote = !inSingleQuote;
                }
                continue;
            }

            if (c == '"')
            {
                if (inSingleQuote)
                {
                    // literal double quote inside single quotes
                    currentArg.Append(c);
                }
                else
                {
                    inDoubleQuote = !inDoubleQuote;
                }
                continue;
            }

            if (char.IsWhiteSpace(c) && !inSingleQuote && !inDoubleQuote)
            {
                if (currentArg.Length > 0)
                {
                    args.Add(currentArg.ToString());
                    currentArg.Clear();
                }
                continue;
            }

            currentArg.Append(c);
        }

        if (currentArg.Length > 0)
        {
            args.Add(currentArg.ToString());
        }

        return args.ToArray();
    }

}
