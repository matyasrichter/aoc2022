using System.Collections.Immutable;

namespace day12;

public static class Solution
{
    private record struct Coords(int X, int Y);

    private record struct Maze(Coords Start, Coords Goal, int[][] Cells);

    private static Maze ToMaze(this (Coords? Start, Coords? End, ImmutableList<ImmutableList<int>> Lines) x) =>
        x is not { Start: { }, End: { } }
            ? throw new ArgumentException("Missing start/end")
            : new(x.Start.Value, x.End.Value, x.Lines.Select(y => y.ToArray()).ToArray());

    private static Maze ParseMaze(this IEnumerable<string> lines) =>
        lines
            .Select(x => x
                .Select((y, j) => (y, j))
                .Aggregate(
                    (Start: null as int?, End: null as int?, Line: ImmutableList<int>.Empty),
                    (agg, c) => (
                        Start: c.y == 'S' ? c.j : agg.Start,
                        End: c.y == 'E' ? c.j : agg.End,
                        Line: agg.Line.Add(c.y switch
                            {
                                'S' => 0,
                                'E' => 'z' - 'a',
                                _ => c.y - 'a'
                            })
                    )
                ))
            .Select((x, i) => (x, i))
            .Aggregate(
                (Start: null as Coords?, End: null as Coords?, Lines: ImmutableList<ImmutableList<int>>.Empty),
                (agg, curr) => (
                    Start: curr.x.Start is not null ? new Coords(curr.x.Start.Value, curr.i) : agg.Start,
                    End: curr.x.End is not null ? new Coords(curr.x.End.Value, curr.i) : agg.End,
                    Lines: agg.Lines.Add(curr.x.Line)
                ))
            .ToMaze();

    private static readonly (int X, int Y)[] Moves = { (0, 1), (0, -1), (1, 0), (-1, 0) };

    private static IEnumerable<Coords> GetNeighbours(this Coords c, int[][] maze)
    {
        return Moves
            .Select(x => new Coords(c.X + x.X, c.Y + x.Y))
            .Where(x => x is { X: >= 0, Y: >= 0 } && x.X < maze[0].Length && x.Y < maze.Length)
            .Where(x => maze[c.Y][c.X] - maze[x.Y][x.X] <= 1);
    }

    private static int FindPathLen(int[][] maze, Coords start, Func<Coords, bool> isGoal)
    {
        var q = new Queue<Coords>();
        var visited = new HashSet<Coords>();
        var pathLen = new Dictionary<Coords, int> { { start, 0 } };
        q.Enqueue(start);
        while (q.Count > 0)
        {
            var v = q.Dequeue();
            if (isGoal(v))
                return pathLen[v];
            if (visited.Contains(v)) continue;
            foreach (var neighbour in GetNeighbours(v, maze))
            {
                if (visited.Contains(neighbour)) continue;
                q.Enqueue(neighbour);
                pathLen[neighbour] = pathLen[v] + 1;
            }

            visited.Add(v);
        }

        return -1;
    }

    public static int Part1(IEnumerable<string> lines)
    {
        var maze = lines.ParseMaze();
        return FindPathLen(maze.Cells, maze.Goal, x => x == maze.Start);
    }

    public static int Part2(IEnumerable<string> lines)
    {
        var maze = lines.ParseMaze();
        return FindPathLen(maze.Cells, maze.Goal, x => maze.Cells[x.Y][x.X] == 0);
    }
}