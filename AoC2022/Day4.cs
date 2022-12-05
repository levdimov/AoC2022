namespace AoC2022;

public class Day4 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        return input
            .Where(i => !string.IsNullOrEmpty(i))
            .Count(i =>
            {
                var parts = i
                    .Split(',')
                    .Select(p => p
                        .Split('-')
                        .Select(int.Parse)
                        .ToArray()
                    )
                    .ToArray();

                return Overlap(parts[0][0], parts[0][1], parts[1][0], parts[1][1]);
            })
            .ToString();
    }

    private static bool Overlap(int a1, int a2, int b1, int b2)
    {
        return (b1 <= a1 && a1 <= b2) || (a1 <= b1 && b1 <= a2);
    }
}
