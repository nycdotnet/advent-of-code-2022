using common;

namespace day8
{
    public class Day8 : IAdventOfCodeDay
    {
        public Day8(IEnumerable<string> input)
        {
            Data = input.Select(x => x.ToCharArray()).ToArray();
        }

        public char[][] Data { get; set; }

        public string GetAnswerForPart1()
        {
            throw new NotImplementedException();
        }

        public string GetAnswerForPart2()
        {
            throw new NotImplementedException();
        }
    }
}