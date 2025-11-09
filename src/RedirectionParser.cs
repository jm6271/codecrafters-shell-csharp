public static class RedirectionParser
{
    public static bool IsRedirect(string[] args)
    {
        foreach (var arg in args)
        {
            if (arg == "1>" || arg == ">")
                return true;
        }

        return false;
    }

    public static void ExecuteRedirection(string[] args)
    {
        if (args.Length < 1)
            throw new ArgumentException("Argument list was empty", nameof(args));
        var redirection = ParseRedirection(args);

        // check for a builtin
        if (Builtins.Commands.TryGetValue(redirection.Args[0], out Action<string[]>? builtin))
        {
            // Redirect stdout to a file
            StreamWriter streamWriter = new(redirection.Filename);
            var originalWriter = Console.Out;
            Console.SetOut(streamWriter);
            builtin(redirection.Args);

            // Reset stdout
            Console.SetOut(originalWriter);

            // Write output
            streamWriter.Flush();
            streamWriter.Close();
        }
        else if (File.Exists(redirection.Args[0])) // Relative path to the current directory
        {
            // If it doesn't contain a '/' character, don't run i.e. command should be './command', not 'command'
            if (redirection.Args[0].Contains('/'))
                ProcessLauncher.RunProgramWithRedirect(redirection.Args, redirection.Filename);
            else
            {
                Console.Error.WriteLine($"{redirection.Args[0]}: command not found");
                Console.Error.WriteLine($"Note: did you mean: './{redirection.Args[0]}'?");
            }
        }
        else
        {
            // Try to run a program
            SystemCommandLookup commandLookup = new();

            if (commandLookup.DoesCommandExist(redirection.Args[0]))
                ProcessLauncher.RunProgramWithRedirect(redirection.Args, redirection.Filename);
            else
            {
                Console.Error.WriteLine($"{redirection.Args[0]}: not found");
            }
        }
    }

    private struct Redirection
    {
        public string[] Args { get; set; } = [];
        public string Filename { get; set; } = "";

        public Redirection() { }
    }

    private static Redirection ParseRedirection(string[] args)
    {
        if (!IsRedirect(args))
            throw new ArgumentException("Provided argument list is not a redirection", nameof(args));

        List<string> commandArgs = [];
        string filename = "";

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "1>" || args[i] == ">")
            {
                if (i + 1 >= args.Length)
                    throw new ArgumentException("Redirection operator without filename", nameof(args));

                filename = args[i + 1];
                i++; // Skip the filename
            }
            else
            {
                commandArgs.Add(args[i]);
            }
        }

        Redirection redirection = new() { Args = [.. commandArgs], Filename = filename };
        return redirection;
    }
}