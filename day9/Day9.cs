using common;

namespace day9
{
    public class Day9 : IAdventOfCodeDay
    {
        public Day9(IEnumerable<string> input)
        {
            Moves = input.Select(RopeMove.Parse).ToList();
        }

        public List<RopeMove> Moves { get; }

        public string GetAnswerForPart1()
        {
            var tailPositions = new HashSet<Point2d>();
            var result = SimulateTwo(tailTracker: p => tailPositions.Add(p));
            return tailPositions.Count.ToString();
        }

        public string GetAnswerForPart2()
        {
            var tailPositions = new HashSet<Point2d>();
            var result = SimulateSet(10, tailTracker: p => tailPositions.Add(p));
            return tailPositions.Count.ToString();
        }

        public (Point2d finalHeadPosition, Point2d finalTailPosition) SimulateTwo(
            int movesToDo = -1,
            Action<Point2d>? tailTracker = null)
        {
            var result = SimulateSet(2, movesToDo, tailTracker);
            return (result[0], result[1]);
        }

        public List<Point2d> SimulateSet(
            int Count,
            int movesToDo = -1,
            Action<Point2d>? tailTracker = null)
        {
            var points = new List<Point2d>(Count);
            for (var i = 0; i < Count; i++)
            {
                points.Add(new Point2d { X = 0, Y = 0 });
            }
            
            if (movesToDo == -1)
            {
                movesToDo = Moves.Count;
            }

            for (var moveIndex = 0; moveIndex < movesToDo; moveIndex++)
            {
                var move = Moves[moveIndex];
                for (var step = 0; step < move.Magnitude; step++)
                {
                    ApplyStep(points[0], move);
                    for (var pointIndex = 0; pointIndex < Count - 1; pointIndex++)
                    {
                        SimulateFollower(points[pointIndex], points[pointIndex + 1]);
                        if (pointIndex + 2 == Count)
                        {
                            tailTracker?.Invoke(points[pointIndex + 1]);
                        }
                    }
                }
            }

            return points;
        }

        static void SimulateFollower(Point2d leadPosition, Point2d followerPosition)
        {
            if (leadPosition.IsTouching(followerPosition))
            {
                return;
            }

            if (leadPosition.X == followerPosition.X)
            {
                if (followerPosition.Y < leadPosition.Y)
                {
                    followerPosition.Y++;
                }
                else
                {
                    followerPosition.Y--;
                }
            }
            else if (leadPosition.Y == followerPosition.Y)
            {
                if (followerPosition.X < leadPosition.X)
                {
                    followerPosition.X++;
                }
                else
                {
                    followerPosition.X--;
                }
            }
            else
            {
                if (leadPosition.X > followerPosition.X)
                {
                    if (leadPosition.Y > followerPosition.Y)
                    {
                        followerPosition.X++;
                        followerPosition.Y++;
                    }
                    else
                    {
                        followerPosition.X++;
                        followerPosition.Y--;
                    }
                }
                else
                {
                    if (leadPosition.Y > followerPosition.Y)
                    {
                        followerPosition.X--;
                        followerPosition.Y++;
                    }
                    else
                    {
                        followerPosition.X--;
                        followerPosition.Y--;
                    }
                }
            }
        }

        static void ApplyStep(Point2d headPosition, RopeMove move)
        {
            switch (move.Direction)
            {
                case 'U':
                    headPosition.Y += 1;
                    break;
                case 'D':
                    headPosition.Y -= 1;
                    break;
                case 'R':
                    headPosition.X += 1;
                    break;
                case 'L':
                    headPosition.X -= 1;
                    break;
                default:
                    break;
            }
        }
    }

    public record RopeMove
    {
        public required char Direction { get; init; }
        public required int Magnitude { get; init; }

        public static RopeMove Parse(string input) =>
            new RopeMove {
                Direction = input[0],
                Magnitude = int.Parse(input[2..].AsSpan())
            };
    }

    public record Point2d
    {
        public required int X { get; set; }
        public required int Y { get; set; }
        /// <summary>
        /// Defined as the other point is in the same position, or is adjacent to
        /// the other <see cref="Point2d"/> including diagonally.
        /// </summary>
        public bool IsTouching(Point2d other)
        {
            return other.X >= X - 1 && other.X <= X + 1 &&
                other.Y >= Y - 1 && other.Y <= Y + 1;
        }
    }
}