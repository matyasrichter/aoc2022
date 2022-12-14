namespace day14.Tests;

public class Tests
{
    private static readonly string[] Example =
    {
        "498,4 -> 498,6 -> 496,6",
        "503,4 -> 502,4 -> 502,9 -> 494,9",
    };

    [Fact]
    public void Test_Part1_Example()
    {
        Assert.Equal(24, Solution.Part1(Example));
    }
    [Fact]
    public void Test_Part2_Example()
    {
        Assert.Equal(93, Solution.Part2(Example));
    }
}