using day07;

var fs = Solution.BuildFs(File.ReadLines("./input.txt"));

Console.WriteLine($"Part 1: {Solution.Part1(fs, 100000)}");
Console.WriteLine($"Part 2: {Solution.Part2(fs, 70000000, 30000000)}");