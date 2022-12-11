using System.Collections.Immutable;
using System.Numerics;
using day11;

namespace Tests;

public class Tests
{
    private static readonly string[] Example =
    {
        "Monkey 0:",
        "  Starting items: 79, 98",
        "  Operation: new = old * 19",
        "  Test: divisible by 23",
        "    If true: throw to monkey 2",
        "    If false: throw to monkey 3",
        "",
        "Monkey 1:",
        "  Starting items: 54, 65, 75, 74",
        "  Operation: new = old + 6",
        "  Test: divisible by 19",
        "    If true: throw to monkey 2",
        "    If false: throw to monkey 0",
        "",
        "Monkey 2:",
        "  Starting items: 79, 60, 97",
        "  Operation: new = old * old",
        "  Test: divisible by 13",
        "    If true: throw to monkey 1",
        "    If false: throw to monkey 3",
        "",
        "Monkey 3:",
        "  Starting items: 74",
        "  Operation: new = old + 3",
        "  Test: divisible by 17",
        "    If true: throw to monkey 0",
        "    If false: throw to monkey 1",
    };

    public static IEnumerable<object[]> ParsingTestData => new[]
    {
        new object[]
        {
            Example[..6],
            new Monkey(
                ImmutableList<BigInteger>.Empty.AddRange(new BigInteger[] { 79, 98 }), x => x * 19,
                x => x % 23 == 0 ? 2 : 3),
            23
        },

        new object[]
        {
            Example[7..13],
            new Monkey(
                ImmutableList<BigInteger>.Empty.AddRange(new BigInteger[] { 54, 65, 75, 74 }), x => x + 6,
                x => x % 19 == 0 ? 2 : 0),
            19
        },

        new object[]
        {
            Example[14..20],
            new Monkey(
                ImmutableList<BigInteger>.Empty.AddRange(new BigInteger[] { 79, 60, 97 }), x => x * x,
                x => x % 13 == 0 ? 1 : 3),
            13
        },

        new object[]
        {
            Example[21..27],
            new Monkey(
                ImmutableList<BigInteger>.Empty.Add(74), x => x + 3, x => x % 17 == 0 ? 0 : 1),
            17
        },
    };

    [Theory]
    [MemberData(nameof(ParsingTestData))]
    public void Test_Single_Monkey_Parsing(string[] input, Monkey expected, BigInteger expectedDiv)
    {
        var (result, div) = Solution.ParseMonkey(input.ToList());
        Assert.Equal(expected.Items, result.Items);
        Assert.Equal(expectedDiv, div);
        var BigIntegerRange = Enumerable.Range(0, 100).Select(x => (BigInteger)x).ToList();
        Assert.Equal(BigIntegerRange.Select(expected.Operation), BigIntegerRange.Select(result.Operation));
        Assert.Equal(BigIntegerRange.Select(expected.Test), BigIntegerRange.Select(result.Test));
    }

    [Fact]
    public void TestFullMonkeyParsing()
    {
        var r = Example.ParseMonkeys().ToList();
        Assert.Equal(4, r.Count);
    }

    [Fact]
    public void Test_Part1_Example()
    {
        Assert.Equal(10605U, Solution.Part1(Example));
    }

    [Fact]
    public void Test_Part2_Example()
    {
        Assert.Equal(2713310158, Solution.Part2(Example));
    }

    public static object[][] TurnResultData =
    {
        new object[]
        {
            new Monkey(ImmutableList<BigInteger>.Empty.AddRange(new BigInteger[] { 79, 98 }), x => x * 19,
                x => x % 23 == 0 ? 2 : 3),
            new List<(BigInteger, int)>() { (500, 3), (620, 3) }
        },
        new object[]
        {
            new Monkey(ImmutableList<BigInteger>.Empty.AddRange(new BigInteger[] { 79, 98 }), x => x * 19,
                x => x % 23 == 0 ? 2 : 3),
            new List<(BigInteger, int)>() { (500, 3), (620, 3) }
        }
    };

    [Theory]
    [MemberData(nameof(TurnResultData))]
    public void Test_TurnResult(Monkey monkey, IEnumerable<(BigInteger, int)> items)
    {
        Assert.Equal(items, monkey.TurnResult(x => x / 3).Item1);
    }
}