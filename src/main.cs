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

            // Process command
            CommandProcessor.ProcessCommand(command);                  
        }
    }
}
