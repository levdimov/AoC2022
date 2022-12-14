namespace AoC2022;

public class Day7 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        _ = input
            .Where(i => !string.IsNullOrEmpty(i))
            .Select(ToCommand)
            .Aggregate(Directory.Root, (currentDirectory, command) => command(currentDirectory));

        return Part2();
    }

    private static string Part1()
    {
        return Directory.Root.EnumerateDirectories()
            .Where(d => d.Size <= 100_000)
            .Sum(d => d.Size)
            .ToString();
    }

    private static string Part2()
    {
        return Directory.Root.EnumerateDirectories()
            .Where(d => d.Size >= 30_000_000 - (70_000_000 - Directory.Root.Size))
            .First()
            .Size
            .ToString();
    }

    private static Func<Directory, Directory> ToCommand(string line)
    {
        var parts = line.Split(" ");

        var command = (parts[0], parts[1], parts.ElementAtOrDefault(2));

        return command switch
        {
            ("$", "cd", _) => d => d.Cd(command.Item3),
            ("$", "ls", _) => d => d,
            ("dir", _, _) => d =>
                             {
                                 d.MkDir(command.Item2);
                                 return d;
                             },
            _ => d =>
                 {
                     d.CreateFile(command.Item2, int.Parse(command.Item1));
                     return d;
                 }
        };
    }

    private class File
    {
        public string Name { get; }

        public int Size { get; }

        public File(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }

    private class Directory
    {
        private readonly Dictionary<string, File> files = new();
        private readonly Dictionary<string, Directory> directories = new();

        public Directory? Parent { get; }

        public string Name { get; }

        public int Size => directories.Values.Sum(d => d.Size) + files.Values.Sum(f => f.Size);

        public static Directory Root { get; } = new("/", null);

        private Directory(string name, Directory? parent)
        {
            Name = name;
            Parent = parent;
        }

        public Directory Cd(string name)
        {
            if (name == "/")
            {
                return Root;
            }

            if (name == "..")
            {
                return Parent ?? Root;
            }

            return directories[name];
        }

        public void MkDir(string name)
        {
            if (directories.ContainsKey(name))
            {
                throw new ArgumentException("Directory with such name already exists", nameof(name));
            }

            directories[name] = new Directory(name, this);
        }

        public void CreateFile(string name, int size)
        {
            if (files.ContainsKey(name))
            {
                throw new ArgumentException("File with such name already exists", nameof(name));
            }

            files[name] = new File(name, size);
        }

        public Directory[] EnumerateDirectories()
        {
            return directories.Values
                .SelectMany(d => d.EnumerateDirectories())
                .Concat(new[] { this })
                .OrderBy(d => d.Size)
                .ToArray();
        }
    }
}
