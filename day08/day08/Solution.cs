namespace day08;

public static class Solution
{
    private record struct Item(int H, int I, int J);

    private static Item[][] ParseInput(this IEnumerable<string> lines) =>
        lines.Select(
            (l, i) => l.Select((x, j) =>
                new Item(int.Parse(x.ToString()), i, j)
            ).ToArray()
        ).ToArray();

    private static void UpdateVisibility(IEnumerable<IEnumerable<Item>> grid, ref bool[,] result)
    {
        foreach (var line in grid)
        {
            var max = -1;
            foreach (var item in line)
            {
                if (item.H <= max)
                    continue;
                result[item.I, item.J] = true;
                max = item.H;
            }
        }
    }

    private static IEnumerable<IEnumerable<bool>> BuildVisibilityGrid(this Item[][] grid)
    {
        var result = new bool[grid.Length, grid[0].Length];

        var leftToRight = grid.Select(x => x);
        var rightToLeft = grid.Select(x => x.Reverse());
        var topToBottom = Enumerable.Range(0, grid.Length).Select(i => grid.Select(x => x[i]));
        var bottomToTop = Enumerable.Range(0, grid.Length).Select(i => grid.Select(x => x[i]).Reverse());

        UpdateVisibility(leftToRight, ref result);
        UpdateVisibility(rightToLeft, ref result);
        UpdateVisibility(topToBottom, ref result);
        UpdateVisibility(bottomToTop, ref result);
        return Enumerable
            .Range(0, grid.Length)
            .Select(i =>
                Enumerable
                    .Range(0, grid[0].Length)
                    .Select(j => result[i, j])
            );
    }

    public static int Part1(IEnumerable<string> lines) =>
        lines
            .ParseInput()
            .BuildVisibilityGrid()
            .Select(x => x.Count(y => y))
            .Sum();

    private static readonly (int i, int j)[] Directions = new[]
    {
        (i: 0, j: 1),
        (i: 0, j: -1),
        (i: 1, j: 0),
        (i: -1, j: 0)
    };

    // not too proud of this one 🙃
    private static int GetMaxScenicScore(this Item[][] grid)
    {
        int GetScoreInDirection((int i, int j) coords, (int i, int j) direction, int hThreshold)
        {
            var score = 0;
            do
            {
                coords = (i: coords.i + direction.i, j: coords.j + direction.j);
                if (coords.i < 0 || coords.i >= grid.Length
                                 || coords.j < 0 || coords.j >= grid[0].Length)
                    break;
                score++;
            } while (grid[coords.i][coords.j].H < hThreshold);

            return score;
        }

        int GetScore((int i, int j) coords)
        {
            var threshold = grid[coords.i][coords.j].H;
            return Directions.Select(d => GetScoreInDirection(coords, d, threshold)).Aggregate((a, b) => a * b);
        }

        return grid.Select(x => x.Select(y => GetScore((y.I, y.J))).Max()).Max();
    }

    public static int Part2(IEnumerable<string> lines) =>
        lines
            .ParseInput()
            .GetMaxScenicScore();
}