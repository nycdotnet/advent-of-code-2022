using common;
using day1;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day1Tests
    {
        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day1(example_input_1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("24000");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day1>("day1.input.txt");
            var sut = new Day1(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("70374");
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day1(example_input_1.Split('\n'));
            sut.GetAnswerForPart2().Should().Be("45000");
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day1>("day1.input.txt");
            var sut = new Day1(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("204610");
        }

        public static readonly string example_input_1 = """
            1000
            2000
            3000

            4000

            5000
            6000

            7000
            8000
            9000

            10000
            """.ReplaceLineEndings("\n");
    }
}