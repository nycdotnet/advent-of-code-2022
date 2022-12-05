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

I did this manually at first but then remembered C# has a Range class added in C# 8.  After reminding myself about that, I went back to doing it manually as Range appears to only implemented to assist with getting ranges in collections.