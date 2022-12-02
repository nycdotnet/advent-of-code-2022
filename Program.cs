using common;
using day1;

namespace advent_of_code_2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                var input = Utils.GetResourceStringFromAssembly<Day1>("day1.input.txt");
                var day1 = new Day1(input.ReplaceLineEndings("\n").Split('\n'));
                Console.WriteLine($"Day 1 answer 1: {day1.GetAnswerForPart1()}");
                Console.WriteLine($"Day 1 answer 2: {day1.GetAnswerForPart2()}");
            }
        }
    }
}