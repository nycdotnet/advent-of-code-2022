using common;
using day10;
using FluentAssertions;
using Xunit;
using static day10.Instruction;

namespace tests
{
    public class Day10Tests
    {
        [Fact]
        public void ParsingWorks()
        {
            var sut = new Day10(TrivialProgram.Split('\n'));
            sut.Instructions.Should().HaveCount(3);
            sut.Instructions[0].Kind.Should().Be(InstructionKind.Noop);
            sut.Instructions[1].Kind.Should().Be(InstructionKind.Addx);
            sut.Instructions[1].Magnitude.Should().Be(3);
            sut.Instructions[2].Kind.Should().Be(InstructionKind.Addx);
            sut.Instructions[2].Magnitude.Should().Be(-5);
        }

        [Fact]
        public void SimulateTrivialWorks()
        {
            var debugOutput = new List<string>();
            var sut = new Day10(TrivialProgram.Split('\n'));
            sut.Simulate(onCycle: (instructionIndex, instructionClock, instruction, cpu) =>
            {
                debugOutput.Add($"Instruction {instruction.Kind}({instruction.Magnitude}) at index {instructionIndex} is executing. CPU State: {cpu}.");
            },
            afterInstruction: (instructionIndex, instruction, cpu) =>
            {
                debugOutput.Add($"Instruction {instruction.Kind}({instruction.Magnitude}) at index {instructionIndex} is now complete. CPU State: {cpu}.");
            });

            debugOutput.Count.Should().Be(8);
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day10(LongerExampleProgram.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("13140");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day10>("day10.input.txt");
            var sut = new Day10(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("13220");
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day10(LongerExampleProgram.Split('\n'));
            sut.GetAnswerForPart2().Should().Be(Part2LongerExampleDocumentedResult);
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day10>("day10.input.txt");
            var sut = new Day10(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be(CorrectPart2Answer);
        }

        public static readonly string Part2LongerExampleDocumentedResult = """
            ##..##..##..##..##..##..##..##..##..##..
            ###...###...###...###...###...###...###.
            ####....####....####....####....####....
            #####.....#####.....#####.....#####.....
            ######......######......######......####
            #######.......#######.......#######.....
            """.ReplaceLineEndings("\n");

        public static readonly string CorrectPart2Answer = """
            ###..#..#..##..#..#.#..#.###..####.#..#.
            #..#.#..#.#..#.#.#..#..#.#..#.#....#.#..
            #..#.#..#.#..#.##...####.###..###..##...
            ###..#..#.####.#.#..#..#.#..#.#....#.#..
            #.#..#..#.#..#.#.#..#..#.#..#.#....#.#..
            #..#..##..#..#.#..#.#..#.###..####.#..#.
            """.ReplaceLineEndings("\n");

        public static readonly string TrivialProgram = """
            noop
            addx 3
            addx -5
            """.ReplaceLineEndings("\n");

        public static readonly string LongerExampleProgram = """
            addx 15
            addx -11
            addx 6
            addx -3
            addx 5
            addx -1
            addx -8
            addx 13
            addx 4
            noop
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx -35
            addx 1
            addx 24
            addx -19
            addx 1
            addx 16
            addx -11
            noop
            noop
            addx 21
            addx -15
            noop
            noop
            addx -3
            addx 9
            addx 1
            addx -3
            addx 8
            addx 1
            addx 5
            noop
            noop
            noop
            noop
            noop
            addx -36
            noop
            addx 1
            addx 7
            noop
            noop
            noop
            addx 2
            addx 6
            noop
            noop
            noop
            noop
            noop
            addx 1
            noop
            noop
            addx 7
            addx 1
            noop
            addx -13
            addx 13
            addx 7
            noop
            addx 1
            addx -33
            noop
            noop
            noop
            addx 2
            noop
            noop
            noop
            addx 8
            noop
            addx -1
            addx 2
            addx 1
            noop
            addx 17
            addx -9
            addx 1
            addx 1
            addx -3
            addx 11
            noop
            noop
            addx 1
            noop
            addx 1
            noop
            noop
            addx -13
            addx -19
            addx 1
            addx 3
            addx 26
            addx -30
            addx 12
            addx -1
            addx 3
            addx 1
            noop
            noop
            noop
            addx -9
            addx 18
            addx 1
            addx 2
            noop
            noop
            addx 9
            noop
            noop
            noop
            addx -1
            addx 2
            addx -37
            addx 1
            addx 3
            noop
            addx 15
            addx -21
            addx 22
            addx -6
            addx 1
            noop
            addx 2
            addx 1
            noop
            addx -10
            noop
            noop
            addx 20
            addx 1
            addx 2
            addx 2
            addx -6
            addx -11
            noop
            noop
            noop
            """.ReplaceLineEndings("\n");
    }
}
