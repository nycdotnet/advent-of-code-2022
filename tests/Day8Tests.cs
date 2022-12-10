using common;
using day8;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day8Tests
    {
        [Fact]
        public void ParsingWorksWithExampleData()
        {
            var sut = new Day8(example_input_1.ReplaceLineEndings("\n").Split('\n'));
            sut.Data.Length.Should().Be(5);
            sut.Data.Select(x => x.Length).Should().AllBeEquivalentTo(5);

            sut.Data[0][0].Should().Be(3);
            sut.Data[4][4].Should().Be(0);
        }

        [Fact]
        public void InitialVisibilityShowsAllEdgesVisible()
        {
            var sut = new Day8(example_input_1.ReplaceLineEndings("\n").Split('\n'));
            var visibility = sut.GetEmptyVisibilityMatrix();
            Day8.MarkExtemitiesVisible(visibility);

            visibility.First().Should().AllBeEquivalentTo(Day8.Visibility.Visible);
            visibility.Last().Should().AllBeEquivalentTo(Day8.Visibility.Visible);
            
            for (var i = 1; i < visibility.Length - 2; i++)
            {
                visibility[i][0].Should().Be(Day8.Visibility.Visible);
                visibility[i][1].Should().Be(Day8.Visibility.NotSure);
                visibility[i][2].Should().Be(Day8.Visibility.NotSure);
                visibility[i][3].Should().Be(Day8.Visibility.NotSure);
                visibility[i][4].Should().Be(Day8.Visibility.Visible);
            }
        }

        [Fact]
        public void MarkVisibleFromWestWorksAsExpected()
        {
            var sut = new Day8(interestingPyramid.Split('\n'));
            var vis = sut.GetEmptyVisibilityMatrix();
            sut.MarkVisibleFromWest(vis);
            Day8.FormatVisibility(vis).Should().Be("""
                00000
                01110
                01100
                01000
                00000
                """.ReplaceLineEndings("\n"));
        }

        [Fact]
        public void MarkVisibleFromNorthWorksAsExpected()
        {
            var sut = new Day8(interestingPyramid.Split('\n'));
            var vis = sut.GetEmptyVisibilityMatrix();
            sut.MarkVisibleFromWest(vis);
            Day8.FormatVisibility(vis).Should().Be("""
                00000
                01110
                01100
                01000
                00000
                """.ReplaceLineEndings("\n"));
        }

        [Fact]
        public void MarkVisibleFromEastWorksAsExpected()
        {
            var sut = new Day8(interestingPyramid.Split('\n'));
            var vis = sut.GetEmptyVisibilityMatrix();
            sut.MarkVisibleFromEast(vis);
            Day8.FormatVisibility(vis).Should().Be("""
                00000
                00010
                00110
                01110
                00000
                """.ReplaceLineEndings("\n"));
        }

        [Fact]
        public void MarkVisibleFromSouthWorksAsExpected()
        {
            var sut = new Day8(interestingPyramid.Split('\n'));
            var vis = sut.GetEmptyVisibilityMatrix();
            sut.MarkVisibleFromEast(vis);
            Day8.FormatVisibility(vis).Should().Be("""
                00000
                00010
                00110
                01110
                00000
                """.ReplaceLineEndings("\n"));
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day8(example_input_1.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("21");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day8>("day8.input.txt");
            var sut = new Day8(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("1832");
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day8(example_input_1.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("8");
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day8>("day8.input.txt");
            var sut = new Day8(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("157320");
        }

        public static readonly string example_input_1 = """
            30373
            25512
            65332
            33549
            35390
            """.ReplaceLineEndings("\n");

        public static readonly string interestingPyramid = """
            00000
            01230
            02920
            03210
            00000
            """.ReplaceLineEndings("\n");

        public static readonly string expectedVisibility1 = """
            11111
            11121
            11211
            12121
            11111
            """.ReplaceLineEndings("\n");
    }
}
