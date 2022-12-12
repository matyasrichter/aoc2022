namespace day12;

public class Tests
{
    private static readonly string[] Example =
    {
        "Sabqponm",
        "abcryxxl",
        "accszExk",
        "acctuvwj",
        "abdefghi",
    };
    [Fact]
    public void Test_Part1_Example()
    {
        Assert.Equal(31, Solution.Part1(Example));
    }
    [Fact]
    public void Test_Part2_Example()
    {
        Assert.Equal(29, Solution.Part2(Example));
    }
}