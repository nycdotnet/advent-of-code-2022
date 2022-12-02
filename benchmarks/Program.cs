using BenchmarkDotNet.Running;

namespace benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Day2Benchmarks>();
        }
    }
}