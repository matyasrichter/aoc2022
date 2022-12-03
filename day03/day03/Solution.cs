namespace day03;

public static class Solution
{
    private static List<int> MapItemsToPriorities(string rucksackContents) =>
        rucksackContents.Select(c => char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27).ToList();

    public static int GetSumOfCommonItems(IEnumerable<string> rucksackStrings) =>
        rucksackStrings
            .Select(MapItemsToPriorities)
            .Select(s => (s.GetRange(0, s.Count / 2), s.GetRange(s.Count / 2, s.Count / 2)))
            .Select(x => (x.Item1.Order(), x.Item2.Order()))
            .Select(x =>
            {
                using var curr2 = x.Item2.GetEnumerator();
                curr2.MoveNext();
                return x.Item1.First(curr1 =>
                {
                    while (curr1 > curr2.Current)
                    {
                        if (!curr2.MoveNext()) throw new ArgumentException("No common item");
                    }

                    return curr1 == curr2.Current;
                });
            })
            .ToList()
            .Sum();

    public static int GetSumOfGroupItems(IEnumerable<string> rucksackStrings, int groupSize) =>
        rucksackStrings
            .Select(MapItemsToPriorities)
            .Select(x => x.Order())
            .Chunk(groupSize)
            .Select(x =>
            {
                var otherRucksacksIterators = x.Skip(1).Select(it => it.GetEnumerator()).ToList();
                foreach (var it in otherRucksacksIterators) it.MoveNext();
                return x.First().First(firstRucksackItem =>
                {
                    foreach (var it in otherRucksacksIterators)
                    {
                        while (firstRucksackItem > it.Current)
                        {
                            if (!it.MoveNext()) throw new ArgumentException("No common item");
                        }
                    }

                    return otherRucksacksIterators.All(it => firstRucksackItem == it.Current);
                });
            })
            .Sum();
}