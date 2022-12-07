using common;
using day1;
using day2;
using day3;
using day4;
using day5;
using day6;
using day7;
using System.Diagnostics;

namespace advent_of_code_2022
{
    internal class Program
    {
        static void Main(string[] _)
        {
            var ticks = 0L;
            ticks += GetTicksAndReport(Day1);
            ticks += GetTicksAndReport(Day2);
            ticks += GetTicksAndReport(Day3);
            ticks += GetTicksAndReport(Day4);
            ticks += GetTicksAndReport(Day5);
            ticks += GetTicksAndReport(Day6);
            ticks += GetTicksAndReport(Day7);

            Console.WriteLine($"Total time elapsed: {new TimeSpan(ticks).TotalMilliseconds}ms");
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

        private static void Day3()
        {
            var input = Utils.GetResourceStringFromAssembly<Day3>("day3.input.txt");
            var day = new Day3(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day4()
        {
            var input = Utils.GetResourceStringFromAssembly<Day4>("day4.input.txt");
            var day = new Day4(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day5()
        {
            var input = Utils.GetResourceStringFromAssembly<Day5>("day5.input.txt");
            var day = new Day5(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            day = new Day5(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day6()
        {
            var input = Utils.GetResourceStringFromAssembly<Day6>("day6.input.txt");
            var day = new Day6(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day7()
        {
            var input = Utils.GetResourceStringFromAssembly<Day7>("day7.input.txt");
            var day = new Day7(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        /// <summary>
        /// NOTE: Real benchmarks should use Benchmark.NET - this is just for relative info on the console
        /// </summary>
        private static long GetTicksAndReport(Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action.Invoke();
            sw.Stop();
            Console.WriteLine($"Completed action in {sw.ElapsedMilliseconds}ms");
            return sw.ElapsedTicks;
        }
    }
}
