using day15;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day15Tests
    {
        [Fact]
        public void ParsingWorksAsExpected()
        {
            var sut = new Day15(exampleInput1.Split('\n'));
            sut.Sensors.Count.Should().Be(14);

            sut.Sensors[0].Position.X.Should().Be(2);
            sut.Sensors[0].Position.Y.Should().Be(18);
            sut.Sensors[0].ClosestBeacon.X.Should().Be(-2);
            sut.Sensors[0].ClosestBeacon.Y.Should().Be(15);

            sut.Sensors[13].Position.X.Should().Be(20);
            sut.Sensors[13].Position.Y.Should().Be(1);
            sut.Sensors[13].ClosestBeacon.X.Should().Be(15);
            sut.Sensors[13].ClosestBeacon.Y.Should().Be(3);

            sut.Grid.MaxX.Should().Be(25);
            sut.Grid.MinX.Should().Be(-2);
            sut.Grid.MaxY.Should().Be(22);
            sut.Grid.MinY.Should().Be(0);
        }

        [Fact]
        public void EmptyTwoDimensionalGridSupportsMaxAndMinFunctions()
        {
            var sut = new TwoDimensionalGrid<char, int>(' ', (_, _, c) => c);
            sut.MaxX.Should().Be(0);
            sut.MinX.Should().Be(0);
            sut.MaxY.Should().Be(0);
            sut.MinY.Should().Be(0);
        }

        [Fact]
        public void TwoDimensionalGridSupportsAddingAndGettingSuccessfully()
        {
            var sut = new TwoDimensionalGrid<char, int>(' ', (_, _, c) => c);

            sut.Push('o', 1, 1);
            sut.Push('*', 1, 2);
            sut.Push('+', 2, 2);
            sut.Push('#', -1, -1);
            sut.Push('@', -1, -1);

            sut.CellsByX.Should().HaveCount(3);

            sut.Get(1, 1).Should().Be('o');
            sut.Get(1, 2).Should().Be('*');
            sut.Get(2, 2).Should().Be('+');
            sut.Get(-1, -1).Should().Be('@');
            sut.Get(0, 0).Should().Be(' ');
        }

        [Fact]
        public void RenderingTwoDimensionalGridToStringWithRendererWorksAsExpected()
        {
            var sut = new TwoDimensionalGrid<char, int>('.', RenderOriginAsPercent);

            sut.Push('o', 1, 1);
            sut.Push('*', 1, 2);
            sut.Push('+', 2, 2);
            sut.Push('#', -1, -1);
            sut.Push('@', -1, -1);

            var result = sut.RenderToString();
            result.Should().Be("""
                @...
                .%..
                ..o.
                ..*+
                """.ReplaceLineEndings("\n"));

            static char RenderOriginAsPercent(int x, int y, char c)
            {
                if (x == 0 && y == 0)
                {
                    return '%';
                }
                return c;
            }
        }

        [Fact]
        public void RenderingTwoDimensionalGridWithExampleDataToStringWorksAsExpected()
        {
            var sut = new Day15(exampleInput1.Split('\n'));
            var result = sut.Grid.RenderToString();
            result.Should().Be("""
                ....S.......................
                ......................S.....
                ...............S............
                ................SB..........
                ............................
                ............................
                ............................
                ..........S.......S.........
                ............................
                ............................
                ....B.......................
                ..S.........................
                ............................
                ............................
                ..............S.......S.....
                B...........................
                ...........SB...............
                ................S..........B
                ....S.......................
                ............................
                ............S......S........
                ............................
                .......................B....
                """.ReplaceLineEndings("\n"));
        }


        public static readonly string exampleInput1 = """
            Sensor at x=2, y=18: closest beacon is at x=-2, y=15
            Sensor at x=9, y=16: closest beacon is at x=10, y=16
            Sensor at x=13, y=2: closest beacon is at x=15, y=3
            Sensor at x=12, y=14: closest beacon is at x=10, y=16
            Sensor at x=10, y=20: closest beacon is at x=10, y=16
            Sensor at x=14, y=17: closest beacon is at x=10, y=16
            Sensor at x=8, y=7: closest beacon is at x=2, y=10
            Sensor at x=2, y=0: closest beacon is at x=2, y=10
            Sensor at x=0, y=11: closest beacon is at x=2, y=10
            Sensor at x=20, y=14: closest beacon is at x=25, y=17
            Sensor at x=17, y=20: closest beacon is at x=21, y=22
            Sensor at x=16, y=7: closest beacon is at x=15, y=3
            Sensor at x=14, y=3: closest beacon is at x=15, y=3
            Sensor at x=20, y=1: closest beacon is at x=15, y=3
            """.ReplaceLineEndings("\n");
    }
}
