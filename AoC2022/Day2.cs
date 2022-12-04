namespace AoC2022;

public class Day2 : SolverBase
{
    protected override long Solve(IEnumerable<string> input)
    {
        var sum = 0;
        foreach (var line in input)
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

        return sum;
    }

    private static int Score(int his, int yours)
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
