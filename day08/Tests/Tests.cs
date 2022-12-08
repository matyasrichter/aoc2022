using day08;

namespace Tests;

public class Tests
{
    private readonly string[] _exampleGrid = new[]
    {
        "30373",
        "25512",
        "65332",
        "33549",
        "35390",
    };

    [Fact]
    public void Test_Part1_Example()
    {
        Assert.Equal(21, Solution.Part1(_exampleGrid));
    }
    
    [Fact]
    public void Test_Part2_Example()
    {
        Assert.Equal(8, Solution.Part2(_exampleGrid));
    }
}