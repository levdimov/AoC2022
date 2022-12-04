namespace AoC2022;

public class Day3 : SolverBase
{
    protected override long Solve(IEnumerable<string> input)
    {
        var sum = 0;
        var counter = 0;
        IEnumerable<char>? items = null;
        foreach (var line in input)
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

        return sum;
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
