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
                rope.MoveHead(delta);
            }
        }

        return rope.TotalVisited.ToString();
    }

    private class Rope
    {
        public (int y, int x) Head { get; private set; } = (0, 0);
        public (int y, int x) Tail { get; private set; } = (0, 0);

        private readonly HashSet<(int, int)> visited = new();

        public int TotalVisited => visited.Count;

        public Rope()
        {
            visited.Add(Tail);
        }

        public void MoveHead((int y, int x) delta)
        {
            Head = (Head.y + delta.y, Head.x + delta.x);

            var distY = Head.y - Tail.y;
            var distX = Head.x - Tail.x;
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

            Tail = (Tail.y + tailDelta.y, Tail.x + tailDelta.x);
            visited.Add(Tail);
        }
    }
}
