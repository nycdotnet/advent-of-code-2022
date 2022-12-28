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

I added a little benchmark for seeing if shortcutting on the values being equal would speed things up.  It didn't.  Both my attempts to make things faster failed, though I believe part of that is due to the possibility of the new implementations potentially throwing an `UnreachableException` rather than defaulting to a draw (this is also potentially an artifact of my data).

```
|                 Method |     Mean |    Error |   StdDev | Rank |
|----------------------- |---------:|---------:|---------:|-----:|
|     GetResultBenchmark | 45.80 us | 0.362 us | 0.302 us |    1 |
| GetResultBenchmarkAlt2 | 48.28 us | 0.152 us | 0.135 us |    2 |
|  GetResultBenchmarkAlt | 49.00 us | 0.522 us | 0.488 us |    2 |
```

The "optimized" implementations can be compared via this [SharpLab.io link](https://sharplab.io/#v2:CYLg1APgAgTAjAWAFBQAwAIpwHQBECWAhgOYB2A9gM4Au+AxpQNzLJQDMmM6AwugN7J0QzBywA2dACUAppQCuAG2roA4tOoz5SgBQAFBYQCe6ACoALaYf1GANOmvGAmuTkOAlOgC8APkHD//trmlg52zq4Ghh6UAO741HRm/H4BqcJ6kdgAynT4lJTkAE6Udg7YkuR0ANYePlKyitTYAOr4pDYpaakZRtm5+UUl9pm6hAAO0oW13vVaTQAyVNIdSF1dPYbllVWlI+OT07ONLW0ra2kbW9W7vTl5BcWHmseLlMud50KXoxOFN5t3AaPLwzZ5KE7tD6fb77P7DXoVapPBrg17vVaffwAfRBR3BuEKhBiUNSAF9mBihJ12Jg4BIwco1BoUdQAII6BymCxWSL/dDhdwgkkBYI8oxeTz8lycgD8eKaBKJ6BAwrWQW5oSlESM0TiCTMqq6AkpmO6ZUBDyGZURNVxDIhZ1NF3N/Ut/2wPwOdpZ2DRjqdAUuNvdnqm3rmDsN0Ot23dFsGyIjfqj5xhvzjroT4eOrUhJoDXzKofdNsTLyW/oLQhxdWoZkK5Bi6FI0ibAFVSIVpIREoQAEYKaQAUQAHnRpGNaORSNo3CmAuSWCaaeJ5ap1Az2dQYBsuSFefCnNLItNher97YtYLYvFEsLjZjtAA3QiFdDUOwvt9RdAxCykd8JXQYw6ntRUYkrdYXXuQYS22MtwVzSDnUyeNihDWEEIWCsUyDWNDw9TDs0Q05cJja4CLQygsN9HD8xQ3pi0ozNgVAn0kLIvZ0wI0tiOwt5kNSGsZjrBsmxbdtO27XsB2HMcJynGc53ooRFxNakOGkUg5AAW0PToHzSYNhWLYUqM6Ukl38GktN0+UDOFZMVPQcDBPQXMLOQUkgA=); SharpLab is a really cool tool to help see what code is actually going to run.

### Day 3:

This one was interesting because the RP of it made it sound more complicated than the program requirements were.  This was actually rather similar to day 1 and 2 I think - mostly dealing with sets of things and finding stuff in common.  I am curious about the performance implications of using the HashSet to do the `.Intersect` work versus just using an array for the size of the data (under 50 chars) - I may have to benchmark that.  Test Explorer claims the problems both run in under 1 ms so it can't be that bad, but perhaps rented arrays would result in less overall memory allocation.

### Day 4:

I did this manually at first but then remembered C# has a Range class added in C# 8.  After reminding myself about that, I went back to doing it manually as Range appears to only implemented to assist with getting ranges in collections.  Pretty cool that days 1-4 run in just about 32 ms.  Flexing that revised Regex system including named capture groups and parsing from spans.

### Day 5:

This one was interesting.  Definitely had a much more complicated parsing requirement - effectively parsing a diagram and a set of instructions.  It was interesting to realize that LINQ doesn't really seem to have a thing to fork a stream, but maybe I'm missing something.  Like "do this until you find condition X, then switch to this other method" would be interesting to have a way to do, but I wonder how you'd aggregate it - possibly in a tuple of enumerables?  Anyway - I was able to do it in two parts and the rest of the problem was pretty simple.  One was literally using Stack and then the second part was reversing the order that the stack would work.  I used a List here but maybe there's a better way by switching to use List<T>s instead of Stack<T>s.  Not sure, but it's done.  Minor optimization by re-using the List and clearing rather than newing one up on each iteration.

I am getting the sense that this year is way easier than last year, though I am sure there will be a mind-blowing one soon enough.  No bit fiddling yet this year yet.

### Day 6:

Yeah still simple.  Really just one method to write and change input size, but I tweaked it a bit so in theory it could be used with more data and still run in a reasonable time.  I'm certain it could be further optimized by bailing early rather than at the end, but it was fast enough.  I anticipate Day 7 being a whopper as we didn't fully explore the communicator protocol.

Total runtime Day 1 - Day 6 is 34.4ms with CTRL+F5.

### Day 7:

OK definitely the complexity here is still on the parser side, though wow it got a lot more complex today.  The challenge was to design a system to interpret the results of basic file system commands (like `ls`, `cd`, etc) to build out a file system hierarchy and then identify a subfolder with at least a certain amount of files.  Taking it a bit at a time it worked out ok.  I started without using an array of the input data and just tried to use a raw Enumerator, but gave up as I was having to keep track of if it was done or not all over.  The array allocation was small for this use case, so I didn't look back.  I may check out a raw enumerator later with a wrapper for "IsComplete" possibly in the future to compare the allocations.

Total runtime Day 1 - Day 7 is 40.1ms with CTRL+F5.

### Day 8:

For some reason I really struggled with this one and it was mostly a series of off-by-one errors.  My implementation returned the correct result for the example input but was wrong and bombed on the real one.  I did a bunch of mental step-throughs and flopped around, but it wasn't until I made some better test cases that I figured out the issue and got it right.  Part 2 was fairly quick by comparison.  It goes to show that TDD can really be helpful for getting the implementation right.  I fell behind this day.

### Day 9:

This one was a simulation of rope physics.  The main trick with part 2 was that the rope was longer.  I was able to refactor the implementation of part 1 to call the implementation of part 2 and just say that the rope was 2 units long.  This was a pretty cool one I think, and for some reason the implementation was rather easy (generally speaking) even though it sounded fairly intimidating.

### Day 10:

This required simulation of a CPU with a fixed clock and corresponding CRT, and it included a Youtube video link to a "racing the beam" thing describing how the Atari 2600 worked - very cool.  I was able to get to it fairly straight-forwardly.  The main source of bugs I had (twice!) was forgetting that the `%` operator has high precedence and I needed to use parentheses more.

Also this one caused me to have to do the "annual rename" of all the earlier projects to use two-digit project and class names.  :-(

### Day 11:

This one was definitely quite hard - the parsing was fairly significant, part 1 was straight-forward once the parsing was done, but part 2 was impossible without knowing the math trick.  I learned something that in retrospect was fairly obvious but I had to look up how to do it.  Perhaps I could have figured it out over time, but yeah `int` didn't work, `long` didn't work, and then `BigInteger` didn't work either haha so, yeah there must be a trick.  Who knows if I'll run into this in the future, but up until now I haven't in the wild.  I did notice some people were complaining about this problem on the subreddit, but honestly I feel like it's fine to learn something about math/number theory and not just get the programming practice in.

Note: This was the first time I used `BigInteger`.  Was very easy to switch to this from `int`/`long` and seemed fast enough until the numbers got ridiculous.  It's not required anymore once you know the "trick", but I am leaving it since it was fine.

### Day 12:

This one was another AStar challenge like last year.  I re-used the implementation from last year as a starting point, but was able to upgrade it to use generic math with .NET 7.  I'm very pleased with how it turned out, though I am sure it could be better.  I did add a shortcut feature to stop if the cost exceeds the previous best, but I kind of feel like this made the code a bit worse and it could probably be improved.  I was surprised that the result of starting from any given starting point was only 1 better than the result of starting from position `'S'`.  The performance of today's program is also by far the worst of any of the days with a 1.1 second runtime.  Oh well.  I know I am converting back and forth between Point2d and the map chars a bit too much and that is creating a lot of unnecessary garbage, but I suspect there is a better algorithm to use to solve multiple starting points vs a single one.  Some sort of A* where it doesn't throw away the data collected, maybe.  I will have to evaluate at some point in the future.

### Day 13:

I didn't like this one from the start.  It reminded me a lot of the Snailfish math one from last year.  I wound up getting through it without too much trouble once I took a break for a few days.  Some interesting implementation details are having implemented `IComparable<T>` to enable the sorting capability, and using the `OneOf` NuGet package for the first time to lean-in to the `ArrayOrNumber` behavior.  I thought I was being clever by making an overload of the `ArrayOrNumber` which took a `JsonNode` but then DUH this is the base class of the two so it was ambiguous.  Wound up with I think a fairly decent implementation, if a bit slow (~100 ms).  I don't think permanent normalization would work; I think it'd be a lot faster if I could avoid allocating potentially multiple strings on each sort.

OK I switched from parsing a new array each time to creating it in memory and it is significantly faster (a bit better than 2x at ~48ms).  Parallelism on the sort doesn't seem to make a difference.

```csharp
  // old example:
   new ArrayOrNumber(JsonNode.Parse($"[{leftValue.GetValue<int>()}]"))

   // new example:
   new ArrayOrNumber(new JsonArray(leftValue.GetValue<int>())),
```
