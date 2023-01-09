using common;
using day01;
using System;
using System.Runtime.InteropServices.JavaScript;

public partial class AdventOfCode
{
    //[JSExport]
    //internal static string Greeting()
    //{
    //    var text = $"Hello, World! Greetings from {GetHRef()}";
    //    Console.WriteLine(text);
    //    return text;
    //}

    //[JSImport("window.location.href", "main.js")]
    //internal static partial string GetHRef();

    [JSExport]
    internal static string SolveDay1Part1(string input)
    {
        var day1 = new Day01(input.ReplaceLineEndings("\n").Split('\n'));
        return day1.GetAnswerForPart1();
    }

    [JSExport]
    internal static string GetDay1Data()
    {
        return Utils.GetResourceStringFromAssembly<Day01>("day01.input.txt");
    }
}
