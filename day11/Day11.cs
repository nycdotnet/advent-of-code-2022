using common;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

namespace day11
{
    public class Day11 : IAdventOfCodeDay
    {
        public Day11(IEnumerable<string> input, Func<BigInteger, BigInteger>? worryFactorAdjustment = null)
        {
            Monkeys = input
                .SelectPartition(string.IsNullOrEmpty)
                .Select(Monkey.Parse)
                .ToList();

            ReasonablenessNumber = CalculateReasonableness(Monkeys);

            if (worryFactorAdjustment is null)
            {
                worryFactorAdjustment = wfa => wfa / 3;
            }
            this.worryFactorAdjustment = worryFactorAdjustment;
        }

        public static int CalculateReasonableness(IEnumerable<Monkey> monkeys)
        {
            var number = 1;
            foreach(var monkey in monkeys)
            {
                number *= monkey.TestDivisor;
            }
            return number;
        }

        public List<Monkey> Monkeys { get; }
        public int ReasonablenessNumber { get; }

        private readonly Func<BigInteger, BigInteger> worryFactorAdjustment;

        public void SimulateRound(Action<string>? logger = null, Action<(int monkeyIndex, BigInteger itemWorryLevel)>? monkeyInspectsItem = null)
        {
            for (var monkeyIndex = 0; monkeyIndex < Monkeys.Count; monkeyIndex++)
            {
                var monkey = Monkeys[monkeyIndex];
                logger?.Invoke($"Monkey {monkeyIndex}:");

                for (var itemIndex = 0; itemIndex < monkey.Items.Count; itemIndex++)
                {
                    var worryLevel = monkey.Items[itemIndex];
                    logger?.Invoke($"  Monkey inspects an item with a worry level of {worryLevel}.");
                    monkeyInspectsItem?.Invoke((monkeyIndex, worryLevel));

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

                    worryLevel = worryFactorAdjustment(worryLevel) % ReasonablenessNumber;

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
            if (worryFactorAdjustment(9) != 3)
            {
                throw new NotSupportedException("Unexpected worry factor adjustment for part 1.  Expected divide by 3.");
            }

            var monkeyInspections = GetMonkeyInspections(20);

            var counts = monkeyInspections
                .Values
                .OrderByDescending(v => v)
                .Take(2)
                .ToArray();

            return (counts[0] * counts[1]).ToString();
        }

        public string GetAnswerForPart2()
        {
            const int roundsToSimulate = 10_000;
            if (worryFactorAdjustment(9) != 9)
            {
                throw new NotSupportedException("Unexpected worry factor adjustment for part 2.  Expected identity function.");
            }

            var monkeyInspections = Monkeys.ToDictionary(m => m.Id, _ => BigInteger.Zero);

            for (var i = 0; i < roundsToSimulate; i++)
            {
                SimulateRound(monkeyInspectsItem: m => monkeyInspections[m.monkeyIndex]++);
            }

            var counts = monkeyInspections
                .Values
                .OrderByDescending(v => v)
                .Take(2)
                .ToArray();

            return (counts[0] * counts[1]).ToString();
        }

        public Dictionary<int, BigInteger> GetMonkeyInspections(int roundsToSimulate)
        {
            var monkeyInspections = Monkeys.ToDictionary(m => m.Id, _ => BigInteger.Zero);

            for (var i = 0; i < roundsToSimulate; i++)
            {
                SimulateRound(monkeyInspectsItem: m => monkeyInspections[m.monkeyIndex]++);
            }
            return monkeyInspections;
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
                    .Select(BigInteger.Parse)
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
        public required List<BigInteger> Items { get; init; }
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