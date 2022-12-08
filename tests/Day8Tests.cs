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

            sut.Data[0][0].Should().Be('3');
            sut.Data[4][4].Should().Be('0');
        }

        public static readonly string example_input_1 = """
            30373
            25512
            65332
            33549
            35390
            """.ReplaceLineEndings("\n");
    }
}
