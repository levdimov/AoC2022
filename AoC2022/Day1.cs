namespace AoC2022;

public class Day1 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var max = new SortedSet<long>();
        var current = 0L;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                max.Add(current);
                current = 0;
                continue;
            }

            current += long.Parse(line);
        }

        max.Add(current);

        return max.TakeLast(3).Sum().ToString();
    }
}
