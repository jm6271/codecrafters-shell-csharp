class Program
{
    static void Main()
    {
        CommandProcessor commandProcessor = new();
        while (true)
        {
            // Write prompt
            Console.Write("$ ");

            // Read command
            string command = Console.ReadLine() ?? "";

            // Process command
            commandProcessor.ProcessCommand(command);                  
        }
    }
}
