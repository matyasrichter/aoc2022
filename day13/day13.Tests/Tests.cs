namespace day13.Tests;

public class Tests
{
    private static readonly string[] Example =
    {
        "[1,1,3,1,1]",
        "[1,1,5,1,1]",
        "",
        "[[1],[2,3,4]]",
        "[[1],4]",
        "",
        "[9]",
        "[[8,7,6]]",
        "",
        "[[4,4],4,4]",
        "[[4,4],4,4,4]",
        "",
        "[7,7,7,7]",
        "[7,7,7]",
        "",
        "[]",
        "[3]",
        "",
        "[[[]]]",
        "[[]]",
        "",
        "[1,[2,[3,[4,[5,6,7]]]],8,9]",
        "[1,[2,[3,[4,[5,6,0]]]],8,9]",
    };

    public static IEnumerable<object[]> P1 => new[]
    {
        new object[]
        {
            "[1,1,3]",
            new ListOrInt.List(new() { new ListOrInt.Int(1), new ListOrInt.Int(1), new ListOrInt.Int(3) })
        },
        new object[]
        {
            "[]",
            new ListOrInt.List(new())
        },
        new object[]
        {
            "[[[]]]",
            new ListOrInt.List(new() { new ListOrInt.List(new() { new ListOrInt.List(new()) }) })
        },
        new object[]
        {
            "[[1],4]",
            new ListOrInt.List(new() { new ListOrInt.List(new() { new ListOrInt.Int(1) }), new ListOrInt.Int(4) })
        },
    };

    [Theory]
    [MemberData(nameof(P1))]
    public void Test_Parsing(string input, ListOrInt expected)
    {
        var result = Solution.ParseLine(input);
        Assert.True(expected.DeepEqualTo(result));
    }

    [Fact]
    public void Test_Part1_Example()
    {
        Assert.Equal(13, Solution.Part1(Example));
    }
    
    [Fact]
    public void Test_Part2_Example()
    {
        Assert.Equal(140, Solution.Part2(Example));
    }
}