using common;
using System.Diagnostics;
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

        public void SimulateRound(Action<string>? logger = null)
        {
            for (var monkeyIndex = 0; monkeyIndex < Monkeys.Count; monkeyIndex++)
            {
                var monkey = Monkeys[monkeyIndex];
                logger?.Invoke($"Monkey {monkeyIndex}:");

                for (var itemIndex = 0; itemIndex < monkey.Items.Count; itemIndex++)
                {
                    var worryLevel = monkey.Items[itemIndex];
                    logger?.Invoke($"  Monkey inspects an item with a worry level of {worryLevel}.");

                    // do op.
                    if (monkey.Operation.Op == "*")
                    {
                        if (monkey.Operation.Other == "old")
                        {
                            worryLevel = worryLevel * worryLevel;
                            logger?.Invoke($"    Worry level is multiplied by itself to {worryLevel}.");
                        }
                        else
                        {
                            var multiplier = int.Parse(monkey.Operation.Other);
                            worryLevel = worryLevel * multiplier;
                            logger?.Invoke($"    Worry level is multiplied by {multiplier} to {worryLevel}.");
                        }
                    }
                    else if (monkey.Operation.Op == "+")
                    {
                        var other = int.Parse(monkey.Operation.Other);
                        worryLevel = worryLevel + other;
                        logger?.Invoke($"    Worry level increases by {other} to {worryLevel}.");
                    }
                    else
                    {
                        throw new UnreachableException($"Not supported operator {monkey.Operation.Op}.");
                    }

                    worryLevel /= 3;
                    logger?.Invoke($"    Monkey gets bored with item. Worry level is divided by 3 to {worryLevel}.");

                    // do test and "throw" to the other monkey
                    if (worryLevel % monkey.TestDivisor == 0)
                    {
                        logger?.Invoke($"    Current worry level is divisible by {monkey.TestDivisor}.");
                        logger?.Invoke($"    Item with worry level {worryLevel} is thrown to monkey {monkey.IfTrueMonkeyId}.");
                        Monkeys[monkey.IfTrueMonkeyId].Items.Add(worryLevel);
                    }
                    else
                    {
                        logger?.Invoke($"    Current worry level is not divisible by {monkey.TestDivisor}.");
                        logger?.Invoke($"    Item with worry level {worryLevel} is thrown to monkey {monkey.IfFalseMonkeyId}.");
                        Monkeys[monkey.IfFalseMonkeyId].Items.Add(worryLevel);
                    }

                    // remove the item from this monkey and fix the item index.
                    monkey.Items.RemoveAt(itemIndex);
                    itemIndex--;
                }
            }
        }

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
                    Op = ops.Groups[nameof(MonkeyOp.Op)].Value,
                    Other = ops.Groups[nameof(MonkeyOp.Other)].Value,
                },
                TestDivisor = int.Parse(TestRegex.Match(input[3]).Groups["divisor"].ValueSpan),
                IfTrueMonkeyId = int.Parse(IfTrueRegex.Match(input[4]).Groups["monkey"].ValueSpan),
                IfFalseMonkeyId = int.Parse(IfFalseRegex.Match(input[5]).Groups["monkey"].ValueSpan)
            };
            return result;
        }


        public static readonly Regex MonkeyIdRegex = new Regex(@"Monkey (?<Id>\d*):");
        public static readonly Regex StartingItemsRegex = new Regex(@"Starting items: (?<Items>.*)");
        public static readonly Regex OperationRegex = new Regex(@"Operation: new = old (?<Op>\+|\*) (?<Other>\S*)");
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
        public required string Op { get; init; }
        public required string Other { get; init; }
    }
}