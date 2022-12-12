namespace AoC2022;

public class Day12 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var heightMap = string.Join(string.Empty, input);
        var mapWidth = input.First().Length;
        var mapHeight = heightMap.Length / mapWidth;

        var start = heightMap.IndexOf('S');
        var end = heightMap.IndexOf('E');

        heightMap = heightMap.Replace('S', 'a').Replace('E', 'z');

        var d = new int?[heightMap.Length];
        d[start] = 0;
        var q = new Queue<C>();
        q.Enqueue(new C(start, mapWidth, mapHeight));

        while (q.Any())
        {
            var a = q.Dequeue();
            var current = heightMap[a.MapHeight];
            foreach (var e in a.E.Where(e => heightMap[e.Plain] - heightMap[a.Plain] <= 1))
            {
                var target = heightMap[e.Plain];
                if (!d[e.Plain].HasValue)
                {
                    d[e.Plain] = d[a.Plain] + 1;
                    q.Enqueue(e);
                }
            }
        }

        return d[end].ToString();
    }

    private record C (
        int Plain,
        int MapWidth,
        int MapHeight
    )
    {
        private C New(int plain) => this with { Plain = plain };

        public IEnumerable<C> E
        {
            get
            {
                var (x, y) = D;

                var d = (x, y + 1);
                var r = (x + 1, y);
                var u = (x, y - 1);
                var l = (x - 1, y);

                if (Test(d))
                {
                    yield return New(ToPlain(d));
                }

                if (Test(l))
                {
                    yield return New(ToPlain(l));
                }

                if (Test(u))
                {
                    yield return New(ToPlain(u));
                }

                if (Test(r))
                {
                    yield return New(ToPlain(r));
                }
            }
        }

        private bool Test((int x, int y) c)
        {
            return c.x >= 0 && c.x < MapWidth && c.y >= 0 && c.y < MapHeight;
        }

        private (int x, int y) D => (Plain % MapWidth, Plain / MapWidth);

        private int ToPlain((int x, int y) c)
        {
            return c.x + (c.y * MapWidth);
        }
    }
}
