using System.Collections.Immutable;

namespace day14;

public static class Solution
{
    private static ImmutableHashSet<(int x, int y)> ParseRockPositions(this IEnumerable<string> lines) =>
        lines
            .Select(l => l.Split(" -> "))
            .Select(l => l.Select(c => c.Split(',').Select(int.Parse).ToList()))
            .Select(l => l.Select(c => (X: c[0], Y: c[1])).ToList())
            .Aggregate(
                ImmutableHashSet<(int X, int Y)>.Empty,
                (agg, curr) =>
                {
                    foreach (var (item, i) in curr.Select((x, i) => (x, i)).Skip(1))
                    {
                        var prev = curr[i - 1];
                        var move = (X: int.Sign(item.X - prev.X), Y: int.Sign(item.Y - prev.Y));
                        var step = prev;
                        while (step != item)
                        {
                            agg = agg.Add(step);
                            step = (step.X + move.X, step.Y + move.Y);
                        }
                    }

                    return agg.Add(curr.Last());
                });

    private static (int X, int Y) Add(this (int X, int Y) l, (int X, int Y) r) => (l.X + r.X, l.Y + r.Y);
    private static readonly (int X, int Y)[] Moves = { (0, 1), (-1, 1), (1, 1) };
    private static IEnumerable<(int X, int Y)> GetMoves((int X, int Y) pos) => Moves.Select(x => pos.Add(x));

    public static int Part1(IEnumerable<string> lines)
    {
        var rocks = lines.ParseRockPositions();
        var maxRock = rocks.Select(x => x.y).Max();
        (int X, int Y) sandInit = (500, 0);
        var sand = new HashSet<(int X, int Y)>();
        var i = 0;
        while (true)
        {
            var sandParticle = sandInit;
            while (true)
            {
                if (sandParticle.Y > maxRock)
                    return i;

                var next = GetMoves(sandParticle).FirstOrDefault(x => !rocks.Contains(x) && !sand.Contains(x));
                if (next == default)
                {
                    sand.Add(sandParticle);
                    break;
                }

                sandParticle = next;
            }

            i++;
        }
    }

    public static int Part2(IEnumerable<string> lines)
    {
        var rocks = lines.ParseRockPositions();
        var maxRock = rocks.Select(x => x.y).Max();
        (int X, int Y) sandInit = (500, 0);
        var sand = new HashSet<(int X, int Y)>();
        var i = 0;
        while (!sand.Contains(sandInit))
        {
            var sandParticle = sandInit;
            while (true)
            {
                var next = GetMoves(sandParticle).FirstOrDefault(x => !rocks.Contains(x) && !sand.Contains(x));
                if (next == default || sandParticle.Y + 1 == maxRock + 2)
                {
                    sand.Add(sandParticle);
                    break;
                }

                sandParticle = next;
            }

            i++;
        }

        return i;
    }
}