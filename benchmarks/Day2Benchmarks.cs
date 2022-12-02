using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using common;
using day2;

namespace benchmarks
{
    //[ShortRunJob]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    public class Day2Benchmarks
    {
        public Day2Benchmarks()
        {
            var input = Utils.GetResourceStringFromAssembly<Day2>("day2.input.txt");
            Day = new Day2(input.ReplaceLineEndings("\n").Split('\n'));
        }

        [Benchmark]
        public int GetResultBenchmark()
        {
            return Day.Strategies.Sum(s => s.Part1ScoreResult);
        }

        [Benchmark]
        public int GetResultBenchmarkAlt()
        {
            return Day.Strategies.Sum(s => s.Part1ScoreResultAlt);
        }

        [Benchmark]
        public int GetResultBenchmarkAlt2()
        {
            return Day.Strategies.Sum(s => s.Part1ScoreResultAlt2);
        }

        public Day2 Day { get; }
    }
}
