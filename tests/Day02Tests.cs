using common;
using day02;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day02Tests
    {
        [Fact]
        public void ParsingStrategiesWorksAsExpected()
        {
            var sut = new Day02(example_input_1.Split('\n'));
            sut.Strategies[0].TheyPlay.Should().Be(Strategy.Play.Rock);
            sut.Strategies[0].Part1YouPlay.Should().Be(Strategy.Play.Paper);
            sut.Strategies[0].Part1Result.Should().Be(Strategy.Result.Win);
            sut.Strategies[0].Part1ScoreResult.Should().Be(8);
            sut.Strategies[0].Part1ScoreResultAlt.Should().Be(8);
            sut.Strategies[0].Part1ScoreResultAlt2.Should().Be(8);
            sut.Strategies[0].Part2RequiredResult.Should().Be(Strategy.Result.Draw);
            sut.Strategies[0].Part2ScoreResult.Should().Be(4);

            sut.Strategies[1].TheyPlay.Should().Be(Strategy.Play.Paper);
            sut.Strategies[1].Part1YouPlay.Should().Be(Strategy.Play.Rock);
            sut.Strategies[1].Part1Result.Should().Be(Strategy.Result.Lose);
            sut.Strategies[1].Part1ScoreResult.Should().Be(1);
            sut.Strategies[1].Part1ScoreResultAlt.Should().Be(1);
            sut.Strategies[1].Part1ScoreResultAlt2.Should().Be(1);
            sut.Strategies[1].Part2RequiredResult.Should().Be(Strategy.Result.Lose);
            sut.Strategies[1].Part2ScoreResult.Should().Be(1);

            sut.Strategies[2].TheyPlay.Should().Be(Strategy.Play.Scissors);
            sut.Strategies[2].Part1YouPlay.Should().Be(Strategy.Play.Scissors);
            sut.Strategies[2].Part1Result.Should().Be(Strategy.Result.Draw);
            sut.Strategies[2].Part1ScoreResult.Should().Be(6);
            sut.Strategies[2].Part1ScoreResultAlt.Should().Be(6);
            sut.Strategies[2].Part1ScoreResultAlt2.Should().Be(6);
            sut.Strategies[2].Part2RequiredResult.Should().Be(Strategy.Result.Win);
            sut.Strategies[2].Part2ScoreResult.Should().Be(7);
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day02(example_input_1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("15");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day02>("day02.input.txt");
            var sut = new Day02(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("12794");
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day02(example_input_1.Split('\n'));
            sut.GetAnswerForPart2().Should().Be("12");
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day02>("day02.input.txt");
            var sut = new Day02(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("14979");
        }

        public static readonly string example_input_1 = """
            A Y
            B X
            C Z
            """.ReplaceLineEndings("\n");

    }
}
