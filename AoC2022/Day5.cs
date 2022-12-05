namespace AoC2022;

public class Day5 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var (stacks, commands) = ParseInput(input);

        foreach (var command in commands.Select(ParseCommand))
        {
            Move2(stacks, command);
        }

        return new string(stacks.Where(s => s.Any()).Select(s => s.Peek()).ToArray().ToArray());
    }

    private static void Move1(Stack<char>[] stacks, (int howMany, int from, int to) command)
    {
        for (var i = 0; i < command.howMany; i++)
        {
            stacks[command.to].Push(stacks[command.from].Pop());
        }
    }

    private static void Move2(Stack<char>[] stacks, (int howMany, int from, int to) command)
    {
        var pile = Enumerable.Range(0, command.howMany).Select(_ => stacks[command.from].Pop());

        foreach (var crate in pile.Reverse())
        {
            stacks[command.to].Push(crate);
        }
    }

    private static (Stack<char>[] stacks, IEnumerable<string> input) ParseInput(IEnumerable<string> input)
    {
        var stacksDescription = input.TakeWhile(i => !string.IsNullOrEmpty(i)).ToArray();

        var stacks = new List<Stack<char>>();
        var stacksCount = int.Parse(stacksDescription[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        for (var stackIndex = 0; stackIndex < stacksCount; stackIndex++)
        {
            var stack = new Stack<char>();
            foreach (var layer in stacksDescription[..^1].Reverse())
            {
                var crateIndex = 1 + (4 * stackIndex);
                if (crateIndex >= layer.Length)
                {
                    continue;
                }

                var crate = layer[crateIndex];
                if (!char.IsWhiteSpace(crate))
                {
                    stack.Push(crate);
                }
            }

            stacks.Add(stack);
        }

        return (stacks.ToArray(), input.SkipWhile(i => !string.IsNullOrEmpty(i)).Skip(1));
    }

    private static (int howMany, int from, int to) ParseCommand(string command)
    {
        var moveFromAndToParts = command.Split("to");
        var to = int.Parse(moveFromAndToParts.Last().Trim()) - 1;
        var moveAndFromParts = moveFromAndToParts.First().Split("from");
        var from = int.Parse(moveAndFromParts.Last().Trim()) - 1;
        var moveParts = moveAndFromParts.First().Split("move");
        var howMany = int.Parse(moveParts.Last().Trim());

        return (howMany, from, to);
    }
}
