using day10;

Console.WriteLine($"Part 1: {Solution.Part1(File.ReadLines("./input.txt"))}");

Console.WriteLine("Part 2:");
foreach (var line in Solution.Part2(File.ReadLines("./input.txt")))
{
    foreach (var c in line)
    {
        Console.Write(c);
    }
    Console.WriteLine();
}