namespace AoC2022;

public class Day11 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var monkeys = input
            .Where(i => !string.IsNullOrEmpty(i))
            .Chunk(6)
            .Select(ToMonkey)
            .ToArray();

        MonkeyBusiness.Instance.Monkeys = monkeys;

        var aggregate = monkeys.Select(m => m.TestDiv).Aggregate(1, (a, b) => a * b);

        for (var i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Round(w => w % aggregate);
            }
        }

        var inspections = monkeys
            .Select(m => m.Inspections)
            .OrderByDescending(i => i)
            .ToArray();

        return (inspections[0] * inspections[1]).ToString();
    }

    private Monkey ToMonkey(string[] monkeyDescription)
    {
        var items = monkeyDescription[1]
            .Split("Starting items: ")[1]
            .Split(", ").Select(long.Parse)
            .Select(i => new Item(i))
            .ToArray();

        var operationDescription = monkeyDescription[2].Split("Operation: new = old ")[1].Split(" ");
        long? factor = operationDescription[1] == "old" ? null : long.Parse(operationDescription[1]);
        var div = int.Parse(monkeyDescription[3].Split("Test: divisible by ")[1]);
        Action<Item> operation = operationDescription[0] switch
        {
            "*" => i => i.WorryLevel *= factor ?? i.WorryLevel,
            "+" => i => i.WorryLevel += factor ?? i.WorryLevel,
            _ => throw new ArgumentOutOfRangeException()
        };

        var throwToMonkeyIfTrue = int.Parse(monkeyDescription[4].Split("If true: throw to monkey ")[1]);
        var throwToMonkeyIfFalse = int.Parse(monkeyDescription[5].Split("If false: throw to monkey ")[1]);

        return new Monkey(items, operation, div, throwToMonkeyIfTrue, throwToMonkeyIfFalse, MonkeyBusiness.Instance);
    }

    private class Item
    {
        public long WorryLevel { get; set; }

        public Item(long worryLevel)
        {
            WorryLevel = worryLevel;
        }
    }

    private class MonkeyBusiness
    {
        public static MonkeyBusiness Instance { get; } = new();

        public Monkey[] Monkeys { get; set; }

        public Monkey Get(int n) => Monkeys[n];
    }

    private class Monkey
    {
        private readonly MonkeyBusiness monkeyBusiness;

        public Queue<Item> Items { get; }

        public Action<Item> Operation { get; }

        public int TestDiv { get; }

        public int MonkeyToThrowIfTrue { get; }

        public int MonkeyToThrowIfFalse { get; }

        public long Inspections { get; private set; }

        public Monkey(
            IEnumerable<Item> items,
            Action<Item> operation,
            int testDiv,
            int monkeyToThrowIfTrue,
            int monkeyToThrowIfFalse,
            MonkeyBusiness monkeyBusiness
        )
        {
            this.monkeyBusiness = monkeyBusiness;
            Items = new Queue<Item>(items);
            Operation = operation;
            TestDiv = testDiv;
            MonkeyToThrowIfTrue = monkeyToThrowIfTrue;
            MonkeyToThrowIfFalse = monkeyToThrowIfFalse;
        }

        public void Round(Func<long, long> limit)
        {
            while (Items.Any())
            {
                Inspections += 1;

                var item = Items.Dequeue();

                Operation(item);

                item.WorryLevel = limit(item.WorryLevel);

                if (item.WorryLevel % TestDiv == 0)
                {
                    monkeyBusiness.Get(MonkeyToThrowIfTrue).Catch(item);
                }
                else
                {
                    monkeyBusiness.Get(MonkeyToThrowIfFalse).Catch(item);
                }
            }
        }

        private void Catch(Item item)
        {
            Items.Enqueue(item);
        }
    }
}
