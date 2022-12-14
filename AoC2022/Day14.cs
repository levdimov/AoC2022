using System.Text;
using static System.Math;

namespace AoC2022;

public class Day14 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var stoneCoords = Parse(input);
        var sliceOfTheCave = new SliceOfTheCave(stoneCoords);

        var result = sliceOfTheCave.Emulate();

        Console.WriteLine(sliceOfTheCave.ToString());

        return result.ToString();
    }

    private static IEnumerable<Coord> Parse(IEnumerable<string> stoneLines)
    {
        return stoneLines
            .SelectMany(
                l =>
                {
                    var lineCoords = l
                        .Split(" -> ")
                        .Select(c =>
                                {
                                    var p = c.Split(',').Select(int.Parse).ToArray();
                                    return new Coord(p[0], p[1]);
                                })
                        .ToArray();

                    var result = new List<Coord>();
                    for (var i = 1; i < lineCoords.Length; i++)
                    {
                        result.AddRange(Coord.Fill(lineCoords[i - 1], lineCoords[i]));
                    }

                    return result;
                })
            .Distinct();
    }

    private record Coord(int X, int Y)
    {
        public static IEnumerable<Coord> Fill(Coord a, Coord b)
        {
            if (a.X == b.X)
            {
                for (var y = Min(a.Y, b.Y); y <= Max(a.Y, b.Y); y++)
                {
                    yield return a with { Y = y };
                }

                yield break;
            }

            if (a.Y == b.Y)
            {
                for (var x = Min(a.X, b.X); x <= Max(a.X, b.X); x++)
                {
                    yield return a with { X = x };
                }

                yield break;
            }

            throw new ArgumentException();
        }
    }

    private class SliceOfTheCave
    {
        private readonly Dictionary<Coord, Unit> map;

        public SliceOfTheCave(IEnumerable<Coord> stoneCoords)
        {
            map = stoneCoords.ToDictionary(c => c, _ => (Unit)new Stone());
        }

        private int Floor
        {
            get
            {
                var ys = map.Keys.Select(c => c.Y).ToArray();
                return ys.Max() + 2;
            }
        }

        public override string ToString()
        {
            var xs = map.Keys.Select(c => c.X).ToArray();
            var ys = map.Keys.Select(c => c.Y).ToArray();

            var fromX = xs.Min() - 1;
            var toX = xs.Max() + 1;
            var fromY = ys.Min() - 1;
            var toY = ys.Max() + 1;

            var sb = new StringBuilder();
            for (var y = fromY; y <= toY; y++)
            {
                for (var x = fromX; x <= toX; x++)
                {
                    sb.Append(map.TryGetValue(new Coord(x, y), out var unit) ? unit.ToString() : ".");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public int Emulate()
        {
            var floor = Floor;

            while (true) //sand tick
            {
                var sc = new Coord(500, 0);

                while (true) //emulation tick
                {
                    if (map.ContainsKey(sc))
                    {
                        return map.Values.OfType<Sand>().Count();
                    }

                    var d = sc with { Y = sc.Y + 1 };
                    var l = sc with { X = sc.X - 1, Y = sc.Y + 1 };
                    var r = sc with { X = sc.X + 1, Y = sc.Y + 1 };

                    if (!map.ContainsKey(d) && d.Y < floor)
                    {
                        sc = d;
                        continue;
                    }

                    if (!map.ContainsKey(l) && l.Y < floor)
                    {
                        sc = l;
                        continue;
                    }

                    if (!map.ContainsKey(r) && r.Y < floor)
                    {
                        sc = r;
                        continue;
                    }

                    map.Add(sc, new Sand());
                    break;
                }
            }
        }
    }

    private abstract record Unit;

    private record Stone : Unit
    {
        public override string ToString() => "#";
    }

    private record Sand : Unit
    {
        public override string ToString() => "o";
    }
}
