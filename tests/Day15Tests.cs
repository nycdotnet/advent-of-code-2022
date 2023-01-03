using common;
using day14;
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
        public void DrawingBeaconAbsencesWorksAsExpected()
        {
            var sut = new Day15(exampleInput1.Split('\n'));
            var sensor = sut.Sensors.Where(s => s.Position.X == 8 && s.Position.Y == 7).Single();
            sut.MarkBeaconAbsence(sensor);

            var result = sut.Grid.RenderToStringWithInvertedY();

            result.startX.Should().Be(-2);
            result.startY.Should().Be(-2);
            result.endX.Should().Be(25);
            result.endY.Should().Be(22);

            result.content.Should().Be("""
                ..........#.................
                .........###................
                ....S...#####...............
                .......#######........S.....
                ......#########S............
                .....###########SB..........
                ....#############...........
                ...###############..........
                ..#################.........
                .#########S#######S#........
                ..#################.........
                ...###############..........
                ....B############...........
                ..S..###########............
                ......#########.............
                .......#######..............
                ........#####.S.......S.....
                B........###................
                ..........#SB...............
                ................S..........B
                ....S.......................
                ............................
                ............S......S........
                ............................
                .......................B....
                """.ReplaceLineEndings("\n"));
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day15(exampleInput1.Split('\n'));
            sut.MarkAllBeaconAbsences();
            sut.GetCountOfPositionsWhereBeaconCanNotBePresentInRow(10).Should().Be(26);
        }

        [Fact]
        public void RangeSetCanBeCreatedAndBasicInclusionsWork()
        {
            var rs = new RangeSet();
            rs.Include(10, 12);
            rs.GetState(9).Should().Be(RangeSet.State.Unspecified);
            rs.GetState(10).Should().Be(RangeSet.State.Included);
            rs.GetState(11).Should().Be(RangeSet.State.Included);
            rs.GetState(12).Should().Be(RangeSet.State.Included);
            rs.GetState(13).Should().Be(RangeSet.State.Unspecified);
        }

        [Fact]
        public void RangeSetOverlappingInclusionsWhenNewStartsLowerWillBeMerged()
        {
            var rs = new RangeSet();
            rs.Include(5, 10);
            rs.Include(4, 5);
            for (var i = 4; i <= 10; i++)
            {
                rs.GetState(i).Should().Be(RangeSet.State.Included);
            }
        }

        [Fact]
        public void RangeSetOverlappingInclusionsWhenNewStartsInMiddleAndEndsHigherWillBeMerged()
        {
            var rs = new RangeSet();
            rs.Include(5, 10);
            rs.Include(10, 11);

            rs.Include(15, 15);
            rs.Include(15, 16);

            for (var i = 5; i <= 10; i++)
            {
                rs.GetState(i).Should().Be(RangeSet.State.Included);
            }

            for (var i = 15; i <= 16; i++)
            {
                rs.GetState(i).Should().Be(RangeSet.State.Included);
            }
        }

        [Fact]
        public void RangeSetOverlappingInclusionsWhenNewStartsInsideWillNotBeMergedButCannotTell()
        {
            var rs = new RangeSet();
            rs.Include(5, 10);
            rs.IncludedRangeCount.Should().Be(1);
            rs.Include(5, 10);
            rs.IncludedRangeCount.Should().Be(1);
            rs.Include(5, 9);
            rs.IncludedRangeCount.Should().Be(1);
            rs.Include(6, 10);
            rs.IncludedRangeCount.Should().Be(1);
            rs.Include(6, 9);
            rs.IncludedRangeCount.Should().Be(1);

            rs.GetState(4).Should().Be(RangeSet.State.Unspecified);
            for (var i = 5; i <= 10; i++)
            {
                rs.GetState(i).Should().Be(RangeSet.State.Included);
            }
            rs.GetState(11).Should().Be(RangeSet.State.Unspecified);
        }


        [Fact]
        public void RangeSetCanBeCreatedAndBasicExclusionsWork()
        {
            var rs = new RangeSet();
            rs.Exclude(10, 12);
            rs.GetState(9).Should().Be(RangeSet.State.Unspecified);
            rs.GetState(10).Should().Be(RangeSet.State.Excluded);
            rs.GetState(11).Should().Be(RangeSet.State.Excluded);
            rs.GetState(12).Should().Be(RangeSet.State.Excluded);
            rs.GetState(13).Should().Be(RangeSet.State.Unspecified);
        }

        [Fact]
        public void RangeSetCanHandleMultipleInclusionsAndExclusions()
        {
            var rs = new RangeSet();
            rs.Include(2, 3);
            rs.Exclude(4, 5);
            rs.Include(6, 7);
            rs.Exclude(8, 9);
            rs.GetState(1).Should().Be(RangeSet.State.Unspecified);
            rs.GetState(2).Should().Be(RangeSet.State.Included);
            rs.GetState(3).Should().Be(RangeSet.State.Included);
            rs.GetState(4).Should().Be(RangeSet.State.Excluded);
            rs.GetState(5).Should().Be(RangeSet.State.Excluded);
            rs.GetState(6).Should().Be(RangeSet.State.Included);
            rs.GetState(7).Should().Be(RangeSet.State.Included);
            rs.GetState(8).Should().Be(RangeSet.State.Excluded);
            rs.GetState(9).Should().Be(RangeSet.State.Excluded);
            rs.GetState(10).Should().Be(RangeSet.State.Unspecified);
        }

        [Fact]
        public void RangeInclusionsAfterBigExclusionWorkAsExpected()
        {
            var rs = new RangeSet();
            rs.Exclude(1, 10);
            rs.Include(0, 2);
            rs.Include(4, 5);
            rs.Include(9, 10);

            rs.GetState(0).Should().Be(RangeSet.State.Included);
            rs.GetState(1).Should().Be(RangeSet.State.Included);
            rs.GetState(2).Should().Be(RangeSet.State.Included);
            rs.GetState(3).Should().Be(RangeSet.State.Excluded);
            rs.GetState(4).Should().Be(RangeSet.State.Included);
            rs.GetState(5).Should().Be(RangeSet.State.Included);
            rs.GetState(6).Should().Be(RangeSet.State.Excluded);
            rs.GetState(7).Should().Be(RangeSet.State.Excluded);
            rs.GetState(8).Should().Be(RangeSet.State.Excluded);
            rs.GetState(9).Should().Be(RangeSet.State.Included);
            rs.GetState(10).Should().Be(RangeSet.State.Included);
        }

        [Fact]
        public void RangeExclusionsAfterBigInclusionWorkAsExpected()
        {
            var rs = new RangeSet();
            rs.Include(1, 10);
            rs.Exclude(0, 2);
            rs.Exclude(4, 5);
            rs.Exclude(9, 10);

            rs.GetState(0).Should().Be(RangeSet.State.Excluded);
            rs.GetState(1).Should().Be(RangeSet.State.Excluded);
            rs.GetState(2).Should().Be(RangeSet.State.Excluded);
            rs.GetState(3).Should().Be(RangeSet.State.Included);
            rs.GetState(4).Should().Be(RangeSet.State.Excluded);
            rs.GetState(5).Should().Be(RangeSet.State.Excluded);
            rs.GetState(6).Should().Be(RangeSet.State.Included);
            rs.GetState(7).Should().Be(RangeSet.State.Included);
            rs.GetState(8).Should().Be(RangeSet.State.Included);
            rs.GetState(9).Should().Be(RangeSet.State.Excluded);
            rs.GetState(10).Should().Be(RangeSet.State.Excluded);
        }

        [Fact]
        public void RangeInclusionAfterSmallerExclusionsWorkAsExpected()
        {
            var rs = new RangeSet();
            rs.Exclude(1, 2);
            rs.Exclude(4, 5);
            rs.Exclude(7, 8);
            rs.Include(2, 7);

            rs.GetState(1).Should().Be(RangeSet.State.Excluded);
            rs.GetState(2).Should().Be(RangeSet.State.Included);
            rs.GetState(3).Should().Be(RangeSet.State.Included);
            rs.GetState(4).Should().Be(RangeSet.State.Included);
            rs.GetState(5).Should().Be(RangeSet.State.Included);
            rs.GetState(6).Should().Be(RangeSet.State.Included);
            rs.GetState(7).Should().Be(RangeSet.State.Included);
            rs.GetState(8).Should().Be(RangeSet.State.Excluded);
        }

        [Fact]
        public void ExcludingAdjacentDoesNotImpactIncluded()
        {
            var rs = new RangeSet();
            rs.Include(2, 3);
            rs.Exclude(1, 1);
            rs.Exclude(4, 4);
            rs.GetState(1).Should().Be(RangeSet.State.Excluded);
            rs.GetState(2).Should().Be(RangeSet.State.Included);
            rs.GetState(3).Should().Be(RangeSet.State.Included);
            rs.GetState(4).Should().Be(RangeSet.State.Excluded);
        }

        [Fact]
        public void RangeSetCanHandleReallyBigRangesFast()
        {
            var rs = new RangeSet();
            rs.Include(-999_999_999, 999_999_999);
            rs.Exclude(1_000_000_000, 2_000_000_000);
            rs.GetState(-888_888_888).Should().Be(RangeSet.State.Included);
            rs.GetState(2_000_000_000).Should().Be(RangeSet.State.Excluded);
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day15>("day15.input.txt")
                .ReplaceLineEndings("\n")
                .Split('\n');

            var sut = new Day15(input);
            sut.GetAnswerForPart1().Should().Be("0");
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

            var result = sut.RenderToStringWithInvertedY();
            result.content.Should().Be("""
                @...
                .%..
                ..o.
                ..*+
                """.ReplaceLineEndings("\n"));

            result.startX.Should().Be(-1);
            result.startY.Should().Be(-1);
            result.endX.Should().Be(2);
            result.endY.Should().Be(2);

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
            var result = sut.Grid.RenderToStringWithInvertedY();
            result.content.Should().Be("""
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
