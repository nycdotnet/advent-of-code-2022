using common;
using System.ComponentModel;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace day15
{
    public class Day15 : IAdventOfCodeDay
    {
        public Day15(IEnumerable<string> input)
        {
            Sensors = input.Select(Sensor.MaybeParse).Where(s => s != null).ToList()!;
            Grid = new TwoDimensionalGrid<char, int>('.', (_, _, c) => c);

            foreach(var item in Sensors)
            {
                Grid.Push('S', item.Position.X, item.Position.Y);
                Grid.Push('B', item.ClosestBeacon.X, item.ClosestBeacon.Y);
            }
        }

        public List<Sensor> Sensors { get; }
        public TwoDimensionalGrid<char, int> Grid { get; }

        public string GetAnswerForPart1()
        {
            throw new NotImplementedException();
        }

        public string GetAnswerForPart2()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents a two-dimensional grid of <see cref="TCell"/>.  Efficiently stores sparse data.
    /// Thread safe for reading but not while writing.
    /// </summary>
    public class TwoDimensionalGrid<TCell, TDimension> where TDimension : notnull, INumber<TDimension>
    {

        public TwoDimensionalGrid(TCell Empty, Func<TDimension, TDimension, TCell, char> Renderer)
        {
            this.Empty = Empty;
            render = Renderer;
            CellsByX = new SortedList<TDimension, SortedList<TDimension, TCell>>();
        }

        /// <summary>
        /// Pushes <paramref name="value"/> to <paramref name="X"/>, <paramref name="Y"/>.
        /// Will overwrite existing value if there is one at that coordinate.  Not thread safe.
        /// </summary>
        public void Push(TCell value, TDimension X, TDimension Y)
        {
            if (CellsByX.TryGetValue(X, out var byY))
            {
                if (!byY.TryAdd(Y, value))
                {
                    byY[Y] = value;
                }
            }
            else
            {
                CellsByX[X] = new SortedList<TDimension, TCell> { { Y, value } };
            }
        }

        public TCell Get(TDimension X, TDimension Y)
        {
            if (CellsByX.TryGetValue(X, out var byY))
            {
                if (byY.TryGetValue(Y, out var value))
                {
                    return value;
                }
            }
            return Empty;
        }

        public TDimension MaxX => CellsByX.LastOrDefault().Key;
        public TDimension MinX => CellsByX.FirstOrDefault().Key;
        public TDimension MaxY => CellsByX.Select(c => c.Value.LastOrDefault().Key).DefaultIfEmpty().Max()!;
        public TDimension MinY => CellsByX.Select(c => c.Value.FirstOrDefault().Key).DefaultIfEmpty().Min()!;

        public (TDimension MinX, TDimension MaxX, TDimension MinY, TDimension MaxY) GetDimensionalLimits()
            => (MinX, MaxX, MinY, MaxY);

        public string RenderToString(string lineSeparator = "\n")
        {
            if (!TDimension.IsInteger(default!))
            {
                throw new NotSupportedException($"{nameof(RenderToString)} is currently not supported except on integers.");
            }

            var limits = GetDimensionalLimits();

            var sb = new StringBuilder();

            for (var y = limits.MinY; y <= limits.MaxY; y++)
            {
                for (var x = limits.MinX; x <= limits.MaxX; x++)
                {
                    sb.Append(render(x, y, Get(x, y)));
                }
                if (y < limits.MaxY)
                {
                    sb.Append(lineSeparator);
                }
            }
            return sb.ToString();
        }

        private readonly Func<TDimension, TDimension, TCell, char> render;

        public TCell Empty { get; init; }

        /// <summary>
        /// TODO: expose this in a readonly manner.
        /// </summary>
        public SortedList<TDimension, SortedList<TDimension, TCell>> CellsByX { get; }
    }

    public sealed partial record Sensor
    {
        public required Point2d Position { get; init; }
        public required Point2d ClosestBeacon {  get; init; }

        [GeneratedRegex(@"^Sensor at x=(?<PositionX>[+-]?\d*), y=(?<PositionY>[+-]?\d*): closest beacon is at x=(?<ClosestBeaconX>[+-]?\d*), y=(?<ClosestBeaconY>[+-]?\d*)$")]
        private static partial Regex SensorRegex();

        public static Sensor? MaybeParse(string s)
        {
            var match = SensorRegex().Match(s);
            if (match.Success)
            {
                return new Sensor
                {
                    Position = new()
                    {
                        X = int.Parse(match.Groups["PositionX"].ValueSpan),
                        Y = int.Parse(match.Groups["PositionY"].ValueSpan)
                    },
                    ClosestBeacon = new()
                    {
                        X = int.Parse(match.Groups["ClosestBeaconX"].ValueSpan),
                        Y = int.Parse(match.Groups["ClosestBeaconY"].ValueSpan)
                    }
                };
            }

            return null;
        }
    }
}