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
        public const int LENGTH_OF_START_OF_MESSAGE_MARKER = 14;

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

        public int StartOfMessageMarkerIndex()
        {
            var buffer = DataStreamBuffer.AsSpan();
            var set = new HashSet<char>(LENGTH_OF_START_OF_MESSAGE_MARKER);
            for (var i = 0; i < DataStreamBuffer.Length - LENGTH_OF_START_OF_MESSAGE_MARKER; i++)
            {
                set.Clear();
                for (var s = 0; s < LENGTH_OF_START_OF_MESSAGE_MARKER; s++)
                {
                    set.Add(buffer[i + s]);
                }

                if (set.Count == LENGTH_OF_START_OF_MESSAGE_MARKER)
                {
                    return i + LENGTH_OF_START_OF_MESSAGE_MARKER;
                }

                // we can cheat a bit here because if there's (for example) two dups
                // which means set.Count will be 12, we know we can move at least
                // 1 extra.  I am sure there's a more efficient algorithm though which
                // would involve bailing early when adding if a dup is detected.
                i += LENGTH_OF_START_OF_MESSAGE_MARKER - set.Count - 1;
            }
            return -1;
        }

        public string GetAnswerForPart1()
        {
            return StartOfPacketMarkerIndex().ToString();
        }

        public string GetAnswerForPart2()
        {
            return StartOfMessageMarkerIndex().ToString();
        }
    }
}