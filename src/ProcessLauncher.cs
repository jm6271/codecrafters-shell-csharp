using System.Diagnostics;

class ProcessLauncher
{
    public static void RunProgram(string[] args)
    {
        // Create argument string
        string argString = "";
        
        foreach (var arg in args.Skip(1))
        {
            if (arg.Contains(' '))
                argString += $"\"{arg}\" ";
            else
                argString += arg + ' ';
        }

        Process process = new()
        {
            StartInfo = new()
            {
                FileName = args[0],
                Arguments = argString,
            }
        };

        process.Start();
        process.WaitForExit();
    }
}
