using common;
using day5;
using day6;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day6Tests
    {
        [Theory]
        [MemberData(nameof(BuffersWithPacketStart))]
        public void StartOfPacketWorksAsExpected(string buffer, int expectedIndex)
        {
            var sut = new Day6(new string[] { buffer });
            sut.StartOfPacketMarkerIndex().Should().Be(expectedIndex);
        }

        [Theory]
        [MemberData(nameof(BuffersWithMessageStart))]
        public void StartOfMessageWorksAsExpected(string buffer, int expectedIndex)
        {
            var sut = new Day6(new string[] { buffer });
            sut.StartOfMessageMarkerIndex().Should().Be(expectedIndex);
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day6>("day6.input.txt");
            var sut = new Day6(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("1909");
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day6>("day6.input.txt");
            var sut = new Day6(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("3380");
        }

        public static readonly TheoryData<string, int> BuffersWithPacketStart = new() {
            { "mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7 },
            { "bvwbjplbgvbhsrlpgdmjqwftvncz", 5 },
            { "nppdvjthqldpwncqszvftbrmjlhg", 6 },
            { "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10 },
            { "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11 }
        };

        public static readonly TheoryData<string, int> BuffersWithMessageStart = new() {
            { "mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19 },
            { "bvwbjplbgvbhsrlpgdmjqwftvncz", 23 },
            { "nppdvjthqldpwncqszvftbrmjlhg", 23 },
            { "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29 },
            { "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26 }
        };
    }
}
