using common;
using System.Text.RegularExpressions;

namespace day04
{
    public class Day04 : IAdventOfCodeDay
    {
        public Day04(IEnumerable<string> input)
        {
            ElfPairs = input
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new ElfPair(x))
                .ToList();
        }

        public List<ElfPair> ElfPairs { get; }

        public string GetAnswerForPart1()
        {
            var result = 0;

            foreach (var pair in ElfPairs)
            {
                if (pair.FirstContainsAllOfSecond || pair.SecondContainsAllOfFirst) {
                    result++;
                }
            }

            return result.ToString();
        }

        public string GetAnswerForPart2()
        {
            var result = 0;

            foreach (var pair in ElfPairs)
            {
                if (ElfPair.Overlaps(pair.First, pair.Second))
                {
                    result++;
                }
            }

            return result.ToString();
        }

    }

    public record ElfPair
    {
        public ElfPair(string input)
        {
            var match = ElfPairRegex.Match(input);

            FirstStart = int.Parse(match.Groups[nameof(FirstStart)].ValueSpan);
            FirstEnd = int.Parse(match.Groups[nameof(FirstEnd)].ValueSpan);
            SecondStart = int.Parse(match.Groups[nameof(SecondStart)].ValueSpan);
            SecondEnd = int.Parse(match.Groups[nameof(SecondEnd)].ValueSpan);
        }

        public static readonly Regex ElfPairRegex = new(@"^(?<FirstStart>\d*)-(?<FirstEnd>\d*),(?<SecondStart>\d*)-(?<SecondEnd>\d*)$");

        public int FirstStart { get; }
        public int FirstEnd { get; }
        public int SecondStart { get; }
        public int SecondEnd { get; }

        public bool FirstContainsAllOfSecond =>
            FirstStart <= SecondStart && FirstEnd >= SecondEnd;
        public bool SecondContainsAllOfFirst =>
            SecondStart <= FirstStart && SecondEnd >= FirstEnd;

        public (int, int) First => (FirstStart, FirstEnd);
        public (int, int) Second => (SecondStart, SecondEnd);

        public static bool Overlaps((int start, int end) a, (int start, int end) b)
        {
            // if either BOTH starts and ends after the other ends, then false.
            if (b.start > a.end && b.end > a.end) return false;
            if (a.start > b.end && a.end > b.end) return false;

            return true;
        }
    }
}