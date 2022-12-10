using day09;

namespace Tests;

public class Tests
{
    private static readonly string[] Example =
    {
        "R 4",
        "U 4",
        "L 3",
        "D 1",
        "R 4",
        "D 1",
        "L 5",
        "R 2",
    };

    [Fact]
    public void Test_Part1_Example()
    {
        Assert.Equal(13, Solution.DistinctTailPositions(Example, 2));
    }

    [Fact]
    public void Test_Part2_Example()
    {
        Assert.Equal(1, Solution.DistinctTailPositions(Example, 10));
    }

    private static readonly string[] Example2 =
    {
        "R 5",
        "U 8",
        "L 8",
        "D 3",
        "R 17",
        "D 10",
        "L 25",
        "U 20",
    };

    [Fact]
    public void Test_Part2_Example2()
    {
        Assert.Equal(36, Solution.DistinctTailPositions(Example2, 10));
    }
}