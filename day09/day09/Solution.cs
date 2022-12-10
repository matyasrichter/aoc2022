using System.Collections.Immutable;

namespace day09;

public static class Solution
{
    public static int DistinctTailPositions(IEnumerable<string> lines, int length) =>
        ParseInput(lines)
            .GetPositions(length)
            .Select(x => x.Last())
            .Distinct()
            .Count();

    private static IEnumerable<IEnumerable<Pos>> GetPositions(this IEnumerable<Move> moves, int length) =>
        moves
            .AggregateWithIntermediate(Enumerable.Repeat(new Pos(0, 0), length).ToImmutableList(), (positions, move) =>
            {
                var nextH = positions[0].Apply(move);
                return positions.Skip(1)
                    .ApplySlidingCalculation(nextH, GetNext)
                    .Prepend(nextH)
                    .ToImmutableList();
            });

    private static Pos GetNext(Pos pos, Pos nextPredPos) =>
        nextPredPos.IsTouching(pos)
            ? pos
            : pos.Apply(new(int.Sign(nextPredPos.X - pos.X), int.Sign(nextPredPos.Y - pos.Y)));

    private record Move(int X, int Y);

    private record Pos(int X, int Y)
    {
        public bool IsTouching(Pos other) => int.Abs(X - other.X) <= 1 && int.Abs(Y - other.Y) <= 1;
        public Pos Apply(Move move) => new(X + move.X, Y + move.Y);
    }

    private static IEnumerable<Move> ParseInput(IEnumerable<string> input) =>
        input
            .Select(x => x.Split(' '))
            .Select(x => (x[0], x[1]))
            .Select(x => (
                x.Item1 switch
                {
                    "U" => new Move(0, 1),
                    "D" => new(0, -1),
                    "L" => new(-1, 0),
                    "R" => new(1, 0),
                    _ => throw new ArgumentException($"Invalid move {x.Item1}")
                },
                int.Parse(x.Item2)
            ))
            .SelectMany(x => Enumerable.Repeat(x.Item1, x.Item2));

    private static IEnumerable<TAccumulate> AggregateWithIntermediate<TSource, TAccumulate>(
        this IEnumerable<TSource> source,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func
    ) => source
        .Aggregate((Acc: ImmutableList<TAccumulate>.Empty, Val: seed), (acc, item) =>
        {
            var next = func(acc.Val, item);
            return (acc.Acc.Add(next), next);
        }).Acc;

    private static IEnumerable<T> ApplySlidingCalculation<T>(
        this IEnumerable<T> source, T init, Func<T, T, T> func
    )
    {
        foreach (var item in source)
        {
            var next = func(item, init);
            yield return next;
            init = next;
        }
    }
}