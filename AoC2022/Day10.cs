namespace AoC2022;

public class Day10 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var register = new Register();

        var signalStrengths = new List<(int, int)> { (0, 0) };
        signalStrengths.AddRange(input
            .Where(i => !string.IsNullOrEmpty(i))
            .SelectMany(ToCommand)
            .Select((c, i) =>
                    {
                        var cycle = i + 1;
                        var value = register.Value;

                        c(register);

                        return (value, cycle * value);
                    }));

        var result = 0;
        for (var i = 20; i < signalStrengths.Count; i += 40)
        {
            result += signalStrengths[i].Item2;
        }

        return result.ToString();
    }

    private Action<Register>[] ToCommand(string commandString)
    {
        var parts = commandString.Split(" ");
        var command = (parts[0], parts.ElementAtOrDefault(1));

        return command switch
        {
            ("addx", _) => new[] { Noop, r => r.Add(int.Parse(command.Item2)) },
            ("noop", _) => new[] { Noop },
            _ => throw new ArgumentOutOfRangeException()
        };

        void Noop(Register _)
        {
        }
    }

    private class Register
    {
        public int Value { get; private set; } = 1;

        public void Add(int addValue)
        {
            Value += addValue;
        }

        public static void Noop() { }
    }
}
