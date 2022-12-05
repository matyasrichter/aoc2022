using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace day05;

using CrateStacksDict = ImmutableDictionary<int, ImmutableStack<char>>;

public record Move(int Count, int From, int To);

public static class Solution
{
    private static CrateStacksDict ParseInitialStack(IEnumerable<string> lines)
    {
        // reverse the lines, because we need to fill the stacks bottom-up
        var linesList = lines.Reverse().SkipWhile(string.IsNullOrEmpty).ToList();
        // dict initialized with all keys and empty stacks
        var result = linesList[0]
            .Trim()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToImmutableDictionary(x => x, _ => ImmutableStack<char>.Empty);

        // skip first (bottom) line with stack labels
        return linesList.Skip(1)
            // for each line, add the elements to the correct stacks
            .Aggregate(result, (current, crate) =>
                // chunk by 4 characters: "[X] "
                crate.Chunk(4)
                    .Zip(current.Keys)
                    // remove brackets and whitespace
                    .Select(stackElem => (
                        StackElement: stackElem.First.FirstOrDefault(x =>
                            x != '[' && x != ']' && !char.IsWhiteSpace(x)),
                        StackLabel: stackElem.Second)
                    )
                    // filter out empty (some stacks have more items than others => extra whitespace)
                    .Where(x => x.StackElement != default)
                    .Aggregate(current,
                        (dict, elem) =>
                            dict.SetItem(elem.StackLabel, current[elem.StackLabel].Push(elem.StackElement))));
    }

    private static readonly Regex MoveRegex = new(@"move (\d+) from (\d+) to (\d+)", RegexOptions.Compiled);

    private static IEnumerable<Move> ParseMoves(IEnumerable<string> lines) =>
        lines
            .Select(x => MoveRegex.Match(x).Groups)
            .Select(x => x.Values.Skip(1).Select(y => int.Parse(y.Value)).ToList())
            .Select(x => new Move(x[0], x[1], x[2]));

    private static (IEnumerable<Move> Moves, CrateStacksDict CrateStack) ParseInput(IEnumerable<string> lines)
    {
        var linesList = lines.ToList();
        return (
            ParseMoves(
                linesList.SkipWhile(x => !x.All(char.IsWhiteSpace)).Skip(1)
            ),
            ParseInitialStack(
                linesList.TakeWhile(x => !x.All(char.IsWhiteSpace))
            )
        );
    }

    public static string GetTopCrates(IEnumerable<string> input)
    {
        var (moves, initialCrates) = ParseInput(input);
        var result = moves
            // unfold each move with count=N to N moves
            .SelectMany(m => Enumerable.Repeat((m.From, m.To), m.Count))
            // now, for each move, pop and push (actually the other way around, because we need to peek the top item first)
            .Aggregate(initialCrates, (crates, move) => crates
                .SetItem(move.To, crates[move.To].Push(crates[move.From].Peek()))
                .SetItem(move.From, crates[move.From].Pop()));
        return string.Join(null, result.Values.Select(x => x.Peek()));
    }

    public static string GetTopCratesPart2(IEnumerable<string> input)
    {
        var (moves, initialCrates) = ParseInput(input);
        var result = moves
            .Aggregate(initialCrates, (crates, move) =>
            {
                var movedCrates = ImmutableList<char>.Empty;
                // repeat move.Count times: pop from move.From
                (movedCrates, crates) = Enumerable.Range(0, move.Count)
                    .Aggregate((Moved: movedCrates, Crates: crates), (tmpState, _) => (
                        tmpState.Moved.Add(tmpState.Crates[move.From].Peek()), tmpState.Crates
                            .SetItem(move.From, tmpState.Crates[move.From].Pop())));
                // push to move.To in reverse order
                return movedCrates.Reverse().Aggregate(crates,
                    (tmpCrates, crateLabel) => tmpCrates.SetItem(move.To, tmpCrates[move.To].Push(crateLabel)));
            });
        return string.Join(null, result.Values.Select(x => x.Peek()));
    }
}