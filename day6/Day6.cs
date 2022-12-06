using common;

namespace day6
{
    public class Day6 : IAdventOfCodeDay
    {
        public string DataStreamBuffer { get; }

        public Day6(IEnumerable<string> input)
        {
            DataStreamBuffer = input.Where(s => !string.IsNullOrEmpty(s)).Single();
        }

        public const int LENGTH_OF_START_OF_PACKET_MARKER = 4;

        public int StartOfPacketMarkerIndex()
        {
            var buffer = DataStreamBuffer.AsSpan();
            for (var i = 0; i < DataStreamBuffer.Length - LENGTH_OF_START_OF_PACKET_MARKER; i++)
            {
                if (buffer[i + 0] == buffer[i + 1] || buffer[i + 0] == buffer[i + 2] || buffer[i + 0] == buffer[i + 3] ||
                    buffer[i + 1] == buffer[i + 2] || buffer[i + 1] == buffer[i + 3] ||
                    buffer[i + 2] == buffer[i + 3])
                {
                    continue;
                }
                return i + LENGTH_OF_START_OF_PACKET_MARKER;
            }
            return -1;
        }

        public string GetAnswerForPart1()
        {
            return StartOfPacketMarkerIndex().ToString();
        }

        public string GetAnswerForPart2()
        {
            throw new NotImplementedException();
        }
    }
}