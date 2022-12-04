using System.Text.RegularExpressions;

namespace day04;

public static class Solution
{
    private static readonly Regex Regex = new(@"(\d+)-(\d+),(\d+)-(\d+)", RegexOptions.Compiled);

    private static
        IEnumerable<((int From, int To) Left, (int From, int To) Right)> ParseLines(IEnumerable<string> lines) =>
        lines
            .Select(it => Regex.Match(it))
            // skip full match, we're only interested in group matches
            .Select(it => it.Groups.Values.Skip(1))
            .Select(it => it.Select(x => int.Parse(x.Value)))
            .Select(it => it.ToList())
            .Select(it => ((it[0], it[1]), (it[2], it[3])));

    private static bool IsRangeSubsetOf((int From, int To) left, (int From, int To) right) =>
        right.From <= left.From && right.To >= left.To;

    private static bool IsInRange(int value, (int From, int To) range) => range.From <= value && range.To >= value;

    private static bool AreRangesOverlapping((int From, int To) left, (int From, int To) right) =>
        // one of the bounds of left must be inside right 
        IsInRange(left.From, right) 
        || IsInRange(left.To, right) 
        // OR left must fully contain right
        || IsRangeSubsetOf(right, left);

    public static int GetNumberOfFullyOverlapping(IEnumerable<string> lines) =>
        ParseLines(lines)
            .Count(it => IsRangeSubsetOf(it.Left, it.Right) || IsRangeSubsetOf(it.Right, it.Left));

    public static int GetNumberOfOverlapping(IEnumerable<string> lines) =>
        ParseLines(lines)
            .Count(it => AreRangesOverlapping(it.Left, it.Right));
}