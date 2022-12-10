namespace AoC2022;

public class Day10 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var register = new Register();

        var signalStrengths = new List<char>();
        signalStrengths.AddRange(input
            .Where(i => !string.IsNullOrEmpty(i))
            .SelectMany(ToCommand)
            .Select((c, i) =>
                    {
                        var value = register.Value;

                        c(register);

                        return new[] { value - 1, value, value + 1 }.Contains(i % 40) ? '#' : '.';
                    }));

        return string.Join(Environment.NewLine, signalStrengths.Chunk(40).Select(c => new string(c)));
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
