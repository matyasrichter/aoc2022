using System.Text.RegularExpressions;

namespace day15;

public static partial class Solution
{
    public readonly record struct Coord(int X, int Y)
    {
        public int Dist(Coord r) => int.Abs(X - r.X) + int.Abs(Y - r.Y);
    }

    public readonly record struct Sensor(Coord S, Coord Beacon)
    {
        public readonly int Range = S.Dist(Beacon);
        private bool CanSense(Coord c) => S.Dist(c) <= Range;

        public bool Contains(Coord tl, Coord br) =>
            CanSense(tl)
            && CanSense(br)
            && CanSense(new(tl.X, br.Y))
            && CanSense(new(br.X, tl.Y));
    };

    [GeneratedRegex("[-\\d]+", RegexOptions.Compiled)]
    private static partial Regex SensorRegex();

    public static IEnumerable<Sensor> ParseInput(this IEnumerable<string> lines) =>
        lines
            .Select(s => SensorRegex().Matches(s).Select(x => int.Parse(x.Value)).ToList())
            .Select(m => new Sensor(new(m[0], m[1]), new(m[2], m[3])));

    public static int Part1(IEnumerable<string> lines, int y)
    {
        var sensors = lines.ParseInput().ToList();
        var beaconsOnY = sensors.Where(x => x.Beacon.Y == y).Select(x => x.Beacon.X).ToHashSet();
        return sensors
            .SelectMany(s =>
            {
                var rangeLeft = s.Range - int.Abs(s.S.Y - y);
                if (rangeLeft < 0) return Enumerable.Empty<int>();
                return Enumerable.Range(s.S.X - rangeLeft, 2 * rangeLeft + 1);
            })
            .Distinct()
            .Count(x => !beaconsOnY.Contains(x));
    }

    private static int HalfWayTo(this int a, int b) => a + (b - a) / 2;

    public static long Part2(IEnumerable<string> lines, int dim)
    {
        Coord? Solve(List<Sensor> sensors, Coord tl, Coord br)
        {
            if (br.X < tl.X || br.Y < tl.Y || sensors.Any(s => s.Contains(tl, br))) return null;
            if (tl == br)
                return tl;

            // split into quadrants and search again
            return Solve(sensors, tl, new(tl.X.HalfWayTo(br.X), tl.Y.HalfWayTo(br.Y)))
                   ?? Solve(sensors, tl with { X = tl.X.HalfWayTo(br.X) + 1 }, br with { Y = tl.Y.HalfWayTo(br.Y) })
                   ?? Solve(sensors, tl with { Y = tl.Y.HalfWayTo(br.Y) + 1 }, br with { X = tl.X.HalfWayTo(br.X) })
                   ?? Solve(sensors, new(tl.X.HalfWayTo(br.X) + 1, tl.Y.HalfWayTo(br.Y) + 1), br);
        }

        var sensors = lines.ParseInput().ToList();
        var match = Solve(sensors, new(0, 0), new(dim, dim));
        if (match is null)
            throw new ArgumentException("No match found.");
        return match.Value.X * 4000000L + match.Value.Y;
    }
}