using common;

namespace day1
{
    public class Day1 : IAdventOfCodeDay
    {
        public Day1(IEnumerable<string> input)
        {
            elves = ParseElves(input);
        }

        public static List<Elf> ParseElves(IEnumerable<string> input)
        {
            var result = new List<Elf>();
            var elf = new Elf();
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    MaybeAddElf();
                    elf = new();
                    continue;
                }
                elf.SnacksByCalories.Add(int.Parse(line));
            }

            MaybeAddElf();

            return result;

            void MaybeAddElf()
            {
                if (elf.SnacksByCalories.Any())
                {
                    result.Add(elf);
                }
            }
        }

        public List<Elf> elves { get; } = new();

        public string GetAnswerForPart1()
        {
            var mostCaloriesElf = elves.MaxBy(e => e.TotalCalories);
            return mostCaloriesElf?.TotalCalories.ToString() ?? "No Elves";
        }

        public string GetAnswerForPart2()
        {
            var topThree = elves
                .OrderByDescending(e => e.TotalCalories)
                .Take(3)
                .ToArray();

            if (topThree.Length < 3)
            {
                return "Not at least three elves.";
            }

            return topThree.Sum(e => e.TotalCalories).ToString();
        }

        public record Elf
        {
            public List<int> SnacksByCalories { get; init; } = new();
            public int TotalCalories => SnacksByCalories.Sum();
        }
    }
}