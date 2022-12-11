using System.Collections.Immutable;
using System.Numerics;
using System.Text.RegularExpressions;

namespace day11;

public static partial class Solution
{
    [GeneratedRegex(@"  Starting items:( (\d+),?)+", RegexOptions.Compiled)]
    private static partial Regex StartingItemsRegex();

    [GeneratedRegex(@"  Operation: new = (old|[0-9]+) ([+\-*/]) (old|[0-9]+)", RegexOptions.Compiled)]
    private static partial Regex OperationRegex();

    private static ImmutableList<BigInteger> ParseStartingItems(string line) =>
        StartingItemsRegex()
            .Match(line)
            .Groups[2]
            .Captures
            .Select(x => BigInteger.Parse(x.Value))
            .ToImmutableList();

    private static Func<BigInteger, BigInteger> ParseOperand(string match) => match switch
    {
        "old" => x => x,
        var str => BigInteger.TryParse(str, out var num) switch
        {
            true => _ => num,
            false => throw new ArgumentException("Invalid operand in operation: {X}", str),
        }
    };

    private static Func<BigInteger, BigInteger> ParseOperation(string line)
    {
        var match = OperationRegex().Match(line);
        var l = ParseOperand(match.Groups[1].Value);
        var r = ParseOperand(match.Groups[3].Value);
        Func<BigInteger, BigInteger, BigInteger> op = match.Groups[2].Value switch
        {
            "+" => (a, b) => a + b,
            "-" => (a, b) => a - b,
            "*" => (a, b) => a * b,
            "/" => (a, b) => a / b,
            _ => throw new ArgumentException("Invalid operation")
        };
        return (value) => op(l(value), r(value));
    }

    private static (Func<BigInteger, int> Test, BigInteger Div) ParseTest(string[] lines)
    {
        var divisibleBy = BigInteger.Parse(lines[0].Split(' ').Last());
        var trueOpt = BigInteger.Parse(lines[1].Split(' ').Last());
        var falseOpt = BigInteger.Parse(lines[2].Split(' ').Last());
        return (value => (int)(value % divisibleBy == 0 ? trueOpt : falseOpt), divisibleBy);
    }

    internal static (Monkey monkey, BigInteger div) ParseMonkey(List<string> x)
    {
        var (test, div) = ParseTest(x.Skip(3).Take(3).ToArray());
        return (new(
            ParseStartingItems(x[1]),
            ParseOperation(x[2]),
            test
        ), div);
    }


    internal static IEnumerable<(Monkey monkey, BigInteger div)> ParseMonkeys(this IEnumerable<string> lines) =>
        lines
            .Where(x => x != string.Empty)
            .Chunk(6)
            .Select(x => x.ToList())
            .Select(ParseMonkey);

    private static BigInteger GetMonkeyBusinessLevel(
        this IEnumerable<Monkey> parsedMonkeys,
        int rounds,
        Func<BigInteger, BigInteger> worryLevelReduction
    ) =>
        Enumerable.Repeat<byte>(default, rounds)
            .Aggregate(parsedMonkeys.ToList(), (m, _) =>
            {
                for (var i = 0; i < m.Count; i++)
                {
                    (var actions, m[i]) = m[i].TurnResult(worryLevelReduction);
                    foreach (var (level, throwTo) in actions)
                    {
                        m[throwTo] = m[throwTo].WithItem(level);
                    }
                }

                return m;
            })
            .Select(x => x.InspectionCount)
            .Order()
            .Reverse()
            .Take(2)
            .Aggregate((agg, x) => agg * x);

    public static BigInteger Part1(IEnumerable<string> lines) =>
        lines
            .ParseMonkeys()
            .Select(x => x.monkey)
            .GetMonkeyBusinessLevel(20, x => x / 3);

    public static BigInteger Part2(IEnumerable<string> lines)
    {
        var (monkeys, lcm) = lines
            .ParseMonkeys()
            .Aggregate((monkeys: ImmutableList<Monkey>.Empty, lcm: BigInteger.One), (agg, curr) => (
                agg.monkeys.Add(curr.monkey),
                agg.lcm * curr.div / BigInteger.GreatestCommonDivisor(agg.lcm, curr.div)
            ));
        return monkeys.GetMonkeyBusinessLevel(10000, x => x % lcm);
    }
}