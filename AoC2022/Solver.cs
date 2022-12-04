using System.Globalization;

namespace AoC2022;

public class Solver
{
    [Test]
    public async Task Day1()
    {
        var max = new SortedSet<long>();
        var current = 0L;
        await foreach (var line in ReadInput())
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

        Console.WriteLine(max.TakeLast(3).Sum());
    }

    private static IAsyncEnumerable<string> ReadInput()
    {
        return File.ReadLinesAsync($"{TestContext.CurrentContext.Test.Name.ToLower(CultureInfo.InvariantCulture)}.txt");
    }
}
