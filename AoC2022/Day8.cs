namespace AoC2022;

public class Day8 : SolverBase
{
    protected override string Solve(IEnumerable<string> input)
    {
        var mapString = input.ToArray();

        var yLength = mapString.Length;
        var xLength = mapString[0].Length;

        var map = new int[yLength][];
        for (var y = 0; y < yLength; y++)
        {
            map[y] = new int[xLength];
            for (var x = 0; x < mapString[y].Length; x++)
            {
                map[y][x] = int.Parse(mapString[y][x].ToString());
            }
        }

        return Part2(yLength, xLength, map).ToString();
    }

    private static int Part1(int yLength, int xLength, int[][] map)
    {
        var visible = 0;
        for (var y = 0; y < yLength; y++)
        {
            for (var x = 0; x < xLength; x++)
            {
                var height = map[y][x];

                //outside
                if (y == 0 || y + 1 == yLength || x == 0 || x + 1 == xLength)
                {
                    visible += 1;
                    continue;
                }

                //visible from left
                if (map[y][..x].Max() < height)
                {
                    visible += 1;
                    continue;
                }

                //visible from right
                if (map[y][(x + 1)..].Max() < height)
                {
                    visible += 1;
                    continue;
                }

                //visible from top
                if (map[..y].Max(l => l[x]) < height)
                {
                    visible += 1;
                    continue;
                }

                //visible from down
                if (map[(y + 1)..].Max(l => l[x]) < height)
                {
                    visible += 1;
                    continue;
                }
            }
        }

        return visible;
    }

    private static int Part2(int yLength, int xLength, int[][] map)
    {
        var maxScore = 0;
        for (var y = 0; y < yLength; y++)
        {
            for (var x = 0; x < xLength; x++)
            {
                var score = 1;
                var height = map[y][x];

                //outside
                if (y == 0 || y + 1 == yLength || x == 0 || x + 1 == xLength)
                {
                    continue;
                }

                //visible from left
                var leftScore = map[y][..x].Reverse().TakeWhile(h => h < height).Count();
                score *= leftScore + (leftScore == x ? 0 : 1);

                //visible from right
                var rightScore = map[y][(x + 1)..].TakeWhile(h => h < height).Count();
                score *= rightScore + (rightScore == (xLength - x - 1) ? 0 : 1);

                //visible from top
                var topScore = map[..y].Reverse().TakeWhile(l => l[x] < height).Count();
                score *= topScore + (topScore == y ? 0 : 1);

                //visible from down
                var downScore = map[(y + 1)..].TakeWhile(l => l[x] < height).Count();
                score *= downScore + (downScore == (yLength - y - 1) ? 0 : 1);

                maxScore = Math.Max(score, maxScore);
            }
        }

        return maxScore;
    }
}
