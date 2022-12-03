using day03;

Console.WriteLine(
    $"Part 1: {Solution.GetSumOfCommonItems(File.ReadLines("./input.txt"))}"
);
Console.WriteLine(
    $"Part 2: {Solution.GetSumOfGroupItems(File.ReadLines("./input.txt"), 3)}"
);