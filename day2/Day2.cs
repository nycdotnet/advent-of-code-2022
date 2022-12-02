using common;

namespace day2
{
    public class Day2 : IAdventOfCodeDay
    {
        public Day2(IEnumerable<string> input)
        {
            Strategies = ParseStrategies(input);
        }

        public List<Strategy> Strategies { get; }

        private static List<Strategy> ParseStrategies(IEnumerable<string> input)
        {
            var result = new List<Strategy>();
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                
                result.Add(Strategy.Parse(line));
            }
            return result;
        }

        public string GetAnswerForPart1()
        {
            return Strategies.Sum(s => s.Part1ScoreResult).ToString();
        }

        public string GetAnswerForPart2()
        {
            return Strategies.Sum(s => s.Part2ScoreResult).ToString();
        }
    }
}