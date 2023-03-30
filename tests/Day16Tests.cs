using day16;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day16Tests
    {
        [Fact]
        public void ParsingWorksAsExpected()
        {
            var sut = new Day16(exampleInput1.Split('\n'));
            sut.Valves.Should().HaveCount(10);

            // spot check
            var dd = sut.Valves.Single(v => v.Code == "DD");
            dd.FlowRate.Should().Be(20);
            dd.LeadsTo.Should().HaveCount(3);
            dd.LeadsTo.Should().Contain("CC");
            dd.LeadsTo.Should().Contain("AA");
            dd.LeadsTo.Should().Contain("EE");
        }

        public static readonly string exampleInput1 = """
            Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
            Valve BB has flow rate=13; tunnels lead to valves CC, AA
            Valve CC has flow rate=2; tunnels lead to valves DD, BB
            Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
            Valve EE has flow rate=3; tunnels lead to valves FF, DD
            Valve FF has flow rate=0; tunnels lead to valves EE, GG
            Valve GG has flow rate=0; tunnels lead to valves FF, HH
            Valve HH has flow rate=22; tunnel leads to valve GG
            Valve II has flow rate=0; tunnels lead to valves AA, JJ
            Valve JJ has flow rate=21; tunnel leads to valve II
            """.ReplaceLineEndings("\n");

    }
}
