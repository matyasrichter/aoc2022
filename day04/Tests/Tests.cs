using day04;

namespace Tests;

public class UnitTest1
{
    private static string[] _example =
    {
        "2-4,6-8",
        "2-3,4-5",
        "5-7,7-9",
        "2-8,3-7",
        "6-6,4-6",
        "2-6,4-8",
    };
    [Fact]
    public void Test_Part1_Example()
    {
        Assert.Equal(2, Solution.GetNumberOfFullyOverlapping(_example));
    }

    [Fact]
    public void Test_Part2_Example()
    {
        Assert.Equal(4, Solution.GetNumberOfOverlapping(_example));
    }
}