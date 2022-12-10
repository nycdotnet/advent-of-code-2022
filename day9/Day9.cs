using common;
using System.Diagnostics;

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
            var result = Simulate(tailTracker: p => tailPositions.Add(p));
            return tailPositions.Count.ToString();
        }

        public (Point2d finalHeadPosition, Point2d finalTailPosition) Simulate(
            int movesToDo = -1,
            Action<Point2d> tailTracker = null)
        {
            var headPosition = new Point2d { X = 0, Y = 0 };
            var tailPosition = new Point2d { X = 0, Y = 0 };

            if (movesToDo == -1)
            {
                movesToDo = Moves.Count;
            }

            for (var i = 0; i < movesToDo; i++)
            {
                var move = Moves[i];
                for (var step = 0; step < move.Magnitude; step++)
                {
                    ApplyStep(headPosition, move);
                    SimulateTail(headPosition, tailPosition);
                    tailTracker?.Invoke(tailPosition);
                }
            }

            return (headPosition, tailPosition);

            static void SimulateTail(Point2d headPosition, Point2d tailPosition)
            {
                if (headPosition.IsTouching(tailPosition))
                {
                    return;
                }

                if (headPosition.X == tailPosition.X)
                {
                    if (tailPosition.Y < headPosition.Y)
                    {
                        tailPosition.Y++;
                    }
                    else
                    {
                        tailPosition.Y--;
                    }
                }
                else if (headPosition.Y == tailPosition.Y)
                {
                    if (tailPosition.X < headPosition.X)
                    {
                        tailPosition.X++;
                    }
                    else
                    {
                        tailPosition.X--;
                    }
                }
                else
                {
                    if (headPosition.X > tailPosition.X)
                    {
                        if (headPosition.Y > tailPosition.Y)
                        {
                            tailPosition.X++;
                            tailPosition.Y++;
                        }
                        else
                        {
                            tailPosition.X++;
                            tailPosition.Y--;
                        }
                    }
                    else
                    {
                        if (headPosition.Y > tailPosition.Y)
                        {
                            tailPosition.X--;
                            tailPosition.Y++;
                        }
                        else
                        {
                            tailPosition.X--;
                            tailPosition.Y--;
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

        public string GetAnswerForPart2()
        {
            throw new NotImplementedException();
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