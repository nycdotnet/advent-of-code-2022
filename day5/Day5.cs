using common;

namespace day5
{
    public class Day5 : IAdventOfCodeDay
    {
        public Day5(IEnumerable<string> input)
        {
            var allInput = input.ToList();
            var firstBlankIndex = allInput.FindIndex(string.IsNullOrEmpty);
            var crates = Crates.Parse(allInput.Take(firstBlankIndex));
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
}