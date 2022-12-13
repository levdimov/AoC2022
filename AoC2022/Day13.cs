using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AoC2022;

public class Day13 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var add2 = new Signal(new[] { new Signal(new[] { new Signal(2) }) });
        var add6 = new Signal(new[] { new Signal(new[] { new Signal(6) }) });

        var signalsOrdered = input
            .Where(i => !string.IsNullOrEmpty(i))
            .Select(Signal.Parse)
            .Concat(new[] { add2, add6 })
            .OrderBy(s => s, new SignalComparer())
            .Index(1)
            .ToArray();

        return (signalsOrdered.Single(i => i.Value == add2).Key * signalsOrdered.Single(i => i.Value == add6).Key).ToString();
    }

    private class SignalComparer : IComparer<Signal>
    {
        public int Compare(Signal x, Signal y)
        {
            var c = Day13.Compare(x, y);
            return c switch
            {
                false => 1,
                null => 0,
                true => -1
            };
        }
    }

    private static bool? Compare(Signal s1, Signal s2)
    {
        // если числа
        if (s1.Value is not null && s2.Value is not null)
        {
            if (s1.Value == s2.Value)
            {
                return null;
            }

            return s1.Value < s2.Value;
        }

        //если списки
        if (s1.Value is null || s2.Value is null)
        {
            if (s1.Value is not null)
            {
                s1 = new Signal(new[] { s1 });
            }

            if (s2.Value is not null)
            {
                s2 = new Signal(new[] { s2 });
            }

            for (var si = 0; si < Math.Max(s1.SubSignals.Length, s2.SubSignals.Length); si++)
            {
                var ss1 = s1.SubSignals.ElementAtOrDefault(si);
                var ss2 = s2.SubSignals.ElementAtOrDefault(si);

                if (ss1 is null && ss2 is not null)
                {
                    return true;
                }

                if (ss2 is null && ss1 is not null)
                {
                    return false;
                }

                if (ss1 is null && ss2 is null)
                {
                    throw new InvalidOperationException("1");
                }

                var compareResult = Compare(ss1!, ss2!);
                if (compareResult != null)
                {
                    return compareResult;
                }
            }
        }

        return null;
    }

    private class Signal
    {
        public Signal[] SubSignals { get; } = Array.Empty<Signal>();
        public int? Value { get; }

        public Signal(Signal[] subSignals)
        {
            SubSignals = subSignals;
        }

        public Signal(int? value)
        {
            Value = value;
        }

        public override string ToString()
        {
            if (Value.HasValue)
            {
                return Value.Value.ToString();
            }

            return $"[{string.Join(",", SubSignals.Select(s => s.ToString()))}]";
        }

        public static Signal Parse(string signalDescription)
        {
            return Parse(JToken.Parse(signalDescription));
        }

        private static Signal Parse(JToken token)
        {
            return token.Type switch
            {
                JTokenType.Array => new Signal(token.Select(Parse).ToArray()),
                JTokenType.Integer => new Signal(token.Value<int>()),
                _ => throw new ArgumentOutOfRangeException(nameof(token.Type), token.Type, ":(")
            };
        }
    }
}
