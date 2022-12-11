using common;
using System.Text.RegularExpressions;

namespace day05
{
    public class Day05 : IAdventOfCodeDay
    {
        public Day05(IEnumerable<string> input)
        {
            var allInput = input.ToList();
            var firstBlankIndex = allInput.FindIndex(string.IsNullOrEmpty);
            Crates = Crates.Parse(allInput.Take(firstBlankIndex));
            StackMoves = allInput
                .Skip(firstBlankIndex + 1)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(StackMove.Parse)
                .ToList();
        }

        public Crates Crates { get; private init; }
        public List<StackMove> StackMoves { get; private init; }

        public string GetAnswerForPart1()
        {
            for (var i = 0; i < StackMoves.Count; i++)
            {
                var currentOp = StackMoves[i];
                var origin = Crates.CrateStacks[currentOp.OriginStack - 1];
                var destination = Crates.CrateStacks[currentOp.DestinationStack - 1];
                for (var subOpIndex = 0; subOpIndex < currentOp.Count; subOpIndex++)
                {
                    var item = origin.Stack.Pop();
                    destination.Stack.Push(item);
                }
            }
            return string.Join("", Crates.CrateStacks.Select(cs => cs.Stack.Peek()));
        }

        public string GetAnswerForPart2()
        {
            var items = new List<char>();
            for (var i = 0; i < StackMoves.Count; i++)
            {
                var currentOp = StackMoves[i];
                var origin = Crates.CrateStacks[currentOp.OriginStack - 1];
                var destination = Crates.CrateStacks[currentOp.DestinationStack - 1];
                items.Clear();

                for (var subOpIndex = 0; subOpIndex < currentOp.Count; subOpIndex++)
                {
                    items.Add(origin.Stack.Pop());
                }

                for (var subOpIndex = items.Count - 1; subOpIndex >= 0; subOpIndex--)
                {
                    destination.Stack.Push(items[subOpIndex]);
                }
            }
            return string.Join("", Crates.CrateStacks.Select(cs => cs.Stack.Peek()));
        }
    }

    public class Crates
    {
        public static Crates Parse(IEnumerable<string> input)
        {
            var stacks = input.ToArray();
            var maxWidth = stacks.Select(x => x.Length).Max();
            var result = new Crates();
            var nextStackIndex = 1;

            for (var col = 1; col < maxWidth; col += 4)
            {
                var cs = new CrateStack { Position = nextStackIndex++ };
                for (var i = stacks.Length - 1; i >= 0; i--)
                {
                    if (stacks[i].Length > col)
                    {
                        var id = stacks[i][col];

                        if (i == stacks.Length - 1)
                        {
                            // This is the stack ID position, so just confirm it.
                            if (int.Parse(new ReadOnlySpan<char>(id)) != cs.Position)
                            {
                                throw new ApplicationException($"Expected to find data for stack {cs.Position}, but found {id} instead.");
                            }
                        }
                        else if (id != ' ')
                        {
                            cs.Stack.Push(id);
                        }
                    }
                }
                result.AddCrateStack(cs);
            }
            return result;
        }

        public List<CrateStack> CrateStacks { get; init; } = new();

        private void AddCrateStack(CrateStack cs)
        {
            CrateStacks.Add(cs);
        }
    }

    public class CrateStack
    {
        public Stack<char> Stack { get; init; } = new();
        /// <summary>
        /// The one-based position of this stack of crates
        /// </summary>
        public required int Position { get; init; }
    }

    public record StackMove
    {
        public static StackMove Parse(string input)
        {
            var match = StackMoveRegex.Match(input);

            return new StackMove
            {
                Count = int.Parse(match.Groups[nameof(Count)].ValueSpan),
                OriginStack = int.Parse(match.Groups[nameof(OriginStack)].ValueSpan),
                DestinationStack = int.Parse(match.Groups[nameof(DestinationStack)].ValueSpan),
            };
        }

        public required int Count { get; init; }
        public required int OriginStack { get; init; }
        public required int DestinationStack { get; init; }

        public static readonly Regex StackMoveRegex = new(@"^move (?<Count>\d*) from (?<OriginStack>\d*) to (?<DestinationStack>\d*)$");
    }
}