using System.Text.Json;
using System.Text.Json.Serialization;

namespace day13;

public record ListOrInt
{
    public record List(List<ListOrInt> L) : ListOrInt;

    public record Int(int X) : ListOrInt;

    private ListOrInt()
    {
    }

    public bool DeepEqualTo(ListOrInt r) =>
        (l: this, r) switch
        {
            (l: Int li, r: Int ri) => li.X == ri.X,
            (l: List ll, r: List rl) => ll.L.Count == rl.L.Count
                                        && ll.L.Zip(rl.L).All(x => x.First.DeepEqualTo(x.Second)),
            _ => false
        };
}

public static class Solution
{
    private static ListOrInt ParseListOrInt(JsonElement x) =>
        x switch
        {
            { ValueKind: JsonValueKind.Number } => new ListOrInt.Int(x.GetInt32()),
            { ValueKind: JsonValueKind.Array } =>
                new ListOrInt.List(x.EnumerateArray().Select(ParseListOrInt).ToList()),
            _ => throw new ArgumentException($"Value is not neither list nor int: {x}")
        };

    internal static ListOrInt ParseLine(string line) =>
        new ListOrInt.List(
            (JsonSerializer.Deserialize<IEnumerable<JsonElement>>(line) ??
             throw new ArgumentException($"Invalid line: {line}"))
            .Select(ParseListOrInt)
            .ToList()
        );

    private static IEnumerable<(ListOrInt, ListOrInt)> ParseInput(this IEnumerable<string> lines) =>
        lines
            .Where(x => x != string.Empty)
            .Select(ParseLine)
            .Chunk(2)
            .Select(x => (x[0], x[1]));

    private static int Compare(ListOrInt l, ListOrInt r) => (l, r) switch
    {
        (ListOrInt.Int li, ListOrInt.Int ri) => li.X - ri.X,
        (ListOrInt.Int, ListOrInt.List) => Compare(new ListOrInt.List(new() { l }), r),
        (ListOrInt.List, ListOrInt.Int) => Compare(l, new ListOrInt.List(new() { r })),
        (ListOrInt.List ll, ListOrInt.List rl) => ll.L.Zip(rl.L)
                .Select(x => Compare(x.First, x.Second))
                .Select((x, i) => (x, i))
                .FirstOrDefault(x => x.x != 0) switch
            {
                { x: 0 } => ll.L.Count - rl.L.Count,
                var x => x.x
            }
    };

    public static int Part1(IEnumerable<string> lines) =>
        lines
            .ParseInput()
            .Select((x, i) => (Compare(x.Item1, x.Item2), i))
            .Where(x => x.Item1 < 0)
            .Select(x => x.i + 1)
            .Sum();

    private static readonly ListOrInt[] DividerPackets = {
        new ListOrInt.List(new() { new ListOrInt.List(new() { new ListOrInt.Int(2) }) }),
        new ListOrInt.List(new() { new ListOrInt.List(new() { new ListOrInt.Int(6) }) })
    };

    public static int Part2(IEnumerable<string> lines) =>
        lines
            .Where(x => x != string.Empty)
            .Select(ParseLine)
            .Concat(DividerPackets)
            .Order(Comparer<ListOrInt>.Create(Compare))
            .Select((x, i) => (x, i))
            .Where(x => DividerPackets.Any(d => d.DeepEqualTo(x.x)))
            .Select(x => x.i + 1)
            .Aggregate((a, b) => a * b);
}