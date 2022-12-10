﻿using common;
using day8;
using day9;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day9Tests
    {
        [Fact]
        public void ParsingWorks()
        {
            var sut = new Day9(exampleInput1.Split('\n'));
            sut.Moves.Should().HaveCount(8);
            sut.Moves[0].Should().Be(new RopeMove { Direction = 'R', Magnitude = 4 });
            sut.Moves[7].Should().Be(new RopeMove { Direction = 'R', Magnitude = 2 });
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(1, 4, 0, 3, 0)]
        [InlineData(2, 4, 4, 4, 3)]
        [InlineData(3, 1, 4, 2, 4)]
        [InlineData(4, 1, 3, 2, 4)]
        [InlineData(5, 5, 3, 4, 3)]
        [InlineData(6, 5, 2, 4, 3)]
        [InlineData(7, 0, 2, 1, 2)]
        [InlineData(8, 2, 2, 1, 2)]
        public void ExercisePart1(int simulateSteps,
            int expectedHeadX,
            int expectedHeadY,
            int expectedTailX,
            int expectedTailY)
        {
            var sut = new Day9(exampleInput1.Split('\n'));
            
            var result = sut.Simulate(simulateSteps);
            result.finalHeadPosition.X.Should().Be(expectedHeadX);
            result.finalHeadPosition.Y.Should().Be(expectedHeadY);
            result.finalTailPosition.X.Should().Be(expectedTailX);
            result.finalTailPosition.Y.Should().Be(expectedTailY);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, true)]
        [InlineData(0, 0, 0, 1, true)]
        [InlineData(0, 0, 1, 1, true)]
        [InlineData(0, 0, 1, 0, true)]
        [InlineData(0, 0, 1, -1, true)]
        [InlineData(0, 0, 0, -1, true)]
        [InlineData(0, 0, -1, -1, true)]
        [InlineData(0, 0, -1, 0, true)]
        [InlineData(0, 0, -1, 1, true)]
        [InlineData(0, 0, 0, 2, false)]
        [InlineData(0, 0, 2, 2, false)]
        [InlineData(0, 0, 2, 0, false)]
        [InlineData(0, 0, 2, -2, false)]
        [InlineData(0, 0, 0, -2, false)]
        [InlineData(0, 0, -2, -2, false)]
        [InlineData(0, 0, -2, 0, false)]
        [InlineData(0, 0, -2, 2, false)]
        public void Point2dIsTouchingWorksAsExpected(int AX, int AY, int BX, int BY, bool expected)
        {
            var a = new Point2d { X = AX, Y = AY };
            var b = new Point2d { X = BX, Y = BY };
            a.IsTouching(b).Should().Be(expected);
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day9(exampleInput1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("13");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day9>("day9.input.txt");
            var sut = new Day9(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("6503");
        }

        public readonly static string exampleInput1 = """
            R 4
            U 4
            L 3
            D 1
            R 4
            D 1
            L 5
            R 2
            """.ReplaceLineEndings("\n");
    }
}
