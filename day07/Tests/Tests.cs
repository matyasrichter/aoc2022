using day07;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tests;

public class Tests
{
    private static readonly string[] Example =
    {
        "$ cd /",
        "$ ls",
        "dir a",
        "14848514 b.txt",
        "8504156 c.dat",
        "dir d",
        "$ cd a",
        "$ ls",
        "dir e",
        "29116 f",
        "2557 g",
        "62596 h.lst",
        "$ cd e",
        "$ ls",
        "584 i",
        "$ cd ..",
        "$ cd ..",
        "$ cd d",
        "$ ls",
        "4060174 j",
        "8033020 d.log",
        "5626152 d.ext",
        "7214296 k",
    };

    [Fact]
    public void TestChunkCommands()
    {
        var result = Example.Take(12).ChunkCommands().Select(x => x.ToList()).ToList();
        var expected = new List<List<string>>()
        {
            new() { "$ cd /" },
            new()
            {
                "$ ls",
                "dir a",
                "14848514 b.txt",
                "8504156 c.dat",
                "dir d",
            },
            new() { "$ cd a", },
            new()
            {
                "$ ls",
                "dir e",
                "29116 f",
                "2557 g",
                "62596 h.lst",
            }
        };
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_BuildFsFromExample()
    {
        var root = new FsNode.Dir
        {
            Items =
            {
                ["a"] = new FsNode.Dir
                {
                    Items =
                    {
                        ["f"] = new FsNode.File(29116),
                        ["g"] = new FsNode.File(2557),
                        ["h.lst"] = new FsNode.File(62596),
                        ["e"] = new FsNode.Dir()
                        {
                            Items =
                            {
                                ["i"] = new FsNode.File(584)
                            }
                        }
                    }
                },
                ["b.txt"] = new FsNode.File(14848514),
                ["c.dat"] = new FsNode.File(8504156),
                ["d"] = new FsNode.Dir
                {
                    Items = new()
                    {
                        ["j"] = new FsNode.File(Size: 4060174),
                        ["d.log"] = new FsNode.File(Size: 8033020),
                        ["d.ext"] = new FsNode.File(Size: 5626152),
                        ["k"] = new FsNode.File(Size: 7214296),
                    }
                }
            }
        };

        var result = Solution.BuildFs(Example);
        Assert.True(FsEqual(root, result));
    }

    private static bool FsEqual(FsNode left, FsNode right) =>
        (left, right) switch
        {
            (FsNode.File l, FsNode.File r) => l.Size == r.Size,
            (FsNode.Dir l, FsNode.Dir r) => l.Items.Count == r.Items.Count &&
                                            l.Items.All(x => r.Items.ContainsKey(x.Key)
                                                             && FsEqual(x.Value, r.Items[x.Key])),
            _ => false
        };

    [Fact]
    public void Test_DirectorySize()
    {
        var exampleFs = Solution.BuildFs(Example);
        Assert.Equal(48381165, exampleFs.GetDirectorySize());
    } 

    [Fact]
    public void Test_Part1_Example()
    {
        var exampleFs = Solution.BuildFs(Example);
        Assert.Equal(95437, Solution.Part1(exampleFs, 100000));
    }
    
    [Fact]
    public void Test_Part2_Example()
    {
        var exampleFs = Solution.BuildFs(Example);
        Assert.Equal(24933642, Solution.Part2(exampleFs, 70000000, 30000000));
    }
}