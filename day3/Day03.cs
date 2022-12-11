using common;

namespace day03
{
    public class Day03 : IAdventOfCodeDay
    {
        public Day03(IEnumerable<string> input)
        {
            Rucksacks = input
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new Rucksack { Contents = x })
                .ToList();
        }

        public List<Rucksack> Rucksacks { get; init; }

        public string GetAnswerForPart1()
        {
            var result = 0;
            foreach (var r in Rucksacks)
            {
                var itemsInFirst = r.FirstCompartment.ToHashSet();
                var itemsInSecond = r.SecondCompartment.ToHashSet();
                var itemsInBoth = itemsInFirst.Intersect(itemsInSecond).ToArray();
                if (itemsInBoth.Length != 1) {
                    throw new ApplicationException("This problem assumes exactly 1 item in both");
                }
                result += Priority(itemsInBoth.Single());
            }
            return result.ToString();
        }

        public string GetAnswerForPart2()
        {
            var result = 0;
            for (var i = 0; i < Rucksacks.Count; i += 3)
            {
                var temp = Rucksacks[i].Contents.ToHashSet();
                temp = temp.Intersect(Rucksacks[i + 1].Contents).ToHashSet();
                temp = temp.Intersect(Rucksacks[i + 2].Contents).ToHashSet();

                if (temp.Count != 1)
                {
                    throw new ApplicationException($"This problem assumes exactly 1 shared item in each set of three (i = {i}).");
                }
                result += Priority(temp.Single());
            }
            return result.ToString();
        }

        public static int Priority(char item)
        {
            const int LETTERS_IN_THE_ALPHABET = 26;
            if (item >= 'a' && item <= 'z')
            {
                return item - 'a' + 1;
            }
            
            if (item >= 'A' && item <= 'Z')
            {
                return item - 'A' + 1 + LETTERS_IN_THE_ALPHABET;
            }

            throw new NotSupportedException($"Item '{item}' not supported.  Only A-Z or a-z.");
        }
    }

    public record Rucksack
    {
        public required string Contents { get; init; }
        public string FirstCompartment => Contents.Substring(0, Contents.Length / 2);
        public string SecondCompartment => Contents.Substring(Contents.Length / 2);
    }
}