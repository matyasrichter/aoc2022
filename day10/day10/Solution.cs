namespace day10;

public static class Solution
{
    public static int Part1(IEnumerable<string> lines) =>
        lines
            .ParseLinesToCycleIncrements()
            .Select((x, i) => (Inst: x, Cycle: i + 1))
            .Aggregate((Str: 0, Reg: 1),
                (agg, val) => (
                    (val.Cycle - 20) % 40 == 0 ? agg.Str + val.Cycle * agg.Reg : agg.Str,
                    agg.Reg + val.Inst
                )
            )
            .Str;

    public static IEnumerable<IEnumerable<char>> Part2(IEnumerable<string> lines) =>
        lines
            .ParseLinesToCycleIncrements()
            .Select((x, i) => (Inst: x, Index: i))
            .Aggregate((Out: "", Reg: 1),
                (agg, val) => (
                    agg.Out + (int.Abs(agg.Reg - val.Index % 40) <= 1 ? '#' : '.'),
                    agg.Reg + val.Inst
                )
            )
            .Out
            .Chunk(40);

    private static IEnumerable<int> ParseLinesToCycleIncrements(this IEnumerable<string> lines) =>
        lines.SelectMany(x => x.Split(' ') switch
        {
            ["noop"] => new[] { 0 },
            ["addx", var val] => new[] { 0, int.Parse(val) },
            _ => throw new ArgumentException("Invalid instruction")
        });
}