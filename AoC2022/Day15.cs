using MoreLinq;
using System.Collections.Concurrent;

namespace AoC2022;

public class Day15 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var points = new Dictionary<C, Point>();

        foreach (var l in input)
        {
            var p = l.Split(": closest beacon is at ");

            var bc = C.Parse(p[1]);
            if (!points.ContainsKey(bc))
            {
                points.Add(bc, new Beacon());
            }

            var sc = C.Parse(p[0].Split("Sensor at ")[1]);
            points.Add(sc, new Sensor(bc));
        }

        var sensorCs = points.Where(p => p.Value is Sensor).ToArray();

        var maxY = 4_000_000;
        var maxX = 4_000_000;
        for (var targetY = 0; targetY <= maxY; targetY++)
        {
            var usable = sensorCs
                .Where(s =>
                       {
                           var maxDist = s.Key.Dist(((Sensor)s.Value).BeaconC);
                           return s.Key.Y - maxDist <= targetY &&
                                  targetY <= s.Key.Y + maxDist;
                       })
                .ToArray();

            var segments = new List<(int, int)>();
            foreach (var sensorC in usable)
            {
                var maxDist = sensorC.Key.Dist(((Sensor)sensorC.Value).BeaconC);

                var ySpan = Math.Abs(targetY - sensorC.Key.Y);
                var xSpan = maxDist - ySpan;

                var left = Math.Max(0, sensorC.Key.X - xSpan);
                var right = Math.Min(maxX, sensorC.Key.X + xSpan);

                segments.Add((left, right));
            }

            var merged = Merge(segments.ToArray());

            if (merged.Length == 2 && merged[1].Item1 - merged[0].Item2 == 2)
            {
                var x = merged[0].Item2 + 1;
                var y = targetY;

                Console.WriteLine($"{x},{y}");

                return ((x * 4_000_000L) + y).ToString();
            }
        }

        return "-1";
    }

    public (int, int)[] Merge((int, int)[] intervals)
    {
        intervals = intervals.OrderBy(i => i.Item1).ToArray();

        var stack = new Stack<(int, int)>();

        var curr = intervals[0];

        for (var i = 1; i < intervals.Length; i++)
        {
            var next = intervals[i];
            if (curr.Item2 >= next.Item1)
            {
                curr = (curr.Item1, Math.Max(curr.Item2, next.Item2));
            }
            else
            {
                stack.Push(curr);
                curr = next;
            }
        }

        stack.Push(curr);

        return stack.Reverse().ToArray();
    }

    private record C(int X, int Y)
    {
        public static C Parse(string c)
        {
            var p = c.Split(", ");

            var x = int.Parse(p[0].Split("x=")[1]);
            var y = int.Parse(p[1].Split("y=")[1]);

            return new C(x, y);
        }

        public int Dist(C that) => Math.Abs(X - that.X) + Math.Abs(Y - that.Y);
    }

    private abstract record Point;

    private record Empty : Point;

    private record Sensor(C BeaconC) : Point;

    private record Beacon : Point;
}
