using day05;

namespace Tests;

public class Tests
{
    private static readonly string[] Example =
    {
        "    [D]     ",
        "[N] [C]     ",
        "[Z] [M] [P] ",
        "1   2   3   ",
        "",
        "move 1 from 2 to 1",
        "move 3 from 1 to 3",
        "move 2 from 2 to 1",
        "move 1 from 1 to 2",
    };
    [Fact]
    public void Test1_Part1_Example()
    {
        Assert.Equal("CMZ", Solution.GetTopCrates(Example));
    }
    
    [Fact]
    public void Test1_Part2_Example()
    {
        Assert.Equal("MCD", Solution.GetTopCratesPart2(Example));
    }
}