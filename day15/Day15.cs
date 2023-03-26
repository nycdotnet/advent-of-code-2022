using common;
using System.Data;
using System.Diagnostics;
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

            foreach(var sensor in Sensors)
            {
                Grid.Push('S', sensor.Position.X, sensor.Position.Y);
                Grid.Push('B', sensor.ClosestBeacon.X, sensor.ClosestBeacon.Y);
            }
        }

        public List<Sensor> Sensors { get; }
        public TwoDimensionalGrid<char, int> Grid { get; }

        public int RowSearchYOffset { get; set; }
        public int BeaconScanMinimumCoordinate { get; set; }
        public int BeaconScanMaximumCoordinate { get; set; }


        public void MarkBeaconAbsence(Sensor sensor)
        {
            var dist = sensor.Position.ManhattanDistance(sensor.ClosestBeacon);

            if (dist > 10_000)
            {
                throw new NotSupportedException("This method is inappropriate for sensors with very high distances.");
            }

            var xRange = -1;
            // this loop will mark a broadening triangle
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
            return CountPositionsThatCannotContainABeaconAtY(RowSearchYOffset).ToString();
        }

        public void MarkAllBeaconAbsences()
        {
            foreach (var sensor in Sensors)
            {
                MarkBeaconAbsence(sensor);
            }
        }

        public int CountPositionsThatCannotContainABeaconAtY(int y)
        {
            var definitelyNoBeacons = new RangeSet();
            var definitelyBeaconXValues = new HashSet<int>();

            foreach(var sensor in Sensors)
            {
                if (sensor.Position.Y == y)
                {
                    definitelyNoBeacons.Exclude(sensor.Position.X, sensor.Position.X);
                }

                var mdToClosestBeacon = sensor.Position.ManhattanDistance(sensor.ClosestBeacon);
                var distanceToY = Math.Abs(sensor.Position.Y - y);

                var exclusionMagnitude = mdToClosestBeacon - distanceToY + 1;
                if (exclusionMagnitude <= 0)
                {
                    continue;
                }

                if (sensor.ClosestBeacon.Y == y)
                {
                    // we exclude the possibility of a beacon anywhere it can reach on y
                    // we include that there is something at the position of the known beacon
                    
                    var rs = new RangeSet();
                    rs.Exclude(sensor.Position.X - exclusionMagnitude + 1, sensor.Position.X + exclusionMagnitude - 1);
                    rs.Include(sensor.ClosestBeacon.X, sensor.ClosestBeacon.X);

                    foreach (var r in rs.ExcludedRanges)
                    {
                        definitelyNoBeacons.Exclude(r.bottom, r.top);
                    }

                    definitelyBeaconXValues.Add(sensor.ClosestBeacon.X);
                }
                else
                {
                    // we exclude the possibility of a beacon anywhere it can reach on y
                    definitelyNoBeacons.Exclude(sensor.Position.X - exclusionMagnitude + 1,
                        sensor.Position.X + exclusionMagnitude - 1);
                }
            }

            foreach(var x in definitelyBeaconXValues)
            {
                definitelyNoBeacons.Include(x, x);
            }

            return definitelyNoBeacons.ExcludedCount;
        }

        public Point2d? FindUndetectedBeacon(int y, int minCoordinate, int maxCoordinate)
        {
            Debug.Assert(y >= minCoordinate && y <= maxCoordinate);

            var rs = new RangeSet();
            
            foreach (var sensor in Sensors)
            {
                if (sensor.Position.Y == y)
                {
                    rs.Exclude(sensor.Position.X, sensor.Position.X);
                }

                var mdToClosestBeacon = sensor.Position.ManhattanDistance(sensor.ClosestBeacon);
                var distanceToY = Math.Abs(sensor.Position.Y - y);

                var exclusionMagnitude = mdToClosestBeacon - distanceToY + 1;
                if (exclusionMagnitude <= 0)
                {
                    continue;
                }

                var excludeBottom = Math.Max(sensor.Position.X - exclusionMagnitude + 1, minCoordinate);
                var excludeTop = Math.Min(sensor.Position.X + exclusionMagnitude - 1, maxCoordinate);

                if (excludeBottom == minCoordinate && excludeTop == maxCoordinate)
                {
                    return null;
                }

                rs.Exclude(excludeBottom, excludeTop);
            }

            if (rs.ExcludedRangeCount == 1)
            {
                var excludedRange = rs.ExcludedRanges.Single();
                if (excludedRange.bottom == minCoordinate && excludedRange.top == maxCoordinate)
                {
                    return null;
                }

                throw new NotImplementedException("need to figure out which end has it.");
            }
            else if (rs.ExcludedRangeCount == 2)
            {
                return new Point2d { X = rs.ExcludedRanges.First().top + 1, Y = y };
            }
            
            throw new NotSupportedException($"There's some sort of error here.  There should only ever be 1 or 2 excluded ranges.  Found {rs.ExcludedRangeCount}.");
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

        public int CalculateCountOfPositionsWhereBeaconCanNotBePresentInRowUsingRangeset(int YValue)
        {
            throw new NotImplementedException("No longer used.");
            //var rs = new RangeSet();
            //foreach (var sensor in Sensors)
            //{
            //    var dist = sensor.Position.ManhattanDistance(sensor.ClosestBeacon);

            //    if (Math.Abs(sensor.Position.Y - YValue) > dist)
            //    {
            //        // this sensor can not affect the row identified in YValue.
            //        continue;
            //    }

            //    var excludabilityMagnitude = Math.Abs(sensor.Position.Y - dist - YValue);
            //    rs.Exclude(sensor.Position.X - excludabilityMagnitude, sensor.Position.X + excludabilityMagnitude);
            //}
            
            //return rs.ExcludedCount;
        }

        public string GetAnswerForPart2()
        {
            if (BeaconScanMinimumCoordinate == 0 && BeaconScanMaximumCoordinate == 0)
            {
                throw new NotSupportedException("The beacon scan min/max coordinates can't both be zero.");
            }

            string tuningFrequency = null;
            Parallel.For(BeaconScanMinimumCoordinate, BeaconScanMaximumCoordinate, (y, loopState) =>
            {
                var result = FindUndetectedBeacon(y, BeaconScanMinimumCoordinate, BeaconScanMaximumCoordinate);
                if (result is not null)
                {
                    tuningFrequency = (((long)result.X * 4_000_000) + result.Y).ToString();
                    loopState.Stop();
                }
            });

            return tuningFrequency ?? throw new ApplicationException("unable to find answer!");
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
            // TODO: this should binary search to find the right start point.
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
                    list.TryAdd(top + 1, existingTop);
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

            for (var i = 0; i < list.Count; i++)
            {
                // TODO: This should binary search to find the right start point.
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
                    if (!list.TryAdd(bottom, existingTop))
                    {
                        list[bottom] = Math.Max(existingTop, top);
                    }
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
                    // the new range totally overlaps, so we can replace the existing entry.
                    list.RemoveAt(i);
                    if (!list.TryAdd(bottom, top))
                    {
                        list[bottom] = Math.Max(existingTop, top);
                    }
                    mergeSuccess = true;
                    continue;
                }
            }

            if (mergeSuccess)
            {
                Compact(list);
            }

            return mergeSuccess;
        }

        private static void Compact(SortedList<int, int> list)
        {
            for (var i = list.Count - 1; i >= 1; i--)
            {
                var currentBottom = list.GetKeyAtIndex(i);
                var nextTop = list.GetValueAtIndex(i - 1);
                if (currentBottom <= nextTop + 1)
                {
                    // need to consolidate.
                    list[list.GetKeyAtIndex(i - 1)] = list.GetValueAtIndex(i);
                    list.RemoveAt(i);
                }
            }
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

        public int ExcludedCount => _excluded.Sum(kvp => kvp.Value - kvp.Key + 1);
        public int IncludedCount => _included.Sum(kvp => kvp.Value - kvp.Key + 1);
        public IEnumerable<(int bottom, int top)> ExcludedRanges => _excluded.Select(ex => (ex.Key, ex.Value));
        public IEnumerable<(int bottom, int top)> IncludedRanges => _included.Select(ex => (ex.Key, ex.Value));


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