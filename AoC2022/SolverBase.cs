using System.Globalization;

namespace AoC2022;

public abstract class SolverBase
{
    [Test]
    public void Solve()
    {
        var answer = Solve(ReadInput());

        Console.WriteLine(answer);
    }

    protected abstract string Solve(IEnumerable<string> input);

    private IEnumerable<string> ReadInput()
    {
        return File.ReadLines($"{GetType().Name.ToLower(CultureInfo.InvariantCulture)}.txt");
    }
}
