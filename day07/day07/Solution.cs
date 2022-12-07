using System.Collections.Immutable;

namespace day07;

public static class Solution
{
    public static IEnumerable<ImmutableList<string>> ChunkCommands(this IEnumerable<string> lines) =>
        lines.Append("$ cd /").Aggregate(
            (Tmp: ImmutableList<string>.Empty, Result: ImmutableList<ImmutableList<string>>.Empty), (lists, line) =>
                line.StartsWith('$') && lists.Tmp.Count > 0
                    // start of nem command, so we clear tmp before appending to it,
                    // and add the accumulated tmp to the result
                    ? (lists.Tmp.Clear().Add(line), lists.Result.Add(lists.Tmp))
                    // otherwise, we simply add the current line to the tmp aggregated list
                    : (lists.Tmp.Add(line), lists.Result)
        ).Result;

    public static FsNode.Dir BuildFs(IEnumerable<string> lines)
    {
        var tree = new FsNode.Dir();
        var dirStack = ImmutableStack<FsNode.Dir>.Empty.Push(tree);
        foreach (var cmd in lines.Skip(1).ChunkCommands())
        {
            switch (cmd[0].Split(' '))
            {
                case ["$", "ls"]:
                    dirStack.Peek().Items = cmd
                        .Skip(1)
                        .Select<string, (string, FsNode)>(
                            it => it.Split(' ') switch
                            {
                                ["dir", var dirName] => (dirName, new FsNode.Dir()),
                                [var size, var fileName] => (fileName, new FsNode.File(int.Parse(size))),
                                _ => throw new ArgumentException("Invalid ls output")
                            })
                        .ToDictionary(x => x.Item1, x => x.Item2);
                    break;
                case ["$", "cd", var target]:
                    dirStack = target switch
                    {
                        ".." => dirStack.IsEmpty ? dirStack : dirStack.Pop(),
                        _ => dirStack.Push(dirStack.Peek().Items[target] as FsNode.Dir ??
                                           throw new ArgumentException("Cannot cd into file"))
                    };
                    break;
            }
        }

        return tree;
    }

    public static int GetDirectorySize(this FsNode.Dir dir) => dir.Items.Select(x => x.Value switch
    {
        FsNode.File f => f.Size,
        FsNode.Dir d => GetDirectorySize(d)
    }).Sum();

    private static IEnumerable<FsNode.Dir> FindDirs(FsNode.Dir fs) =>
        fs.Items.Values.OfType<FsNode.Dir>().SelectMany(FindDirs).Append(fs);

    public static int Part1(FsNode.Dir fs, int maxSize) =>
        FindDirs(fs)
            .Select(x => x.GetDirectorySize())
            .Where(x => x < maxSize)
            .Sum();

    public static int Part2(FsNode.Dir fs, int totalSpace, int spaceNeeded)
    {
        var freeSpace = totalSpace - fs.GetDirectorySize();
        return FindDirs(fs)
            .Select(x => x.GetDirectorySize())
            .Where(x => x + freeSpace > spaceNeeded)
            .Min();
    }
}