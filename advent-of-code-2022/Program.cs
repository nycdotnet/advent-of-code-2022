﻿using common;
using day01;
using day10;
using day02;
using day03;
using day04;
using day05;
using day06;
using day07;
using day08;
using day09;
using System.Diagnostics;
using day11;
using day12;
using day13;
using day14;
using day15;

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
            ticks += GetTicksAndReport(Day8);
            ticks += GetTicksAndReport(Day9);
            ticks += GetTicksAndReport(Day10);
            ticks += GetTicksAndReport(Day11);
            ticks += GetTicksAndReport(Day12);
            ticks += GetTicksAndReport(Day13);
            ticks += GetTicksAndReport(Day14);
            ticks += GetTicksAndReport(Day15);

            Console.WriteLine($"Total time elapsed: {new TimeSpan(ticks).TotalMilliseconds}ms");
        }

        private static void Day1()
        {
            var input = Utils.GetResourceStringFromAssembly<Day01>("day01.input.txt");
            var day = new Day01(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day2()
        {
            var input = Utils.GetResourceStringFromAssembly<Day02>("day02.input.txt");
            var day = new Day02(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day3()
        {
            var input = Utils.GetResourceStringFromAssembly<Day03>("day03.input.txt");
            var day = new Day03(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day4()
        {
            var input = Utils.GetResourceStringFromAssembly<Day04>("day04.input.txt");
            var day = new Day04(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day5()
        {
            var input = Utils.GetResourceStringFromAssembly<Day05>("day05.input.txt");
            var day = new Day05(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            day = new Day05(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day6()
        {
            var input = Utils.GetResourceStringFromAssembly<Day06>("day06.input.txt");
            var day = new Day06(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day7()
        {
            var input = Utils.GetResourceStringFromAssembly<Day07>("day07.input.txt");
            var day = new Day07(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day8()
        {
            var input = Utils.GetResourceStringFromAssembly<Day08>("day08.input.txt");
            var day = new Day08(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day9()
        {
            var input = Utils.GetResourceStringFromAssembly<Day09>("day09.input.txt");
            var day = new Day09(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day10()
        {
            var input = Utils.GetResourceStringFromAssembly<Day10>("day10.input.txt");
            var day = new Day10(input.ReplaceLineEndings("\n").Split('\n'));
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2:\n{day.GetAnswerForPart2()}");
        }

        private static void Day11()
        {
            var input = Utils.GetResourceStringFromAssembly<Day11>("day11.input.txt")
                .ReplaceLineEndings("\n").Split('\n');
            var day = new Day11(input);
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            day = new Day11(input, wfa => wfa);
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day12()
        {
            var input = Utils.GetResourceStringFromAssembly<Day12>("day12.input.txt")
                .ReplaceLineEndings("\n").Split('\n');
            var day = new Day12(input);
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            day = new Day12(input);
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day13()
        {
            var input = Utils.GetResourceStringFromAssembly<Day13>("day13.input.txt")
                .ReplaceLineEndings("\n").Split('\n');
            var day = new Day13(input);
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day14()
        {
            var input = Utils.GetResourceStringFromAssembly<Day14>("day14.input.txt")
                .ReplaceLineEndings("\n").Split('\n');
            var day = new Day14(input);
            Console.WriteLine($"{day.GetType().Name} answer 1: {day.GetAnswerForPart1()}");
            Console.WriteLine($"{day.GetType().Name} answer 2: {day.GetAnswerForPart2()}");
        }

        private static void Day15()
        {
            var input = Utils.GetResourceStringFromAssembly<Day15>("day15.input.txt")
                .ReplaceLineEndings("\n").Split('\n');

            var day = new Day15(input);
            day.RowSearchYOffset = 2000000;
            day.BeaconScanMaximumCoordinate = 4_000_000;
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
