namespace AoC2022;

public class Day6 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var buffer = input.Single();

        const int length = 14;

        for (var i = 0; i < buffer.Length - length; i++)
        {
            if (buffer[i..(i + length)].Distinct().Count() == length)
            {
                return (i + length).ToString();
            }
        }

        throw new ArgumentOutOfRangeException();
    }
}
