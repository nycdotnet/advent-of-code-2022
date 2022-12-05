using common;
using day2;
using day3;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace tests
{
    public class Day3Tests
    {
        [Fact]
        public void CompartmentsAreIdentifiedCorrectly()
        {
            var sut = new Day3(example_input_1.Split('\n'));

            sut.Rucksacks.Count.Should().Be(6);

            sut.Rucksacks[0].Contents.Should().Be("vJrwpWtwJgWrhcsFMMfFFhFp");
            sut.Rucksacks[0].FirstCompartment.Should().Be("vJrwpWtwJgWr");
            sut.Rucksacks[0].SecondCompartment.Should().Be("hcsFMMfFFhFp");

            sut.Rucksacks[1].FirstCompartment.Should().Be("jqHRNqRjqzjGDLGL");
            sut.Rucksacks[1].SecondCompartment.Should().Be("rsFMfFZSrLrFZsSL");
        }

        [Theory]
        [InlineData('a', 1)]
        [InlineData('z', 26)]
        [InlineData('A', 27)]
        [InlineData('Z', 52)]
        public void PrioritiesWorkAsDocumented(char item, int expectedPriority)
        {
            Day3.Priority(item).Should().Be(expectedPriority);
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day3(example_input_1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("157");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day3>("day3.input.txt");
            var sut = new Day3(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("7737");
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day3(example_input_1.Split('\n'));
            sut.GetAnswerForPart2().Should().Be("70");
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day3>("day3.input.txt");
            var sut = new Day3(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("2697");
        }

        public static readonly string example_input_1 = """
            vJrwpWtwJgWrhcsFMMfFFhFp
            jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
            PmmdzqPrVvPwwTWBwg
            wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
            ttgJtRGJQctTZtZT
            CrZsJsPPZsGzwwsLwLmpwMDw
            """.ReplaceLineEndings("\n");
    }
}
