using Solution;

namespace TestProject1;

public class Tests
{
    [Theory]
    [InlineData('X', 'A', 3)]
    [InlineData('X', 'B', 0)]
    [InlineData('X', 'C', 6)]
    [InlineData('Y', 'A', 6)]
    [InlineData('Y', 'B', 3)]
    [InlineData('Y', 'C', 0)]
    [InlineData('Z', 'A', 0)]
    [InlineData('Z', 'B', 6)]
    [InlineData('Z', 'C', 3)]
    void Test_GetPoints(char our, char their, int expected)
    {
        Assert.Equal(expected, Solution.Solution.GetResultPoints((our, their)));
    }
}