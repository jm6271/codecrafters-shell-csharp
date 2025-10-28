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
        return command.Split(' ');
    }
}
