using common;
using System.Diagnostics;

namespace day12
{
    public class Day12 : IAdventOfCodeDay, IWeightedGraph<Point2d, int>
    {
        public Day12(IEnumerable<string> input)
        {
            Map = input.Select(x => x.ToCharArray()).ToArray();
            if (Map.Select(row => row.Length).Distinct().Count() != 1)
            {
                throw new ArgumentException(paramName: nameof(input), message: "Unexpected jagged map.");
            }

            var points = FindStartandEnd();
            if (points.start is null)
            {
                throw new ArgumentException(paramName: nameof(input), message: "Unexpected missing start point in input.");
            }
            if (points.end is null)
            {
                throw new ArgumentException(paramName: nameof(input), message: "Unexpected missing end point in input.");
            }
            Start = points.start;
            End = points.end;
        }

        public char[][] Map { get; private set; }
        /// <summary>
        /// The start position where the top left is 0,0 and increasing positions are to the right and down.
        /// </summary>
        public Point2d Start {  get; private set; }
        /// <summary>
        /// The end position where the top left is 0,0 and increasing positions are to the right and down.
        /// </summary>
        public Point2d End { get; private set; }

        /// <summary>
        /// Finds the start and end point.  If either or both are not found, the corresponding
        /// <see cref="Point2d"/> will be null.
        /// </summary>
        private (Point2d? start, Point2d? end) FindStartandEnd()
        {
            Point2d? start = null;
            Point2d? end = null;

            for (var row = 0; row < Map.Length; row++)
            {
                for (var col = 0; col < Map[row].Length; col++)
                {
                    if (Map[row][col] == 'S')
                    {
                        start = new Point2d { Y = row, X = col };
                    }
                    else if (Map[row][col] == 'E')
                    {
                        end = new Point2d { Y = row, X = col };
                    }

                    if (start is not null && end is not null)
                    {
                        break;
                    }
                }

                if (start is not null && end is not null)
                {
                    break;
                }
            }
            return (start, end);
        }

        public int Cost(Point2d a, Point2d b) => a.ManhattanDistance(b);

        public string GetAnswerForPart1()
        {
            var aStar = AnalyzePath(Start);
            return aStar.GetOptimalPathCost().ToString();
        }

        public string GetAnswerForPart2()
        {
            var best = int.MaxValue;

            foreach(var start in Part2StartingPoints())
            {
                var aStar = AnalyzePath(start, previousBest: best);
                if (aStar.TryGetOptimalPathCost(out var cost))
                {
                    best = Math.Min(cost, best);
                }
            }

            return best.ToString();
        }

        public IEnumerable<Point2d> Part2StartingPoints()
        {
            for (var row = 0; row < Map.Length; row++)
            {
                for (var col = 0; col < Map[row].Length; col++)
                {
                    var point = new Point2d { X = col, Y = row };
                    if (GetElevation(point) == 'a')
                    {
                        yield return point;
                    }
                }
            }
        }

        public AStarSearch<Point2d, int> AnalyzePath(Point2d startPoint, int previousBest = int.MaxValue) => new(
            this, startPoint, End,
            (next, goal) => next.ManhattanDistance(goal),
            failIfCostExceeds: previousBest
            );

        public static int Heuristic(Point2d next, Point2d goal) => next.ManhattanDistance(goal);

        public string GetDebugPathFromBackwardsPath(Point2d[] stepsFromEnd)
        {
            var map = Map.Select(x => {
                var row = new char[x.Length];
                Array.Fill(row, '.');
                return row;
            }).ToArray();

            map[End.Y][End.X] = 'E';
            var previousStep = stepsFromEnd[0];
            
            for (var i = 1; i < stepsFromEnd.Length; i++)
            {
                var currentStep = stepsFromEnd[i];
                map[currentStep.Y][currentStep.X] = GetBackwardsArrow(currentStep, previousStep);
                previousStep = currentStep;
            }

            map[Start.Y][Start.X] = GetBackwardsArrow(Start, previousStep);

            return RenderPath(map);

            char GetBackwardsArrow(Point2d current, Point2d previous)
            {
                if (current.X < previous.X)
                {
                    return '>';
                }
                else if (current.X > previous.X)
                {
                    return '<';
                }
                else if (current.Y < previous.Y)
                {
                    return 'v';
                }
                else if (current.Y > previous.Y)
                {
                    return '^';
                }
                throw new UnreachableException($"Not expected relationship between points - need to be adjacent up down left or right current={current} previous={previous}.");
            }
        }

        private static string RenderPath(char[][] map) =>
            string.Join('\n', map.Select(row => string.Join("", row)));
        
        public char GetElevation(Point2d point)
        {
            var elevation = Map[point.Y][point.X];
            if (elevation == 'S')
            {
                return 'a';
            }
            if (elevation == 'E')
            {
                return 'z';
            }
            return elevation;
        }

        /// <summary>
        /// Returns the neighbors for this <see cref="Point2d"/> on the <see cref="Map"/>.
        /// Will return an <see cref="IndexOutOfRangeException"/> if the argument node is
        /// out of bounds.
        /// </summary>
        public IEnumerable<Point2d> GetNeighbors(Point2d node)
        {
            // in this graph, we allow cardinal directions only.

            var maxNextElevation = GetElevation(node) + 1;

            // check south
            if (node.Y < Map.Length - 1)
            {
                var southElevation = GetElevation(new Point2d { Y = node.Y + 1, X = node.X });
                if (southElevation <= maxNextElevation)
                {
                    yield return new Point2d { X = node.X, Y = node.Y + 1 };
                }
            }

            // check north
            if (node.Y > 0)
            {
                var northElevation = GetElevation(new Point2d { Y = node.Y - 1, X = node.X });
                if (northElevation <= maxNextElevation)
                {
                    yield return new Point2d { X = node.X, Y = node.Y - 1 };
                }
            }

            // check east
            if (node.X < Map[node.Y].Length - 1)
            {
                var eastElevation = GetElevation(new Point2d { Y = node.Y, X = node.X + 1 }); ;
                if (eastElevation <= maxNextElevation)
                {
                    yield return new Point2d { X = node.X + 1, Y = node.Y };
                }
            }

            // check west
            if (node.X > 0)
            {
                var westElevation = GetElevation(new Point2d { Y = node.Y, X = node.X - 1 }); ;
                if (westElevation <= maxNextElevation)
                {
                    yield return new Point2d { X = node.X - 1, Y = node.Y };
                }
            }
        }
    }
}