using day04;

Console.WriteLine(
    $"Part 1: {Solution.GetNumberOfFullyOverlapping(File.ReadLines("./input.txt"))}"
);
Console.WriteLine(
    $"Part 2: {Solution.GetNumberOfOverlapping(File.ReadLines("./input.txt"))}"
);