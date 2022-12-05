using common;
using day5;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day5Tests
    {
        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day5(example_input_1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("CMZ");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day5>("day5.input.txt");
            var sut = new Day5(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("CNSZFDVLJ");
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day5(example_input_1.Split('\n'));
            sut.GetAnswerForPart2().Should().Be("MCD");
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day5>("day5.input.txt");
            var sut = new Day5(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("QNDWLMGNS");
        }

        public static readonly string example_input_1 = """
                [D]    
            [N] [C]    
            [Z] [M] [P]
             1   2   3 

            move 1 from 2 to 1
            move 3 from 1 to 3
            move 2 from 2 to 1
            move 1 from 1 to 2
            """.ReplaceLineEndings("\n");
    }
}
