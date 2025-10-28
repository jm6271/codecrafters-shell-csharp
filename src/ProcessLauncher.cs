using System.Diagnostics;

class ProcessLauncher
{
    public static void RunProgram(string[] args)
    {
        // Process.Start(args[0], args.Skip(1));

        Process process = new()
        {
            StartInfo = new()
            {
                FileName = args[0],
                Arguments = string.Join(' ', args.Skip(1)),
            }
        };

        process.Start();
        process.WaitForExit();
    }
}
