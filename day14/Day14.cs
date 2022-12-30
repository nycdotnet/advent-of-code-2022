using common;
using System.Diagnostics;

namespace day14
{
    public class Day14 : IAdventOfCodeDay
    {
        public Day14(IEnumerable<string> input)
        {
            InitialPoints = input.Where(s => !string.IsNullOrEmpty(s)).Select(ParsePoints).ToList();
        }

        public IReadOnlyList<Point2d[]> InitialPoints { get; }

        public IEnumerable<Point2d> GetAllRockPoints()
        {
            for (var i = 0; i < InitialPoints.Count; i++)
            {
                var line = InitialPoints[i];
                for (var j = 0; j < line.Length - 1; j++)
                {
                    var from = line[j];
                    var to = line[j + 1];
                    if (from.X == to.X)
                    {
                        if (from.Y < to.Y)
                        {
                            for (var k = from.Y; k <= to.Y; k++)
                            {
                                yield return new Point2d { X = from.X, Y = k };
                            }
                        }
                        else
                        {
                            Debug.Assert(from.Y > to.Y);
                            for (var k = from.Y; k <= to.Y; k--)
                            {
                                yield return new Point2d { X = from.X, Y = k };
                            }
                        }
                    }
                    else
                    {
                        Debug.Assert(from.Y == to.Y);

                        if (from.X < to.X)
                        {
                            for (var k = from.X; k <= to.X; k++)
                            {
                                yield return new Point2d { X = k, Y = from.Y };
                            }
                        }
                        else
                        {
                            Debug.Assert(from.X > to.X);
                            for (var k = from.X; k >= to.X; k--)
                            {
                                yield return new Point2d { X = k, Y = from.Y };
                            }
                        }
                    }
                }
            }
        }

        public static readonly Point2d SandPoursInFrom = new Point2d { X = 500, Y = 0 };

        public static Point2d[] ParsePoints(string input) => input
            .Split("->")
            .Select(s => s.Trim().Split(','))
            .Select(p => new Point2d { X = int.Parse(p[0]), Y = int.Parse(p[1]) })
            .ToArray();

        public char[][] GenerateBaseMap()
        {
            var xRange = GetXRange();
            var headerX = Math.Max(Math.Max(xRange.maxX.ToString().Length, xRange.minX.ToString().Length), SandPoursInFrom.X.ToString().Length);
            var maxY = GetMaxY();
            var headerY = maxY.ToString().Length;

            var result = new char[maxY + 1 + headerX][];
            for (var y = 0; y < result.Length; y++)
            {
                result[y] = new char[xRange.maxX - xRange.minX + 2 + headerY];
                Array.Fill(result[y], ' ');
            }

            // draw air
            for (var y = headerX; y < maxY + headerX + 1; y++)
            {
                var currentRow = result[y];
                for (var x = headerY + 1; x < currentRow.Length; x++)
                {
                    currentRow[x] = '.';
                }
            }

            // draw sand origin
            result[headerX][headerY + 1 + SandPoursInFrom.X - xRange.minX] = '+';

            // draw rock
            foreach (var rock in GetAllRockPoints())
            {
                result[rock.Y + headerX][rock.X + headerY + 1 - xRange.minX] = '#';
            }

            // draw Y labels
            for (var num = 0; num <= maxY; num++)
            {
                var label = num.ToString().PadLeft(headerY, ' ').AsSpan();
                for (var yy = headerY; yy > 0; yy--)
                {
                    result[headerX + num][yy - 1] = label[yy - 1];
                }
            }

            DrawXLabel(xRange.minX);
            DrawXLabel(xRange.maxX);
            DrawXLabel(SandPoursInFrom.X);

            return result;

            void DrawXLabel(int number)
            {
                var label = number.ToString().PadLeft(headerX, ' ').AsSpan();
                var columnOffset = headerY + 1 + (number - xRange.minX);
                for (var i = 0; i < label.Length; i++)
                {
                    result[i][columnOffset] = label[i];
                }
            }
        }

        public (int minX, int maxX) GetXRange() => (
            minX: InitialPoints.Select(p => p.Min(x => x.X)).Min(),
            maxX: InitialPoints.Select(p => p.Max(x => x.X)).Max());

        public int GetMaxY() => InitialPoints.Select(p => p.Max(y => y.Y)).Max();

        public string GetAnswerForPart1()
        {
            throw new NotImplementedException();
        }

        public string GetAnswerForPart2()
        {
            throw new NotImplementedException();
        }
    }
}