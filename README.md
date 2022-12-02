# advent-of-code-2022
 Steve Ognibene's answers to the Advent of Code 2022 challenges

 Uses the .NET 7 SDK

 https://adventofcode.com/2021/

## Commentary

I am adding this commentary section because everyone (still) loves my opinions.

### Day 1:

A good warm-up using some LINQ and sorting by a grouping.  This year I set up using .NET 7 and I intend to use a single Program.cs as an entry point with persistent tests exercising the documented and individual results.  We'll see how interesting it gets.

### Day 2:

This was an interesting puzzle because it changed the meaning of some of the parsed data between part 1 and part 2.  I made use of the brand-new `UnreachableException` as part of some of the switch statements - this will replace `NotSupportedException` when doing exhaustiveness checks.  I got to do some pattern matching with tuples which is always fun.  So far, I'm glad I am doing this with each challenge being a class in a project.  This is proving fairly flexible with regards to testing and execution, and I can make a test using the documented results and add the "correct" results once I have them.
