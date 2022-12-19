namespace common
{
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

        public int ManhattanDistance(Point2d other) =>
            Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
    }
}
