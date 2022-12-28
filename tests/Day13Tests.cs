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
        [InlineData(0, true)]
        [InlineData(1, true)]
        public void AnalyzeExampleInputProducesDocumentedResult(int index, bool expectedAreInRightOrder)
        {
            var sut = new Day13(exampleInput1.Split('\n'));
            var result = Day13.Analyze(sut.Pairs[index][0], sut.Pairs[index][1]);
            result.AreInRightOrder.Should().Be(expectedAreInRightOrder);
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
