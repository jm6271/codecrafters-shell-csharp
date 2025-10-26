class SystemCommandLookup
{
    private List<string> PathDirs = new();

    public SystemCommandLookup()
    {
        // Load path variable
        string PATH = Environment.GetEnvironmentVariable("PATH") ?? "";
        var paths = PATH.Split(Path.PathSeparator);

        // Ensure paths exist then put them in the PathDirs list
        foreach (var path in paths)
        {
            if (Path.Exists(path))
                PathDirs.Add(path);
        }
    }

    public string GetCommandPath(string command)
    {
        // Search all directories on path for the specified command
        foreach (var dir in PathDirs)
        {
            if (Path.Exists(Path.Combine(dir, command)))
                return Path.Combine(dir, command);
        }

        throw new FileNotFoundException($"{command}: not found");
    }
}