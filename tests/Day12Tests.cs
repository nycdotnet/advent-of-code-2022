using common;
using day11;
using day12;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day12Tests
    {
        [Fact]
        public void ParsingWorksAsExpected()
        {
            var sut = new Day12(exampleInput1.Split('\n'));
            sut.Map.Length.Should().Be(5);
            sut.Map.Select(x => x.Length).Should().AllBeEquivalentTo(8);
            sut.Start.X.Should().Be(0);
            sut.Start.Y.Should().Be(0);
            sut.End.X.Should().Be(5);
            sut.End.Y.Should().Be(2);
        }

        [Fact]
        public void GetNeighborsAtStartHasEastAndSouth()
        {
            var sut = new Day12(exampleInput1.Split('\n'));
            var neighbors = sut.GetNeighbors(new Point2d { X = 0, Y = 0 }).ToList();
            neighbors.Count.Should().Be(2);
            neighbors.Should().Contain(new Point2d { X = 1, Y = 0 });
            neighbors.Should().Contain(new Point2d { X = 0, Y = 1 });
        }

        [Fact]
        public void GetNeighborsAtOneOneHasAll()
        {
            var sut = new Day12(exampleInput1.Split('\n'));
            var neighbors = sut.GetNeighbors(new Point2d { X = 1, Y = 1 }).ToList();
            neighbors.Count.Should().Be(4);
            neighbors.Should().Contain(new Point2d { X = 1, Y = 0 });
            neighbors.Should().Contain(new Point2d { X = 0, Y = 1 });
            neighbors.Should().Contain(new Point2d { X = 1, Y = 2 });
            neighbors.Should().Contain(new Point2d { X = 2, Y = 1 });
        }

        [Fact]
        public void GetNeighborsAtX3Y1HasExpected()
        {
            var sut = new Day12(exampleInput1.Split('\n'));
            var neighbors = sut.GetNeighbors(new Point2d { X = 3, Y = 1 }).ToList();
            neighbors.Count.Should().Be(3);
            neighbors.Should().Contain(new Point2d { X = 3, Y = 0 });
            neighbors.Should().Contain(new Point2d { X = 3, Y = 2 });
            neighbors.Should().Contain(new Point2d { X = 2, Y = 1 });
        }

        [Fact]
        public void GetNeighborsContrivedAtX1Y3HasExpected()
        {
            var sut = new Day12(contrivedInput1.Split('\n'));
            var neighbors = sut.GetNeighbors(new Point2d { X = 1, Y = 3 }).ToList();
            neighbors.Count.Should().Be(3);
            neighbors.Should().Contain(new Point2d { X = 1, Y = 2 });
            neighbors.Should().Contain(new Point2d { X = 0, Y = 3 });
            neighbors.Should().Contain(new Point2d { X = 2, Y = 3 });
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day12(exampleInput1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("31");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day12>("day12.input.txt")
                .ReplaceLineEndings("\n")
                .Split('\n');
            var sut = new Day12(input);
            sut.GetAnswerForPart1().Should().Be("447");
        }

        [Fact]
        public void Part1WithSampleDataProducesDocumentedDebugPath()
        {
            var sut = new Day12(exampleInput1.Split('\n'));
            var aStar = sut.AnalyzePath(sut.Start);
            var path = sut.GetDebugPathFromBackwardsPath(aStar.GetOptimalPathBackward().ToArray());
            path.Should().Be(exampleDebugPath1);
        }

        [Fact]
        public void ContrivedPathWorksAsExpected()
        {
            var sut = new Day12(contrivedInput1.Split('\n'));

            sut.GetAnswerForPart1().Should().Be("25");
        }

        [Fact]
        public void Part2StartingPointsWithExampleInput()
        {
            var sut = new Day12(exampleInput1.Split('\n'));
            sut.Part2StartingPoints().ToList().Should().HaveCount(6);
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day12>("day12.input.txt")
                .ReplaceLineEndings("\n")
                .Split('\n');
            var sut = new Day12(input);
            sut.GetAnswerForPart2().Should().Be("446");
        }

        public static readonly string exampleInput1 = """
            Sabqponm
            abcryxxl
            accszExk
            acctuvwj
            abdefghi
            """.ReplaceLineEndings("\n");

        /// <summary>
        /// NOTE: This is not the actual example input from the web site, but it is equivalent since
        /// there are the same number of steps and all the rules are consistently obeyed.  My algorithm
        /// made an equivalent decision to go east at X = 1, Y = 1 whereas the example goes south, but
        /// both travel from a 'b' elevation to a 'c' elevation, and both follow-up to travel to X = 2,
        /// Y = 2 which is also a 'c' elevation.
        /// </summary>
        public static readonly string exampleDebugPath1 = """
            v..v<<<<
            >>vvv<<^
            ..vv>E^^
            ..v>>>^^
            ..>>>>>^
            """.ReplaceLineEndings("\n");

        public static readonly string contrivedInput1 = """
            Sbcdef
            lkjihg
            mnopqr
            xwvuts
            yEzzzz
            """.ReplaceLineEndings("\n");
    }
}
