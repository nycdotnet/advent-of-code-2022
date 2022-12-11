using common;

namespace day10
{
    public class Day10 : IAdventOfCodeDay
    {
        public Day10(IEnumerable<string> input)
        {
            Instructions = input.Select(Instruction.Parse).ToList();
        }

        public List<Instruction> Instructions { get; }

        public void Simulate(int instructionCount = -1, Action<int,int,Instruction,CpuState>? duringInstruction = null, Action<int,Instruction,CpuState>? afterInstruction = null)
        {
            if (instructionCount == -1)
            {
                instructionCount = Instructions.Count;
            }
            var cpu = new CpuState();

            for (var instructionIndex = 0; instructionIndex < instructionCount; instructionIndex++)
            {
                var currentInstruction = Instructions[instructionIndex];

                if (!InstructionClockCounts.TryGetValue(currentInstruction.Kind, out var instructionClockCycles))
                {
                    ThrowNotSupportedInstruction(currentInstruction.Kind);
                }

                for (var instructionClock = 0; instructionClock < instructionClockCycles; instructionClock++)
                {
                    cpu.NextCycle();
                    duringInstruction?.Invoke(instructionIndex, instructionClock, currentInstruction, cpu);
                    switch (currentInstruction.Kind)
                    {
                        case Instruction.InstructionKind.Noop:
                            break;
                        case Instruction.InstructionKind.Addx:
                            if (instructionClock == instructionClockCycles -1)
                            {
                                cpu.X += currentInstruction.Magnitude;
                            }
                            break;
                        default:
                            ThrowNotSupportedInstruction(currentInstruction.Kind);
                            break;
                    }
                }
                
                afterInstruction?.Invoke(instructionIndex, currentInstruction, cpu);
            }

            static void ThrowNotSupportedInstruction(Instruction.InstructionKind kind)
            {
                throw new NotSupportedException($"The instruction kind {kind} is not supported.");
            }
        }

        public static readonly Dictionary<Instruction.InstructionKind, int> InstructionClockCounts = new()
        {
            { Instruction.InstructionKind.Noop, 1 },
            { Instruction.InstructionKind.Addx, 2 }
        };

        public string GetAnswerForPart1()
        {
            var signalStrengths = new List<int>();
            Simulate(duringInstruction: (instructionIndex, instructionClock, instruction, cpu) =>
            {
                if ((cpu.Cycle - 20) % 40 == 0)
                {
                    signalStrengths.Add(cpu.Cycle * cpu.X);
                }
            });
            return signalStrengths.Sum().ToString();
        }

        public string GetAnswerForPart2()
        {
            throw new NotImplementedException();
        }
    }

    public record CpuState
    {
        public int X { get; set; } = 1;
        /// <summary>
        /// A one-based cycle index.  The first cycle is 1.
        /// </summary>
        public int Cycle { get; private set; } = 0;
        public void NextCycle()
        {
            Cycle++;
        }
    }

    public record Instruction
    {
        public InstructionKind Kind { get; private set; }
        public int Magnitude {  get ; private set; }

        public static Instruction Parse(string instruction)
        {
            var result = new Instruction();
            if (instruction == "noop")
            {
                result.Kind = InstructionKind.Noop;
            }
            else if (instruction.StartsWith("addx "))
            {
                result.Kind = InstructionKind.Addx;
                result.Magnitude = int.Parse(instruction[5..].AsSpan());
            }
            else
            {
                throw new NotSupportedException($"Instruction {instruction} is not supported.");
            }

            return result;
        }

        public enum InstructionKind
        {
            Noop,
            Addx
        }
    }
}