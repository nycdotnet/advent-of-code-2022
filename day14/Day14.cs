using common;
using OneOf;
using OneOf.Types;
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
                            for (var k = from.Y; k >= to.Y; k--)
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

        public static readonly Point2d SandPoursInFrom = new() { X = 500, Y = 0 };

        public static Point2d[] ParsePoints(string input) => input
            .Split("->")
            .Select(s => s.Trim().Split(','))
            .Select(p => new Point2d { X = int.Parse(p[0]), Y = int.Parse(p[1]) })
            .ToArray();

        public SandMap GenerateBaseMap(int part)
        {
            if (!(part == 1 || part == 2))
            {
                throw new NotSupportedException("You must specify 1 or 2 for part.");
            }

            var (minX, maxX) = GetXRange(part);
            var headerX = Math.Max(Math.Max(maxX.ToString().Length, minX.ToString().Length), SandPoursInFrom.X.ToString().Length);
            var maxY = GetMaxY(part);
            var headerY = maxY.ToString().Length;

            var result = new char[maxY + 1 + headerX][];
            for (var y = 0; y < result.Length; y++)
            {
                result[y] = new char[maxX - minX + 2 + headerY];
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
            result[headerX][headerY + 1 + SandPoursInFrom.X - minX] = '+';

            // draw rock
            foreach (var rock in GetAllRockPoints())
            {
                result[rock.Y + headerX][rock.X + headerY + 1 - minX] = '#';
            }

            if (part == 2)
            {
                var bottomRow = result[^1];
                // draw the rock floor.
                for (var x = headerY + 1; x < bottomRow.Length; x++)
                {
                    bottomRow[x] = '#';
                }
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

            DrawXLabel(minX);
            DrawXLabel(maxX);
            DrawXLabel(SandPoursInFrom.X);

            return new SandMap
            {
                Map = result,
                Height = maxY + 1,
                Width = maxX - minX + 1,
                TopLeft = (rowIndex: headerX, columnIndex: headerY + 1),
                TopLeftXValue = minX,
                Part = part
            };

            void DrawXLabel(int number)
            {
                var label = number.ToString().PadLeft(headerX, ' ').AsSpan();
                var columnOffset = headerY + 1 + (number - minX);
                for (var i = 0; i < label.Length; i++)
                {
                    result[i][columnOffset] = label[i];
                }
            }
        }

        public (int minX, int maxX) GetXRange(int part) => (
            minX: InitialPoints.Select(p => p.Min(x => x.X)).Min() - (part == 2 ? SandPoursInFrom.X * 3 : 0),
            maxX: InitialPoints.Select(p => p.Max(x => x.X)).Max() + (part == 2 ? SandPoursInFrom.X * 3 : 0));

        public int GetMaxY(int part) => InitialPoints.Select(p => p.Max(y => y.Y)).Max() + (part == 2 ? 2 : 0);

        public string GetAnswerForPart1()
        {
            var map = GenerateBaseMap(1);
            var result = SimulateContinuously(map);
            return result.FinalOneBasedStep.Match(
                finalOneBasedStep => finalOneBasedStep.ToString(),
                _ => "Did not finish");
        }

        public string GetAnswerForPart2()
        {
            var map = GenerateBaseMap(2);
            var result = SimulateContinuously(map);
            return result.FinalOneBasedStep.Match(
                finalOneBasedStep => finalOneBasedStep.ToString(),
                _ => "Did not finish");
        }

        public readonly record struct SimulateResult
        {
            public required SandMap SandMap { get; init; }
            public required OneOf<int, None> FinalOneBasedStep { get; init; }
        }

        public static SimulateResult SimulateContinuously(SandMap sandMap) => Simulate(int.MaxValue, sandMap);

        /// <summary>
        /// Returns a map of the final state after simulating for <paramref name="sandCount"/> sand drops.
        /// </summary>
        public static SimulateResult Simulate(int sandCount, SandMap sandMap)
        {
            InBounds sandLocation = sandMap.Peek(SandPoursInFrom with { }).AsT0;

            for (var sandIndex = 0; sandIndex < sandCount; sandIndex++)
            {
                var result = SimulateStep(sandMap, sandLocation);
                OneOf<ComesToRest, FallsForever> finalDisposition;
                while (result.TryPickT0(out FallsTo fallsTo, out finalDisposition))
                {
                    // the sand fell so we can simulate another step.
                    result = SimulateStep(sandMap, fallsTo.NewLocation);
                }

                if (finalDisposition.TryPickT1(out FallsForever _, out ComesToRest comesToRest))
                {
                    if (sandMap.Part == 2)
                    {
                        throw new NotSupportedException("We are not supposed to get a falls forever result in part 2.");
                    }

                    // NOTE: We don't add one here because we want to indicate that the PREVIOUS one came to rest.
                    return new SimulateResult { SandMap = sandMap, FinalOneBasedStep = sandIndex };
                }

                sandMap.MarkSandComesToRest(comesToRest.FinalLocation);

                if (comesToRest.FinalLocation.Point == SandPoursInFrom)
                {
                    return new SimulateResult { SandMap = sandMap, FinalOneBasedStep = sandIndex + 1 };
                }
            }
            return new SimulateResult { SandMap = sandMap, FinalOneBasedStep = new None() };
        }

        /// <summary>
        /// Simulates one step of the drop of one unit of sand
        /// </summary>
        public static OneOf<FallsTo, ComesToRest, FallsForever> SimulateStep(SandMap map, InBounds startSandLocation)
        {
            var down = map.PeekDown(startSandLocation.Point).Match(
                    MapInBounds,
                    oob => new FallsForever());

            if (!down.TryPickT0(out var downBlocked, out var downRest))
            {
                return downRest.Match<OneOf<FallsTo, ComesToRest, FallsForever>>(
                    air => new FallsTo { NewLocation = air.InBoundsLocation },
                    fallsForever => fallsForever
                );
            }

            var downLeft = map.PeekDownLeft(startSandLocation.Point).Match(
                    MapInBounds,
                    oob => new FallsForever());

            if (!downLeft.TryPickT0(out var downLeftBlocked, out var downLeftRest))
            {
                return downLeftRest.Match<OneOf<FallsTo, ComesToRest, FallsForever>>(
                    air => new FallsTo { NewLocation = air.InBoundsLocation },
                    fallsForever => fallsForever
                );
            }

            var downRight = map.PeekDownRight(startSandLocation.Point).Match(
                    MapInBounds,
                    oob => new FallsForever());

            if (!downRight.TryPickT0(out var downRightBlocked, out var downRightRest))
            {
                return downRightRest.Match<OneOf<FallsTo, ComesToRest, FallsForever>>(
                    air => new FallsTo { NewLocation = air.InBoundsLocation },
                    fallsForever => fallsForever
                );
            }

            return new ComesToRest { FinalLocation = startSandLocation };

            static OneOf<Blocked, Air, FallsForever> MapInBounds(InBounds ib) => ib.State switch
            {
                '.' => new Air { InBoundsLocation = ib },
                ' ' => new FallsForever(),
                '#' or 'o' => new Blocked(),
                _ => throw new UnreachableException()
            };
        }
    }

    public sealed record SandMap
    {
        public required char[][] Map { get; init; }
        /// <summary>
        /// Represents the row and column index in <see cref="Map"/> of the top left air.
        /// </summary>
        public required (int rowIndex, int columnIndex) TopLeft { get; init; }
        /// <summary>
        /// Represents the "true" X value of <see cref="TopLeft"/>.  Note the true Y is always 0.
        /// </summary>
        public required int TopLeftXValue { get; init; }
        public required int Width { get; init; }
        public required int Height { get; init; }

        private int _part;
        public required int Part {
            get => _part;
            set {
                if (value != 1 && value != 2)
                {
                    throw new NotSupportedException("Part must be 1 or 2");
                }
                _part = value;
            }
        }

        public string RenderToString() => string.Join('\n', Map.Select(line => new string(line)));

        /// <summary>
        /// Maps the <paramref name="point"/> in 2D space to the row and column index
        /// of the <see cref="Map"/> array.
        /// </summary>
        public OneOf<InBounds, OutOfBounds> Peek(Point2d point)
        {
            var rowIndex = point.Y + TopLeft.rowIndex;
            var columnIndex = point.X - TopLeftXValue + TopLeft.columnIndex;

            if (rowIndex < 0 || columnIndex < 0 || rowIndex >= Map.Length || columnIndex >= Map[rowIndex].Length)
            {
                return new OutOfBounds { Point = point };
            }

            return new InBounds { RowIndex = rowIndex, ColumnIndex = columnIndex, Point = point, State = Map[rowIndex][columnIndex] };
        }
        public OneOf<InBounds, OutOfBounds> PeekDown(Point2d @from) => Peek(@from with { Y = @from.Y + 1 });
        public OneOf<InBounds, OutOfBounds> PeekDownLeft(Point2d @from) => Peek(@from with { Y = @from.Y + 1, X = @from.X - 1 });
        public OneOf<InBounds, OutOfBounds> PeekDownRight(Point2d @from) => Peek(@from with { Y = @from.Y + 1, X = @from.X + 1 });
        internal void MarkSandComesToRest(InBounds location)
        {
            Map[location.RowIndex][location.ColumnIndex] = 'o';
        }
    }

    public struct Blocked { }
    public readonly struct Air {
        public required InBounds InBoundsLocation { get; init; }
    }

    /// <summary>
    /// Indicates that the specified point is out of bounds of the visualized array.
    /// </summary>
    public readonly struct OutOfBounds {
        public required Point2d Point { get; init; }
    }

    /// <summary>
    /// Indicates that the specified point is in bounds of the visualized array
    /// </summary>
    public readonly struct InBounds
    {
        public required int RowIndex { get; init; }
        public required int ColumnIndex { get; init; }
        public required Point2d Point { get; init; }
        public required char State { get; init; }
    }

    /// <summary>
    /// Indicates the request would result in the sand falling forever
    /// </summary>
    public struct FallsForever { }

    /// <summary>
    /// Indicates that the sand got stuck at the origin.
    /// </summary>
    //public struct GetsStuck { }

    /// <summary>
    /// Indicates that the sand comes to rest at the specified coordinates
    /// </summary>
    public readonly struct ComesToRest
    {
        public required InBounds FinalLocation { get; init; }
    }

    /// <summary>
    /// Indicates that the sand fell to <see cref="NewLocation"/> and that it has not yet come to rest.
    /// </summary>
    public readonly struct FallsTo
    {
        public required InBounds NewLocation { get; init; }
    }
}