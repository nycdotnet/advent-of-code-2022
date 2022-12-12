using common;
using FluentAssertions;
using Xunit;

namespace tests
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void SelectPartitionWithNoPartitionsReturnsSingleItem()
        {
            var items = new string[] { "a", "b", "c" };
            var result = items.SelectPartition(string.IsNullOrEmpty);

            result.Single()[0].Should().Be("a");
            result.Single()[1].Should().Be("b");
            result.Single()[2].Should().Be("c");
        }

        [Fact]
        public void SelectPartitionWithOnePartitionReturnsTwoLists()
        {
            var items = new string[] { "a", "b", "", "c", "d" };
            var result = items.SelectPartition(string.IsNullOrEmpty).ToArray();

            result.Should().HaveCount(2);
            result[0][0].Should().Be("a");
            result[0][1].Should().Be("b");
            result[1][0].Should().Be("c");
            result[1][1].Should().Be("d");
        }

        [Fact]
        public void SelectPartitionThatEndsWithPartitionReturnsEmptyAtEnd()
        {
            var items = new string[] { "a", "b", "", "c", "d", "" };
            var result = items.SelectPartition(string.IsNullOrEmpty).ToArray();

            result.Should().HaveCount(3);
            result[0][0].Should().Be("a");
            result[0][1].Should().Be("b");
            result[1][0].Should().Be("c");
            result[1][1].Should().Be("d");
            result[2].Should().BeEmpty();
        }

        [Fact]
        public void SelectPartitionThatStartsWithPartitionReturnsEmptyAtStart()
        {
            var items = new string[] { "", "a", "b", "", "c", "d"};
            var result = items.SelectPartition(string.IsNullOrEmpty).ToArray();

            result.Should().HaveCount(3);
            result[0].Should().BeEmpty();
            result[1][0].Should().Be("a");
            result[1][1].Should().Be("b");
            result[2][0].Should().Be("c");
            result[2][1].Should().Be("d");
        }

        [Fact]
        public void SelectPartitionWithEmptyIsEmpty()
        {
            var items = new string[] { };
            var result = items.SelectPartition(string.IsNullOrEmpty).ToArray();

            result.Should().BeEmpty();
        }

        [Fact]
        public void SelectPartitionWithSinglePartitionHasSingleEmpty()
        {
            var items = new string[] { "" };
            var result = items.SelectPartition(string.IsNullOrEmpty).ToArray();

            result.Should().ContainSingle();
            result[0].Should().BeEmpty();
        }

        [Fact]
        public void SelectPartitionWithTwoPartitionsHasTwoEmpty()
        {
            var items = new string[] { "", "" };
            var result = items.SelectPartition(string.IsNullOrEmpty).ToArray();

            result.Should().HaveCount(2);
            result[0].Should().BeEmpty();
            result[1].Should().BeEmpty();
        }
    }
}
