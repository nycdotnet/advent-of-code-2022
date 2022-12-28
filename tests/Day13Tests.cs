using common;
using day13;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day13Tests
    {
        [Fact]
        public void InitialParsingWorks()
        {
            var sut = new Day13(exampleInput1.Split('\n'));
            sut.Pairs.Should().HaveCount(8);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(4, true)]
        [InlineData(5, false)]
        [InlineData(6, true)]
        [InlineData(7, false)]
        [InlineData(8, false)]
        public void AnalyzeExampleInputProducesDocumentedResult(int oneBasedIndex, bool expectedAreInRightOrder)
        {
            var sut = new Day13(exampleInput1.Split('\n'));
            var result = Day13.AnalyzePackets(sut.Pairs[oneBasedIndex - 1][0], sut.Pairs[oneBasedIndex - 1][1]);
            result.AreInRightOrder.Should().Be(expectedAreInRightOrder);
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day13(exampleInput1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("13");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day13>("day13.input.txt")
                .ReplaceLineEndings("\n")
                .Split('\n');
            var sut = new Day13(input);
            sut.GetAnswerForPart1().Should().Be("5717");
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day13(exampleInput1.Split('\n'));
            sut.GetAnswerForPart2().Should().Be("140");
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day13>("day13.input.txt")
                .ReplaceLineEndings("\n")
                .Split('\n');
            var sut = new Day13(input);
            sut.GetAnswerForPart2().Should().Be("25935");
        }


        public static readonly string exampleInput1 = """
            [1,1,3,1,1]
            [1,1,5,1,1]

            [[1],[2,3,4]]
            [[1],4]

            [9]
            [[8,7,6]]

            [[4,4],4,4]
            [[4,4],4,4,4]

            [7,7,7,7]
            [7,7,7]

            []
            [3]

            [[[]]]
            [[]]

            [1,[2,[3,[4,[5,6,7]]]],8,9]
            [1,[2,[3,[4,[5,6,0]]]],8,9]
            """.ReplaceLineEndings("\n");
    }
}
