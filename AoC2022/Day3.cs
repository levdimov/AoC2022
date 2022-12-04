namespace AoC2022;

public class Day3 : SolverBase
{
    protected override long Solve(IEnumerable<string> input)
    {
        return input
            .Where(i => !string.IsNullOrEmpty(i))
            .Chunk(3)
            .Select(lines => lines
                .Select(l => l.AsEnumerable())
                .Aggregate((a, b) => a.Intersect(b)))
            .Select(items => Score(items.Single()))
            .Sum();
    }

    private static int Score(char item)
    {
        if (item == 0)
        {
            return 0;
        }

        if (char.IsLower(item))
        {
            return item - 'a' + 1;
        }

        return item - 'A' + 27;
    }
}
