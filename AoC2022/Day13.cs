﻿namespace AoC2022;

public class Day13 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var signalPairs = input
            .Where(i => !string.IsNullOrEmpty(i))
            .Chunk(2)
            .Select(p => p.Select(Signal.Parse).ToArray())
            .ToArray();

        var comparisonResults = signalPairs
            .Select((signalPair, i) => (Compare(signalPair[0], signalPair[1]), i + 1))
            .ToArray();

        return comparisonResults
            .Where(r => r.Item1 ?? true)
            .Sum(r => r.Item2)
            .ToString();
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

        private Signal(int? value)
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
            signalDescription = signalDescription[1..^1];

            if (string.IsNullOrEmpty(signalDescription))
            {
                return new Signal(Array.Empty<Signal>());
            }

            var subSignals = new Queue<Signal>();
            while (true)
            {
                var (subSignalBegin, subSignalEnd) = GetSubSignalIndexes(signalDescription);
                if (subSignalBegin >= 0)
                {
                    subSignals.Enqueue(Parse(signalDescription[subSignalBegin..(subSignalEnd + 1)]));

                    signalDescription = signalDescription[..subSignalBegin] + signalDescription[(subSignalEnd + 1)..];
                }
                else
                {
                    var integers = signalDescription
                        .Split(",")
                        .Select(token => string.IsNullOrEmpty(token) ? subSignals.Dequeue() : new Signal(int.Parse(token)))
                        .ToArray();

                    return new Signal(integers);
                }
            }
        }

        private static (int, int) GetSubSignalIndexes(string signalDescription)
        {
            var subSignalBegin = signalDescription.IndexOf('[');
            var subSignalEnd = -1;

            if (subSignalBegin >= 0)
            {
                var subSignalsCount = 0;
                for (var i = subSignalBegin; i < signalDescription.Length; i++)
                {
                    if (signalDescription[i] == '[')
                    {
                        subSignalsCount += 1;
                        continue;
                    }

                    if (signalDescription[i] == ']')
                    {
                        subSignalsCount -= 1;
                        if (subSignalsCount == 0)
                        {
                            subSignalEnd = i;
                            break;
                        }

                        continue;
                    }
                }
            }

            return (subSignalBegin, subSignalEnd);
        }
    }
}