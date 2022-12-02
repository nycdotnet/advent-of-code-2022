using common;
using day1;
using day2;

namespace advent_of_code_2022
{
    internal class Program
    {
        static void Main(string[] _)
        {
            Day1();
            Day2();
        }

        private static void Day1()
        {
            var input = Utils.GetResourceStringFromAssembly<Day1>("day1.input.txt");
            var day = new Day1(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day2()
        {
            var input = Utils.GetResourceStringFromAssembly<Day2>("day2.input.txt");
            var day = new Day2(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }
    }
}