using day03;

namespace day03Tests;

public class Tests
{
    private static string[] _example = {
        "vJrwpWtwJgWrhcsFMMfFFhFp",
        "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
        "PmmdzqPrVvPwwTWBwg",
        "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
        "ttgJtRGJQctTZtZT",
        "CrZsJsPPZsGzwwsLwLmpwMDw",
    };
    [Fact]
    public void Test_Part1_Example()
    {
        Assert.Equal(157, Solution.GetSumOfCommonItems(_example));
    }
    
    [Fact]
    public void Test_Part2_Example()
    {
        Assert.Equal(70, Solution.GetSumOfGroupItems(_example, 3));
    }
}