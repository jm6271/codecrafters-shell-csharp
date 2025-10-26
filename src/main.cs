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

            // Command not recognized
            Console.Error.WriteLine($"{command}: command not found");            
        }

    }
}
