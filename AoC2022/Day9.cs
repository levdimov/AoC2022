using static System.Math;

namespace AoC2022;

public class Day9 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var rope = new Rope();

        foreach (var step in input.Where(i => !string.IsNullOrEmpty(i)))
        {
            var p = step.Split(" ");
            var delta = p[0] switch
            {
                "R" => (0, 1),
                "L" => (0, -1),
                "D" => (-1, 0),
                "U" => (1, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
            for (var steps = 0; steps < int.Parse(p[1]); steps++)
            {
                rope.Move(delta);
            }
        }

        return rope.TotalVisited.ToString();
    }

    private class Knot
    {
        public int Y { get; set; }
        public int X { get; set; }

        public Knot(int y, int x)
        {
            Y = y;
            X = x;
        }

        public void Move((int y, int x) delta)
        {
            Y += delta.y;
            X += delta.x;
        }

        public static implicit operator (int y, int x)(Knot t) => (t.Y, t.X);

        public override string ToString() => $"{Y},{X}";
    }

    private class Rope
    {
        public Knot Head { get; } = new(0, 0);
        public Knot[] Tail { get; } = Enumerable.Range(0, 9).Select(_ => new Knot(0, 0)).ToArray();

        private readonly HashSet<(int, int)> visited = new();

        public int TotalVisited => visited.Count;

        public Rope()
        {
            visited.Add(Tail.Last());
        }

        public void Move((int y, int x) delta)
        {
            Head.Move(delta);

            var previousKnot = Head;
            foreach (var knot in Tail)
            {
                var distY = previousKnot.Y - knot.Y;
                var distX = previousKnot.X - knot.X;
                if (Abs(distY) <= 1 && Abs(distX) <= 1)
                {
                    return;
                }

                (int y, int x) tailDelta;
                if (distY == 0 || distX == 0)
                {
                    tailDelta = distY == 0
                        ? (0, distX / Abs(distX))
                        : (distY / Abs(distY), 0);
                }
                else
                {
                    tailDelta = (distY / Abs(distY), distX / Abs(distX));
                }

                knot.Move(tailDelta);
                previousKnot = knot;
            }

            visited.Add(Tail.Last());
        }
    }
}
