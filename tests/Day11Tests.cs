using day11;
using FluentAssertions;
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
            sut.Monkeys[0].Items.Should().Equal(new int[] { 79, 98 });
            sut.Monkeys[0].Operation.A.Should().Be("old");
            sut.Monkeys[0].Operation.Op.Should().Be("*");
            sut.Monkeys[0].Operation.B.Should().Be("19");
            sut.Monkeys[0].TestDivisor.Should().Be(23);
            sut.Monkeys[0].IfTrueMonkeyId.Should().Be(2);
            sut.Monkeys[0].IfFalseMonkeyId.Should().Be(3);
            sut.Monkeys[1].Id.Should().Be(1);
            sut.Monkeys[1].Items.Should().Equal(new int[] { 54, 65, 75, 74 });
            sut.Monkeys[2].Id.Should().Be(2);
            sut.Monkeys[2].Items.Should().Equal(new int[] { 79, 60, 97 });
            sut.Monkeys[3].Id.Should().Be(3);
            sut.Monkeys[3].Items.Should().Equal(new int[] { 74 });
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
    }
}
