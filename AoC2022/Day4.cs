namespace AoC2022;

public class Day4 : SolverBase
{
    protected override long Solve(IEnumerable<string> input)
    {
        return input.Where(i => !string.IsNullOrEmpty(i)).Count(i =>
        {
            var parts = i.Split(',');
            var (a, b) = (parts[0], parts[1]);
            var aParts = a.Split('-').Select(int.Parse).OrderBy(_ => _).ToArray();
            var bParts = b.Split('-').Select(int.Parse).OrderBy(_ => _).ToArray();

            return Overlap(aParts[0], aParts[1], bParts[0], bParts[1]);
        });
    }

    private static bool Overlap(int a1, int a2, int b1, int b2)
    {
        return (b1 <= a1 && a1 <= b2) || (a1 <= b1 && b1 <= a2);
    }
}
