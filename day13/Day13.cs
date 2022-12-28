using common;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace day13
{
    public class Day13 : IAdventOfCodeDay
    {
        public Day13(IEnumerable<string> input)
        {
            Pairs = input.SelectPartition(string.IsNullOrEmpty).ToList();
        }

        public static AnalysisResult Analyze(string leftPacket, string rightPacket)
        {
            throw new NotImplementedException();
            //var left = JsonNode.Parse(leftPacket) as JsonArray;
            //var right = JsonNode.Parse(rightPacket) as JsonArray;
            //var areInRightOrder = false;

            //if (left is null)
            //{
            //    throw new ArgumentNullException("Left could not be parsed as an array.");
            //}
            //if (right is null)
            //{
            //    throw new ArgumentNullException("Right could not be parsed as an array.");
            //}

            //// We are going to work with a zero-based i, but when printing index, we should add one
            //// because the problem defines "index" to be 1-based.
            //int i = 0;
            //while(true)
            //{
            //    if (i >= left.Count)
            //    {
            //        throw new IndexOutOfRangeException($"One-based index {i + 1} is too big for left with Count {left.Count}.");
            //    }

            //    if (i >= right.Count)
            //    {
            //        throw new IndexOutOfRangeException($"One-based index {i + 1} is too big for right with Count {right.Count}.");
            //    }

            //    var leftArray = left[i] as JsonArray;
            //    var rightArray = right[i] as JsonArray;

            //    if (leftArray is not null && rightArray is not null)
            //    {

            //    }
            //    else if (leftArray is null && rightArray is null)
            //    {
            //        // both are integers.
            //        var leftNumber = left[i]!.GetValue<int>();
            //        var rightNumber = right[i]!.GetValue<int>();
            //        if (leftNumber == rightNumber)
            //        {
            //            i += 1;
            //            continue;
            //        }
            //        else if (leftNumber < rightNumber)
            //        {
            //            areInRightOrder = true;
            //            break;
            //        }
            //        else if (leftNumber > rightNumber)
            //        {
            //            areInRightOrder = false;
            //            break;
            //        }
            //    }
            //    else
            //    {
            //        throw new NotSupportedException();
            //    }


            //    //var leftValue = left[index - 1].GetValue<object>();
            //    //var rightValue = right[index - 1].GetValue<object>();

            //    //if (leftValue is JsonElement le && rightValue is JsonElement re)
            //    //{
            //    //    var leftNumber = le.GetInt32();
            //    //    var rightNumber = re.GetInt32();


            //    //}
            //    //else
            //    //{
            //    //    throw new NotSupportedException();
            //    //}
            //}

            //return new AnalysisResult(areInRightOrder);
        }

        public record AnalysisResult(bool AreInRightOrder);

        public List<List<string>> Pairs { get; }

        public string GetAnswerForPart1()
        {
            throw new NotImplementedException();
        }

        public string GetAnswerForPart2()
        {
            throw new NotImplementedException();
        }
    }
}