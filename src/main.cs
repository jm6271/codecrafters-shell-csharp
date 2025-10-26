class Program
{
    static void Main()
    {
        Console.Write("$ ");

        string command = Console.ReadLine() ?? "";

        Console.Error.WriteLine($"{command}: command not found");
    }
}
