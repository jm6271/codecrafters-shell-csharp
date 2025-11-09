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

    public static void RunProgramWithRedirect(string[] args, string filename)
    {
        Process process = new()
        {
            StartInfo = new()
            {
                FileName = args[0],
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            },
        };

        foreach (var arg in args.Skip(1))
        {
            process.StartInfo.ArgumentList.Add(arg);
        }

        process.Start();
        process.WaitForExit();

        StreamWriter outputWriter = new(filename);
        outputWriter.Write(process.StandardOutput.ReadToEnd());
        outputWriter.Flush();
        outputWriter.Close();
    }
}
