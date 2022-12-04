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

    [Test]
    public async Task Day2()
    {
        var sum = 0;
        await foreach (var line in ReadInput())
        {
            var his = line[0] - 'A' + 1;
            sum += (line[2] - 'X' + 1) switch
            {
                1 => Score(his, his - 1),
                2 => Score(his, his),
                3 => Score(his, (his + 1) % 3),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        Console.WriteLine(sum);

        int Score(int his, int yours)
        {
            his = his == 0 ? 3 : his;
            yours = yours == 0 ? 3 : yours;

            if (his == 1 && yours == 3)
            {
                return yours;
            }

            if ((his == 3 && yours == 1) || his < yours)
            {
                return yours + 6;
            }

            if (his == yours)
            {
                return yours + 3;
            }

            return yours;
        }
    }

    [Test]
    public async Task Day3()
    {
        var sum = 0;
        var counter = 0;
        IEnumerable<char>? items = null;
        await foreach (var line in ReadInput())
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            items = (items ?? line).Intersect(line);
            counter += 1;

            if (counter == 3)
            {
                counter = 0;
                sum += Score(items.Single());
                items = null;
            }
        }

        Console.WriteLine(sum);

        int Score(char item)
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

    private static IAsyncEnumerable<string> ReadInput()
    {
        return File.ReadLinesAsync($"{TestContext.CurrentContext.Test.Name.ToLower(CultureInfo.InvariantCulture)}.txt");
    }
}
