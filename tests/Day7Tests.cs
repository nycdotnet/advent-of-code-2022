using common;
using day7;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class Day7Tests
    {
        [Fact]
        public void ParsingWorksAsDocumented()
        {
            var sut = new Day7(example_input_1.ReplaceLineEndings("\n").Split('\n'));
            sut.RootElfFolder.ToString().Should().Be(example_directory_listing_1);
        }

        [Fact]
        public void AllSubFoldersProducesExpectedResult()
        {
            var sut = new Day7(example_input_1.ReplaceLineEndings("\n").Split('\n'));
            var result = sut.RootElfFolder.AllSubFolders().ToArray();
            result.Select(f => f.Name).Should().Equal(new[] { "a", "e", "d" });
        }

        [Theory]
        [InlineData("14848514 b.txt", 14848514, "b.txt")]
        [InlineData("1234 file name with some spaces", 1234, "file name with some spaces")]
        public void FileRegexWorksAsExpected(string fileSpec, int expectedSize, string expectedFileName)
        {
            var sut = ElfFolder.FileRegex.Match(fileSpec);
            int.Parse(sut.Groups["Size"].ValueSpan).Should().Be(expectedSize);
            sut.Groups["FileName"].Value.Should().Be(expectedFileName);
        }

        [Fact]
        public void Part1WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day7(example_input_1.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("95437");
        }

        [Fact]
        public void Part1WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day7>("day7.input.txt");
            var sut = new Day7(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart1().Should().Be("1648397");
        }

        [Fact]
        public void Part2WithSampleInputProducesDocumentedResult()
        {
            var sut = new Day7(example_input_1.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("24933642");
        }

        [Fact]
        public void Part2WithActualInputProducesCorrectResult()
        {
            var input = Utils.GetResourceStringFromAssembly<Day7>("day7.input.txt");
            var sut = new Day7(input.ReplaceLineEndings("\n").Split('\n'));
            sut.GetAnswerForPart2().Should().Be("1815525");
        }

        public static readonly string example_input_1 = """
            $ cd /
            $ ls
            dir a
            14848514 b.txt
            8504156 c.dat
            dir d
            $ cd a
            $ ls
            dir e
            29116 f
            2557 g
            62596 h.lst
            $ cd e
            $ ls
            584 i
            $ cd ..
            $ cd ..
            $ cd d
            $ ls
            4060174 j
            8033020 d.log
            5626152 d.ext
            7214296 k
            """.ReplaceLineEndings("\n");

        public static readonly string example_directory_listing_1 = """
            - / (dir)
              - a (dir)
                - e (dir)
                  - i (file, size=584)
                - f (file, size=29116)
                - g (file, size=2557)
                - h.lst (file, size=62596)
              - b.txt (file, size=14848514)
              - c.dat (file, size=8504156)
              - d (dir)
                - j (file, size=4060174)
                - d.log (file, size=8033020)
                - d.ext (file, size=5626152)
                - k (file, size=7214296)
            """;
    }
}
