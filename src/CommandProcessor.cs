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
            try
            {
                var program = commandLookup.GetCommandPath(args[0]);
                var process = new Process
                {
                    StartInfo = new()
                    {
                        FileName = program,
                        Arguments = string.Join(" ", args.Skip(1).ToArray()),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };


                process.Start();
                process.OutputDataReceived += (s, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
                process.ErrorDataReceived += (s, e) => { if (e.Data != null) Console.Error.WriteLine(e.Data); };
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{args[0]}: command not found");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
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
