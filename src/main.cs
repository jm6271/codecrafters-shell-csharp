class Program
{
    static void Main()
    {
        while (true)
        {
            // Write prompt
            Console.Write("$ ");

            // Read command
            string command = Console.ReadLine() ?? "";
            command = command.Trim();

            if (command == "")
            {
                // whitespace
                continue;
            }
            else if (command.StartsWith("exit"))
            {
                // Exit can take one arg, the exit code
                var args = command.Split(' ');

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
            else if (command.StartsWith("echo"))
            {
                // Print all args, with spaces between them
                var args = command.Split(' ');

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
            else
            {
                // Command not recognized
                Console.Error.WriteLine($"{command}: command not found");
            }                      
        }
    }
}
