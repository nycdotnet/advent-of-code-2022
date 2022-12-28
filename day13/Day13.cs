using common;
using OneOf;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace day13
{
    public class Day13 : IAdventOfCodeDay
    {
        public Day13(IEnumerable<string> input)
        {
            Pairs = input
                .SelectPartition(string.IsNullOrEmpty)
                .Where(x => x.Count > 0)
                .ToList();
        }

        public static AnalysisResult AnalyzePackets(string left, string right)
        {
            var leftPacket = new Day13Packet { PacketData = left };
            var rightPacket = new Day13Packet { PacketData = right };

            var result = leftPacket.CompareTo(rightPacket);

            return new AnalysisResult(AreInRightOrder: result < 0);
        }

        public record AnalysisResult(bool AreInRightOrder);

        public List<List<string>> Pairs { get; }

        public string GetAnswerForPart1()
        {
            var sum = 0;
            for (var i = 0; i < Pairs.Count; i++)
            {
                var analysis = AnalyzePackets(Pairs[i][0], Pairs[i][1]);
                if (analysis.AreInRightOrder)
                {
                    sum += i + 1; // it's a one-based index
                }
            }
            return sum.ToString();
        }

        public string GetAnswerForPart2()
        {
            var dividerTwo = new Day13Packet { PacketData = "[[2]]" };
            var dividerSix = new Day13Packet { PacketData = "[[6]]" };
            var packets = new List<Day13Packet> {
                dividerTwo,
                dividerSix
            };
            packets.AddRange(
                Pairs
                .SelectMany(p => p)
                .Select(p => new Day13Packet { PacketData = p }));

            packets.Sort();

            var indexOfDividerTwo = packets.IndexOf(dividerTwo) + 1; // one based index
            var indexOfDividerSix = packets.IndexOf(dividerSix) + 1; // one based index

            return (indexOfDividerTwo * indexOfDividerSix).ToString();
        }
    }

    public record Day13Packet : IComparable<Day13Packet>
    {
        public required string PacketData { get; init; }

        public int CompareTo(Day13Packet? other)
        {
            if (other is null)
            {
                // Would return 1 here to indicate that nulls go first, or -1 for nulls go last.
                throw new ArgumentNullException(paramName: nameof(other), message: $"It is not supported to compare to a null {nameof(Day13Packet)}.");
            }

            return CompareValues(new ArrayOrNumber(JsonNode.Parse(PacketData)), new ArrayOrNumber(JsonNode.Parse(other.PacketData)));
        }

        private static int CompareValues(
            ArrayOrNumber left,
            ArrayOrNumber right)
        {
            var leftIsArray = left.TryPickT0(out var leftArray, out var leftValue);
            var rightIsArray = right.TryPickT0(out var rightArray, out var rightValue);

            return (leftIsArray, rightIsArray) switch
            {
                (true, true) => CompareArrays(leftArray, rightArray),
                (true, false) => CompareValues(
                    new ArrayOrNumber(leftArray),
                    new ArrayOrNumber(JsonNode.Parse($"[{rightValue.GetValue<int>()}]"))),
                (false, true) => CompareValues(
                    new ArrayOrNumber(JsonNode.Parse($"[{leftValue.GetValue<int>()}]")),
                    new ArrayOrNumber(rightArray)),
                (false, false) => leftValue.GetValue<int>().CompareTo(rightValue.GetValue<int>())
            };
        }

        static int CompareArrays(JsonArray left, JsonArray right)
        {
            var i = 0;
            while (true)
            {
                if (left.Count > i && right.Count > i)
                {
                    var comparison = CompareValues(new ArrayOrNumber(left[i]), new ArrayOrNumber(right[i]));
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }
                else if (left.Count == right.Count)
                {
                    return 0;
                }
                else if (left.Count > right.Count)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
                i++;
            }
        }
    }

    public class ArrayOrNumber : OneOfBase<JsonArray, JsonValue>
    {
        /// <summary>
        /// Creates an <see cref="ArrayOrNumber"/> from the <paramref name="input"/>.  The input
        /// must be castable to a <see cref="JsonArray" /> or a <see cref="JsonValue"/> or else
        /// this will throw.
        /// </summary>
        public ArrayOrNumber(JsonNode? input) : base(Wrap(input))
        {
        }

        public static OneOf<JsonArray, JsonValue> Wrap(JsonNode? input)
        {
            ArgumentNullException.ThrowIfNull(input, paramName: nameof(input));

            return input switch
            {
                JsonArray a => a,
                JsonValue v => v,
                _ => throw new InvalidCastException($"Expected {nameof(input)} to be a {nameof(JsonArray)} or {nameof(JsonValue)}, but it is a {input.GetType()}.")
            };
        }
    }
}