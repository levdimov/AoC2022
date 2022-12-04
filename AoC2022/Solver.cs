using System.Globalization;

namespace AoC2022;

public class Solver
{
    private static IAsyncEnumerable<string> ReadInput()
    {
        return File.ReadLinesAsync($"{TestContext.CurrentContext.Test.Name.ToLower(CultureInfo.InvariantCulture)}.txt");
    }
}
