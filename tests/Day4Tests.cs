using common;
using day4;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day4Tests
    {
        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day4(example_input_1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("2");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day4>("day4.input.txt");
            var sut = new Day4(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("644");
        }

        [Theory]
        [InlineData("1-3,4-6", false)]
        [InlineData("2-4,4-6", true)]
        [InlineData("3-5,4-6", true)]
        [InlineData("4-6,4-6", true)]
        [InlineData("5-7,4-6", true)]
        [InlineData("6-8,4-6", true)]
        [InlineData("7-9,4-6", false)]
        public void OverlapsWorksAsExpected(string inputText, bool expectedResult)
        {
            var sut = new ElfPair(inputText);
            ElfPair.Overlaps(sut.First, sut.Second).Should().Be(expectedResult);
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day4(example_input_1.Split('\n'));
            sut.GetAnswerForPart2().Should().Be("4");
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day4>("day4.input.txt");
            var sut = new Day4(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("926");
        }

        public static readonly string example_input_1 = """
            2-4,6-8
            2-3,4-5
            5-7,7-9
            2-8,3-7
            6-6,4-6
            2-6,4-8
            """.ReplaceLineEndings("\n");
    }
}
