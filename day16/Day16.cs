using System.Text.RegularExpressions;

namespace day16
{
    public class Day16
    {
        public Day16(IEnumerable<string> input)
        {
            Valves = input.Select(Valve.MaybeParse).Where(v => v != null).ToList()!;

        }

        public List<Valve> Valves { get; }
    }

    public sealed partial record Valve
    {
        public required string Code { get; init; }
        public required int FlowRate { get; init; }
        public required HashSet<string> LeadsTo { get; init; }

        public static Valve? MaybeParse(string s)
        {
            var match = ValveRegex().Match(s);
            if (!match.Success)
            {
                return null;
            }

            var result = new Valve
            {
                Code = match.Groups[nameof(Code)].Value,
                FlowRate = int.Parse(match.Groups[nameof(FlowRate)].ValueSpan),
                LeadsTo = match.Groups[nameof(LeadsTo)].Value.Split(", ").ToHashSet()
            };

            return result;
        }

        [GeneratedRegex(@"^Valve (?<Code>\S\S) has flow rate=(?<FlowRate>\d*); tunnels? leads? to valves? (?<LeadsTo>.*)$")]
        private static partial Regex ValveRegex();
    }
}