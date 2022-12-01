using System.Collections.Immutable;

IEnumerable<int?> ReadValues(string inputFileName)
{
    foreach (var line in File.ReadLines(inputFileName))
        yield return (line == String.Empty) ? null : int.Parse(line);
}

int GetMax(IEnumerable<int?> elves) =>
    elves
        // append null as the last elf's terminator
        .Append(null)
        .Aggregate<int?, (int Max, int CurrAgg)>(
            (0, 0),
            (agg, curr) => curr is null ? (int.Max(agg.Max, agg.CurrAgg), 0) : (agg.Max, agg.CurrAgg + curr.Value)
        ).Max;

int GetSumOfTopN(IEnumerable<int?> elves, int n)
{
    return elves
        .Append(null)
        .Aggregate((CurrAgg: 0, TopNElves: ImmutableList<int>.Empty),
            (agg, curr) => (agg, curr) switch
            {
                { curr: not null } => (agg.CurrAgg + curr.Value, agg.TopNElves),
                _ => (
                    0,
                    agg.TopNElves.Count < n
                        // if there's less then N elves, this one is one of the top N
                        ? agg.TopNElves.Add(agg.CurrAgg).Sort()
                        : agg.TopNElves.First() < agg.CurrAgg
                            // if the elf with
                            ? agg.TopNElves.RemoveAt(0).Add(agg.CurrAgg).Sort()
                            : agg.TopNElves
                )
            })
        .TopNElves.Sum();
}


const string inputFile = @"./input.txt";
Console.WriteLine($"Max: {GetMax(ReadValues(inputFile))}");
Console.WriteLine($"Sum of top three: {GetSumOfTopN(ReadValues(inputFile), 3)}");