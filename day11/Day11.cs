using common;
using System.Text.RegularExpressions;

namespace day11
{
    public class Day11 : IAdventOfCodeDay
    {
        public Day11(IEnumerable<string> input)
        {
            Monkeys = input
                .SelectPartition(string.IsNullOrEmpty)
                .Select(Monkey.Parse)
                .ToList();
        }

        public List<Monkey> Monkeys { get; }

        public string GetAnswerForPart1()
        {
            throw new NotImplementedException();
        }

        public string GetAnswerForPart2()
        {
            throw new NotImplementedException();
        }
    }

    public class Monkey
    {
        public static Monkey Parse(IReadOnlyList<string> input)
        {
            var ops = OperationRegex.Match(input[2]);


            var result = new Monkey {
                Id = int.Parse(MonkeyIdRegex.Match(input[0]).Groups[nameof(Id)].ValueSpan),
                Items = StartingItemsRegex
                    .Match(input[1])
                    .Groups[nameof(Items)]
                    .Value
                    .Split(", ")
                    .Select(int.Parse)
                    .ToList(),
                Operation = new MonkeyOp
                {
                    A = ops.Groups[nameof(MonkeyOp.A)].Value,
                    Op = ops.Groups[nameof(MonkeyOp.Op)].Value,
                    B = ops.Groups[nameof(MonkeyOp.B)].Value,
                },
                TestDivisor = int.Parse(TestRegex.Match(input[3]).Groups["divisor"].ValueSpan),
                IfTrueMonkeyId = int.Parse(IfTrueRegex.Match(input[4]).Groups["monkey"].ValueSpan),
                IfFalseMonkeyId = int.Parse(IfFalseRegex.Match(input[5]).Groups["monkey"].ValueSpan)
            };
            return result;
        }


        public static readonly Regex MonkeyIdRegex = new Regex(@"Monkey (?<Id>\d*):");
        public static readonly Regex StartingItemsRegex = new Regex(@"Starting items: (?<Items>.*)");
        public static readonly Regex OperationRegex = new Regex(@"Operation: new = (?<A>\S*) (?<Op>\+|\*) (?<B>\S*)");
        public static readonly Regex TestRegex = new Regex(@"Test: divisible by (?<divisor>\d*)");
        public static readonly Regex IfTrueRegex = new Regex(@"If true: throw to monkey (?<monkey>\d*)");
        public static readonly Regex IfFalseRegex = new Regex(@"If false: throw to monkey (?<monkey>\d*)");


        public required int Id { get; init; }
        public required List<int> Items { get; init; }
        public required MonkeyOp Operation { get; init; }
        public required int TestDivisor { get; init; }
        public required int IfTrueMonkeyId { get; init; }
        public required int IfFalseMonkeyId { get; init; }
    }

    public record MonkeyOp
    {
        public required string A { get; init; }
        public required string Op { get; init; }
        public required string B { get; init; }
    }
}