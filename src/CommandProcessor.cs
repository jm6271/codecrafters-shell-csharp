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
        string currentArg = "";
        bool inSingleQuote = false;
        bool inDoubleQuote = false;
        bool escaped = false;

        for (int i = 0; i < command.Length; i++)
        {
            char c = command[i];

            if (escaped)
            {
                currentArg += c;
                escaped = false;
                continue;
            }

            if (c == '\\')
            {
                escaped = true;
                continue;
            }

            if (c == '\'')
            {
                if (!inDoubleQuote)
                {
                    inSingleQuote = !inSingleQuote;
                }
                else
                {
                    currentArg += c;
                }
            }
            else if (c == '"')
            {
                if (!inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                }
                else
                {
                    currentArg += c;
                }
            }
            else if (c == ' ' && !inSingleQuote && !inDoubleQuote)
            {
                if (currentArg.Length > 0)
                {
                    args.Add(currentArg);
                    currentArg = "";
                }
            }
            else
            {
                currentArg += c;
            }
        }

        if (currentArg.Length > 0)
        {
            args.Add(currentArg);
        }

        return [.. args];
    }
}
