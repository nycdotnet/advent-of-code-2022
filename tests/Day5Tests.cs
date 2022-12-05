using day4;
using day5;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace tests
{
    public class Day5Tests
    {
        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day5(example_input_1.Split('\n'));
            sut.GetAnswerForPart1().Should().Be("2");
        }

        public static readonly string example_input_1 = """
                [D]    
            [N] [C]    
            [Z] [M] [P]
             1   2   3 

            move 1 from 2 to 1
            move 3 from 1 to 3
            move 2 from 2 to 1
            move 1 from 1 to 2
            """.ReplaceLineEndings("\n");
    }
}
