namespace Solution;

public static class Solution
{
    public static IEnumerable<(char Our, char Their)> ReadValues(string inputFileName)
    {
        foreach (var line in File.ReadLines(inputFileName))
        {
            var chars = line.Split(" ");
            yield return (chars[1][0], chars[0][0]);
        }
    }

    public static int GetResultPoints((char Our, char Their) moves) => moves switch
    {
        ('X', 'A') => 3,
        ('Y', 'B') => 3,
        ('Z', 'C') => 3,
        ('X', 'B') => 0,
        ('X', 'C') => 6,
        ('Y', 'A') => 6,
        ('Y', 'C') => 0,
        ('Z', 'B') => 6,
        ('Z', 'A') => 0,
        _ => throw new ArgumentOutOfRangeException(nameof(moves), moves, null)
    };

    public static int GetRoundPointsPart1((char Our, char Their) round)
    {
        var moveValue = round.Our switch
        {
            'X' => 1,
            'Y' => 2,
            'Z' => 3,
            _ => throw new ArgumentOutOfRangeException()
        };

        return moveValue + GetResultPoints(round);
    }

    private static readonly List<char> Options = new() { 'X', 'Y', 'Z' };

    public static int GetRoundPointsPart2((char Our, char Their) round)
    {
        var targetPoints = round.Our switch { 'X' => 0, 'Y' => 3, 'Z' => 6 };
        char? move = null;
        foreach (var option in Options)
        {
            if (GetResultPoints((option, round.Their)) == targetPoints)
            {
                move = option;
                break;
            }
        }

        if (move is null) throw new InvalidOperationException();
        return GetRoundPointsPart1((move.Value, round.Their));
    }

    public static int GetPointsSum(IEnumerable<(char Our, char Their)> rounds,
        Func<(char Our, char Their), int> calcFunc)
    {
        return rounds.Select(calcFunc).Sum();
    }
}