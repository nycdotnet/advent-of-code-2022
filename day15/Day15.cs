using common;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
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

            foreach(var sensor in Sensors)
            {
                Grid.Push('S', sensor.Position.X, sensor.Position.Y);
                Grid.Push('B', sensor.ClosestBeacon.X, sensor.ClosestBeacon.Y);
            }
        }

        public List<Sensor> Sensors { get; }
        public TwoDimensionalGrid<char, int> Grid { get; }

        public void MarkBeaconAbsence(Sensor sensor)
        {
            var dist = sensor.Position.ManhattanDistance(sensor.ClosestBeacon);

            if (dist > 10_000)
            {
                throw new NotSupportedException("This method is inappropriate for sensors with very high distances.");
            }

            var xRange = -1;
            for (var y = sensor.Position.Y - dist; y <= sensor.Position.Y; y++)
            {
                xRange++;
                for (var x = sensor.Position.X - xRange; x <= sensor.Position.X + xRange; x++)
                {
                    Grid.PushIfEmpty('#', x, y);
                }
            }

            // this loop will mark a narrowing triangle.
            for (var y = sensor.Position.Y + 1; y <= sensor.Position.Y + dist; y++)
            {
                xRange--;
                for (var x = sensor.Position.X - xRange; x <= sensor.Position.X + xRange; x++)
                {
                    Grid.PushIfEmpty('#', x, y);
                }
            }
        }

        public string GetAnswerForPart1()
        {



            return "fail!!!";
        }

        public void MarkAllBeaconAbsences()
        {
            foreach (var sensor in Sensors)
            {
                MarkBeaconAbsence(sensor);
            }
        }

        public int CalculateCountOfPositionsWhereBeaconCanNotBePresentInRow(int YValue)
        {
            foreach(var sensor in Sensors)
            {

            }

            return -1;
        }

        /// <summary>
        /// Gets the pre-marked conditions.
        /// </summary>
        public int GetCountOfPositionsWhereBeaconCanNotBePresentInRow(int YValue)
        {
            var limits = Grid.GetDimensionalLimits();
            var count = 0;
            for (var x = limits.MinX; x <= limits.MaxX; x++)
            {
                var content = Grid.Get(x, YValue);
                if (content == '#' || content == 'S') 
                {
                    count++;
                }
            }
            return count;
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
    public class TwoDimensionalGrid<TCell, TDimension>
        where TDimension : notnull, INumber<TDimension>
        where TCell : IComparable<TCell>, IEquatable<TCell>
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

        /// <summary>
        /// Pushes <paramref name="value"/> to <paramref name="X"/>, <paramref name="Y"/>.
        /// Will overwrite existing value if there is one at that coordinate.  Not thread safe.
        /// </summary>
        public void PushIfEmpty(TCell value, TDimension X, TDimension Y)
        {
            if (Get(X, Y).Equals(Empty))
            {
                Push(value, X, Y);
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

        /// <summary>
        /// Renders the Grid to a string with positive X to the right and positive Y down.
        /// The start X and Y represent the coordinates of the first character of the string
        /// result, and end X and Y represent the coordinates of the last character.
        /// </summary>
        public (string content, TDimension startX, TDimension startY, TDimension endX, TDimension endY) RenderToStringWithInvertedY(string lineSeparator = "\n")
        {
            if (!TDimension.IsInteger(default!))
            {
                throw new NotSupportedException($"{nameof(RenderToStringWithInvertedY)} is currently not supported except on integers.");
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
            return (content: sb.ToString(), startX: limits.MinX, startY: limits.MinY, endX: limits.MaxX, endY: limits.MaxY);
        }

        private readonly Func<TDimension, TDimension, TCell, char> render;

        public TCell Empty { get; init; }

        /// <summary>
        /// TODO: expose this in a readonly manner.
        /// </summary>
        public SortedList<TDimension, SortedList<TDimension, TCell>> CellsByX { get; }
    }


    /// <summary>
    /// Tracks a value of true or false for a particular range of integers.
    /// Thread safe for reading as long as there are no simultaneous writers.
    /// </summary>
    public class RangeSet
    {
        /// <summary>
        /// Key is the bottom of the range, inclusive.  Value is top inclusive.
        /// </summary>
        private SortedList<int, int> _included;
        /// <summary>
        /// Key is the bottom of the range, inclusive.  Value is top inclusive.
        /// </summary>
        private SortedList<int, int> _excluded;

        public RangeSet() {
            _included = new SortedList<int, int>();
            _excluded = new SortedList<int, int>();
        }

        public void Include(int bottom, int top)
        {
            if (top < bottom)
            {
                throw new ArgumentException(message: "Argument top must be greater than or equal to bottom.");
            }

            if (!TryMergeIntoExisting(_included, bottom, top))
            {
                _included.Add(bottom, top);
            }

            TrimOverlappingInverse(_excluded, bottom, top);
        }

        private static void TrimOverlappingInverse(SortedList<int, int> list, int bottom, int top)
        {
            // this should binary search to find the right start point.
            for (var i = 0; i < list.Count; i++)
            {
                var existingBottom = list.GetKeyAtIndex(i);
                if (existingBottom > top)
                {
                    // no need to search anymore.
                    return;
                }

                var existingTop = list[existingBottom];
                if (existingTop < bottom)
                {
                    continue;
                }

                // there is some kind of overlap.  Fix and test again.
                i--;

                if (bottom <= existingBottom && top < existingTop)
                {
                    // hanging off the bottom.  We need to change this one to
                    // start after the top.
                    list.Remove(existingBottom);
                    list.Add(top + 1, existingTop);
                }
                else if (existingBottom <= bottom && existingTop >= top)
                {
                    // bottom = 4, top = 5
                    // existing bottom = 3, existing top == 10
                    list.Remove(existingBottom);

                    // we need to split this one.
                    if (bottom > existingBottom)
                    {
                        list.Add(existingBottom, bottom - 1);
                    }
                    if (existingTop > top)
                    {
                        list.Add(top + 1, existingTop);
                    }
                }
                else if (existingBottom <= bottom && existingTop <= top)
                {
                    // this is hanging off the top.
                    // we need to change the start to be after the top.
                    list[existingBottom] = top + 1;
                }
                else
                {
                    Debug.Assert(bottom <= existingBottom && top >= existingTop);
                    // this range totally overlaps, so we can just eliminate this entry.
                    list.Remove(existingBottom);
                }
            }
        }

        private static bool TryMergeIntoExisting(SortedList<int, int> list, int bottom, int top)
        {
            var mergeSuccess = false;

            // this should binary search to find the right start point.
            for (var i = 0; i < list.Count; i++)
            {
                var existingBottom = list.GetKeyAtIndex(i);
                if (existingBottom > top)
                {
                    // no need to search anymore.
                    break;
                }
                var existingTop = list[existingBottom];
                if (existingTop < bottom)
                {
                    continue;
                }

                if (bottom >= existingBottom && top <= existingTop)
                {
                    // this is entirely in the existing included range, so we can do nothing
                    // and consider it merged.
                    mergeSuccess = true;
                    continue;
                }
                else if (bottom <= existingBottom && top <= existingTop)
                {
                    // new range hangs below.  Need to replace this entry.
                    list.RemoveAt(i);
                    list.Add(bottom, existingTop);
                    mergeSuccess = true;
                    continue;
                }
                else if (bottom >= existingBottom && top >= existingTop)
                {
                    // new range hangs above.  Need to extend existing entry.
                    list[existingBottom] = top;
                    mergeSuccess = true;
                    continue;
                }
                else
                {
                    Debug.Assert(bottom <= existingBottom && top >= existingTop);
                    // this range totally overlaps, so we need a new entry.
                    list.RemoveAt(i);
                    list.Add(bottom, top);
                    mergeSuccess = true;
                    continue;
                }
            }
            return mergeSuccess;
        }

        public void Exclude(int bottom, int top)
        {
            if (top < bottom)
            {
                throw new ArgumentException(message: "Argument top must be greater than or equal to bottom.");
            }


            if (!TryMergeIntoExisting(_excluded, bottom, top))
            {
                _excluded.Add(bottom, top);
            }

            TrimOverlappingInverse(_included, bottom, top);
        }

        public int IncludedRangeCount => _included.Count;
        public int ExcludedRangeCount => _excluded.Count;

        public State GetState(int value)
        {
            foreach(var inc in _included)
            {
                if (inc.Key > value)
                {
                    break;
                }
                if (value >= inc.Key && value <= inc.Value)
                {
                    return State.Included;
                }
            }

            foreach (var ex in _excluded)
            {
                if (ex.Key > value)
                {
                    break;
                }
                if (value >= ex.Key && value <= ex.Value)
                {
                    return State.Excluded;
                }
            }

            return State.Unspecified;
        }

        public enum State
        {
            Unspecified,
            Included,
            Excluded
        }
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