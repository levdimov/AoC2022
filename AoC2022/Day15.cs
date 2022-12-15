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
        const int targetY = 2000000;
        foreach (var sensorC in sensorCs)
        {
            var maxDist = sensorC.Key.Dist(((Sensor)sensorC.Value).BeaconC);
            if (sensorC.Key.Y - maxDist <= targetY && targetY <= sensorC.Key.Y + maxDist)
            {
                var ySpan = Math.Abs(targetY - sensorC.Key.Y);
                var xSpan = maxDist - ySpan;

                for (var x = sensorC.Key.X - xSpan; x <= sensorC.Key.X + xSpan; x++)
                {
                    var newC = new C(x, targetY);
                    if (!points.ContainsKey(newC))
                    {
                        points.Add(newC, new Empty());
                    }
                }
            }
        }

        return points.Where(p => p.Key.Y == targetY).Count(p => p.Value is Empty).ToString();
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
