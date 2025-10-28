using System.Diagnostics;

class ProcessLauncher
{
    public static void RunProgram(string[] args)
    {

        Process process = new()
        {
            StartInfo = new()
            {
                FileName = args[0],
            }
        };

        foreach (var arg in args.Skip(1))
        {
            process.StartInfo.ArgumentList.Add(arg);
        }

        process.Start();
        process.WaitForExit();
    }
}
