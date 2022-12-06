namespace day06;

public static class Solution
{
    public static int GetMarkerPosition(string input, int markerSize)
    {
        return input
            // add indices
            .Select((c, i) => (c, i))
            // no point in checking the first N
            .Skip(markerSize - 1)
            // check if all characters on positions (i-marker) through i are unique
            .First(x => input[int.Max(x.i - markerSize, 0)..x.i].Distinct().Count() == markerSize)
            // return the index we found
            .i;
    }
}