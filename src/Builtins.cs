// Shell builtin commands

// NOTE: args array include the name of the command as the first item

static class Builtins
{
    public static readonly Dictionary<string, Action<string[]>> Commands = new()
    {
        ["cd"] = Cd,
        ["echo"] = Echo,
        ["exit"] = Exit,
        ["history"] = History,
        ["pwd"] = Pwd,
        ["type"] = Type,
    };

    public static void Cd(string[] args)
    {
        if (args.Length == 1)
        {
            Console.Error.WriteLine("cd: Error: No directory provided");
        }
        else if (args.Length > 2)
        {
            Console.Error.WriteLine("cd: Error: Too many arguments");
        }
        else
        {
            if (args[1] == "~") // Home directory
            {
                string? home = Environment.GetEnvironmentVariable("HOME");
                if (home != null)
                {
                    Directory.SetCurrentDirectory(home);
                }
                else
                {
                    Console.Error.WriteLine("cd: Error: couldn't find home directory");
                }
            }
            else if (Directory.Exists(args[1]))
            {
                Directory.SetCurrentDirectory(args[1]);
            }
            else
            {
                Console.WriteLine($"cd: {args[1]}: No such file or directory");
            }
        }
    }

    public static void Echo(string[] args)
    {
        if (args.Length == 1)
        {
            // Just print a newline
            Console.WriteLine();
        }
        else
        {
            for (int i = 1; i < args.Length; i++)
            {
                Console.Write($"{args[i]} ");
            }
            Console.WriteLine();
        }
    }

    public static void Exit(string[] args)
    {
        if (args.Length == 1)
        {
            Environment.Exit(0);
        }
        else if (args.Length == 2)
        {
            try
            {
                int code = Convert.ToInt32(args[1]);
                Environment.Exit(code);
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"exit: Error: invalid exit code '{args[1]}'");
            }
        }
        else
        {
            Console.Error.WriteLine("exit: Error: Too many arguments");
        }
    }

    public static void History(string[] args)
    {
        if (args.Length > 1)
        {
            Console.Error.WriteLine("history: Error: Too many arguments");
            return;
        }

        for (int i = 0; i < ShellHistory.History.Count; i++)
        {
            Console.WriteLine($"    {i + 1}  {ShellHistory.History[i]}");
        }
    }

    public static void Pwd(string[] args)
    {
        if (args.Length == 1)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
        }
        else
        {
            Console.Error.WriteLine("pwd: Error: Too many arguments");
        }
    }

    public static void Type(string[] args)
    {
        if (args.Length == 2)
        {
            if (Commands.ContainsKey(args[1]))
            {
                Console.WriteLine($"{args[1]} is a shell builtin");
            }
            else
            {
                try
                {
                    SystemCommandLookup commandLookup = new();
                    string commandPath = commandLookup.GetCommandPath(args[1]);
                    Console.WriteLine($"{args[1]} is {commandPath}");
                }
                catch (FileNotFoundException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }
        }

        if (args.Length > 2)
        {
            Console.Error.WriteLine("type: Error: too many arguments");
        }
    }
}
