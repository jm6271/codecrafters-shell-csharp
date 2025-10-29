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
            string command = ReadLine.Read() ?? "";

            // Process command
            commandProcessor.ProcessCommand(command);                  
        }
    }
}
