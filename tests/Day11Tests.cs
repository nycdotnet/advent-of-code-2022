using common;
using day11;
using FluentAssertions;
using System.Numerics;
using Xunit;

namespace tests
{
    public class Day11Tests
    {
        [Fact]
        public void ParsingWorksCorrectly()
        {
            var sut = new Day11(exampleInput1.Split('\n'));
            sut.Monkeys.Count.Should().Be(4);

            sut.Monkeys[0].Id.Should().Be(0);
            sut.Monkeys[0].Items.Should().Equal(new BigInteger[] { 79, 98 });
            sut.Monkeys[0].Operation.Op.Should().Be("*");
            sut.Monkeys[0].Operation.Other.Should().Be("19");
            sut.Monkeys[0].TestDivisor.Should().Be(23);
            sut.Monkeys[0].IfTrueMonkeyId.Should().Be(2);
            sut.Monkeys[0].IfFalseMonkeyId.Should().Be(3);
            sut.Monkeys[1].Id.Should().Be(1);
            sut.Monkeys[1].Items.Should().Equal(new BigInteger[] { 54, 65, 75, 74 });
            sut.Monkeys[2].Id.Should().Be(2);
            sut.Monkeys[2].Items.Should().Equal(new BigInteger[] { 79, 60, 97 });
            sut.Monkeys[3].Id.Should().Be(3);
            sut.Monkeys[3].Items.Should().Equal(new BigInteger[] { 74 });
        }

        [Fact]
        public void FirstExampleRoundIsSimulatedAsDocumented()
        {
            var sut = new Day11(exampleInput1.Split('\n'));
            var output = new List<string>();
            sut.SimulateRound(output.Add);

            string.Join('\n', output).Should().Be(exampleRound1Narration);
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day11(exampleInput1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("10605");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day11>("day11.input.txt");
            var sut = new Day11(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("58794");
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResultToRound1()
        {
            var sut = new Day11(exampleInput1.Split('\n'), wfa => wfa);
            var result = sut.GetMonkeyInspections(1);
            result[0].Should().Be(2);
            result[1].Should().Be(4);
            result[2].Should().Be(3);
            result[3].Should().Be(6);
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResultToRound20()
        {
            var sut = new Day11(exampleInput1.Split('\n'), wfa => wfa);
            var result = sut.GetMonkeyInspections(20);
            result[0].Should().Be(99);
            result[1].Should().Be(97);
            result[2].Should().Be(8);
            result[3].Should().Be(103);
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResultToRound1000()
        {
            var sut = new Day11(exampleInput1.Split('\n'), wfa => wfa);
            var result = sut.GetMonkeyInspections(1000);
            result[0].Should().Be(5204);
            result[1].Should().Be(4792);
            result[2].Should().Be(199);
            result[3].Should().Be(5192);
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day11>("day11.input.txt");
            var sut = new Day11(input.ReplaceLineEndings("\n").Split('\n'), wfa => wfa);
            sut.GetAnswerForPart2().Should().Be("20151213744");
        }

        private static readonly string exampleInput1 = """
            Monkey 0:
              Starting items: 79, 98
              Operation: new = old * 19
              Test: divisible by 23
                If true: throw to monkey 2
                If false: throw to monkey 3

            Monkey 1:
              Starting items: 54, 65, 75, 74
              Operation: new = old + 6
              Test: divisible by 19
                If true: throw to monkey 2
                If false: throw to monkey 0

            Monkey 2:
              Starting items: 79, 60, 97
              Operation: new = old * old
              Test: divisible by 13
                If true: throw to monkey 1
                If false: throw to monkey 3

            Monkey 3:
              Starting items: 74
              Operation: new = old + 3
              Test: divisible by 17
                If true: throw to monkey 0
                If false: throw to monkey 1
            """.ReplaceLineEndings("\n");

        private static readonly string exampleRound1Narration = """
            Monkey 0:
              Monkey inspects an item with a worry level of 79.
                Worry level is multiplied by 19 to 1501.
                Monkey gets bored with item. Worry level is divided by 3 to 500.
                Current worry level is not divisible by 23.
                Item with worry level 500 is thrown to monkey 3.
              Monkey inspects an item with a worry level of 98.
                Worry level is multiplied by 19 to 1862.
                Monkey gets bored with item. Worry level is divided by 3 to 620.
                Current worry level is not divisible by 23.
                Item with worry level 620 is thrown to monkey 3.
            Monkey 1:
              Monkey inspects an item with a worry level of 54.
                Worry level increases by 6 to 60.
                Monkey gets bored with item. Worry level is divided by 3 to 20.
                Current worry level is not divisible by 19.
                Item with worry level 20 is thrown to monkey 0.
              Monkey inspects an item with a worry level of 65.
                Worry level increases by 6 to 71.
                Monkey gets bored with item. Worry level is divided by 3 to 23.
                Current worry level is not divisible by 19.
                Item with worry level 23 is thrown to monkey 0.
              Monkey inspects an item with a worry level of 75.
                Worry level increases by 6 to 81.
                Monkey gets bored with item. Worry level is divided by 3 to 27.
                Current worry level is not divisible by 19.
                Item with worry level 27 is thrown to monkey 0.
              Monkey inspects an item with a worry level of 74.
                Worry level increases by 6 to 80.
                Monkey gets bored with item. Worry level is divided by 3 to 26.
                Current worry level is not divisible by 19.
                Item with worry level 26 is thrown to monkey 0.
            Monkey 2:
              Monkey inspects an item with a worry level of 79.
                Worry level is multiplied by itself to 6241.
                Monkey gets bored with item. Worry level is divided by 3 to 2080.
                Current worry level is divisible by 13.
                Item with worry level 2080 is thrown to monkey 1.
              Monkey inspects an item with a worry level of 60.
                Worry level is multiplied by itself to 3600.
                Monkey gets bored with item. Worry level is divided by 3 to 1200.
                Current worry level is not divisible by 13.
                Item with worry level 1200 is thrown to monkey 3.
              Monkey inspects an item with a worry level of 97.
                Worry level is multiplied by itself to 9409.
                Monkey gets bored with item. Worry level is divided by 3 to 3136.
                Current worry level is not divisible by 13.
                Item with worry level 3136 is thrown to monkey 3.
            Monkey 3:
              Monkey inspects an item with a worry level of 74.
                Worry level increases by 3 to 77.
                Monkey gets bored with item. Worry level is divided by 3 to 25.
                Current worry level is not divisible by 17.
                Item with worry level 25 is thrown to monkey 1.
              Monkey inspects an item with a worry level of 500.
                Worry level increases by 3 to 503.
                Monkey gets bored with item. Worry level is divided by 3 to 167.
                Current worry level is not divisible by 17.
                Item with worry level 167 is thrown to monkey 1.
              Monkey inspects an item with a worry level of 620.
                Worry level increases by 3 to 623.
                Monkey gets bored with item. Worry level is divided by 3 to 207.
                Current worry level is not divisible by 17.
                Item with worry level 207 is thrown to monkey 1.
              Monkey inspects an item with a worry level of 1200.
                Worry level increases by 3 to 1203.
                Monkey gets bored with item. Worry level is divided by 3 to 401.
                Current worry level is not divisible by 17.
                Item with worry level 401 is thrown to monkey 1.
              Monkey inspects an item with a worry level of 3136.
                Worry level increases by 3 to 3139.
                Monkey gets bored with item. Worry level is divided by 3 to 1046.
                Current worry level is not divisible by 17.
                Item with worry level 1046 is thrown to monkey 1.
            """.ReplaceLineEndings("\n");
    }
}
