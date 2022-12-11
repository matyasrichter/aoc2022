using System.Collections.Immutable;
using System.Numerics;

namespace day11;

public record Monkey(ImmutableList<BigInteger> Items, Func<BigInteger, BigInteger> Operation,
    Func<BigInteger, int> Test)
{
    public BigInteger InspectionCount { get; private init; }

    public (IEnumerable<(BigInteger Level, int ThrowTo)>, Monkey) TurnResult(
        Func<BigInteger, BigInteger> worryLevelReduction)
    {
        return (
            Items.Select(Operation).Select(worryLevelReduction).Select(x => (x, Test(x))),
            this with { Items = ImmutableList<BigInteger>.Empty, InspectionCount = InspectionCount + Items.Count }
        );
    }

    public Monkey WithItem(BigInteger item) =>
        this with { Items = Items.Add(item) };
}