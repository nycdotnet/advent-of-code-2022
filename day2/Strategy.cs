using System.Diagnostics;

namespace day2
{
    public record Strategy
    {
        public Play TheyPlay { get; private set; }
        public Play Part1YouPlay { get; private set; }
        public Play Part2YouPlay => (TheyPlay, Part2RequiredResult) switch
        {
            (_, Result.Draw) => TheyPlay,
            (Play.Scissors, Result.Win) => Play.Rock,
            (Play.Scissors, Result.Lose) => Play.Paper,
            (Play.Rock, Result.Win) => Play.Paper,
            (Play.Rock, Result.Lose) => Play.Scissors,
            (Play.Paper, Result.Win) => Play.Scissors,
            (Play.Paper, Result.Lose) => Play.Rock,
            _ => throw new UnreachableException("Unexpected combination")
        };

        public Result Part2RequiredResult { get; private set; }

        public static Result GetResult(Play TheyPlay, Play YouPlay) =>
            (TheyPlay, YouPlay) switch {
                (Play.Scissors, Play.Rock) => Result.Win,
                (Play.Scissors, Play.Paper) => Result.Lose,
                (Play.Rock, Play.Paper) => Result.Win,
                (Play.Rock, Play.Scissors) => Result.Lose,
                (Play.Paper, Play.Scissors) => Result.Win,
                (Play.Paper, Play.Rock) => Result.Lose,
                _ => Result.Draw
            };

        /// <summary>
        /// An experiment to see if checking the equal case as a shortcut would
        /// be faster.  It's not, at least not with raising an unreachable
        /// exception as an exhaustiveness guard.
        /// </summary>
        public static Result GetResultAlt(Play TheyPlay, Play YouPlay) =>
            TheyPlay == YouPlay ? Result.Draw :
                 (TheyPlay, YouPlay) switch
                 {
                     (Play.Scissors, Play.Rock) => Result.Win,
                     (Play.Scissors, Play.Paper) => Result.Lose,
                     (Play.Rock, Play.Paper) => Result.Win,
                     (Play.Rock, Play.Scissors) => Result.Lose,
                     (Play.Paper, Play.Scissors) => Result.Win,
                     (Play.Paper, Play.Rock) => Result.Lose,
                     _ => throw new UnreachableException()
                 };

        /// <summary>
        /// Another experiment - also not faster
        /// </summary>
        public static Result GetResultAlt2(Play TheyPlay, Play YouPlay) =>
            (TheyPlay, YouPlay) switch
            {
                (var t, var y) when t == y => Result.Draw,
                (Play.Scissors, Play.Rock) => Result.Win,
                (Play.Scissors, Play.Paper) => Result.Lose,
                (Play.Rock, Play.Paper) => Result.Win,
                (Play.Rock, Play.Scissors) => Result.Lose,
                (Play.Paper, Play.Scissors) => Result.Win,
                (Play.Paper, Play.Rock) => Result.Lose
            };

        public static int GetScore(Result result, Play youPlayed) =>
            result switch {
                Result.Win => 6,
                Result.Lose => 0,
                Result.Draw => 3,
                var v => throw new UnreachableException($"Unknown value {v} for {nameof(Result)}.")
            } + youPlayed switch
            {
                Play.Paper => 2,
                Play.Rock => 1,
                Play.Scissors => 3,
                var v => throw new UnreachableException($"Unknown value {v} for {nameof(Play)}.")
            };

        public Result Part1Result => GetResult(TheyPlay, Part1YouPlay);
        public Result Part1ResultAlt => GetResultAlt(TheyPlay, Part1YouPlay);
        public Result Part1ResultAlt2 => GetResultAlt2(TheyPlay, Part1YouPlay);

        public int Part1ScoreResult => GetScore(Part1Result, Part1YouPlay);
        public int Part1ScoreResultAlt => GetScore(Part1ResultAlt, Part1YouPlay);
        public int Part1ScoreResultAlt2 => GetScore(Part1ResultAlt2, Part1YouPlay);
        public int Part2ScoreResult => GetScore(Part2RequiredResult, Part2YouPlay);


        public static Strategy Parse(string strategyText)
        {
            if (strategyText.Length != 3)
            {
                throw new ArgumentOutOfRangeException($"Invalid input \"{strategyText}\".  Must be exactly 3 characters.");
            }

            var result = new Strategy
            {
                TheyPlay = strategyText[0] switch
                {
                    'A' => Play.Rock,
                    'B' => Play.Paper,
                    'C' => Play.Scissors,
                    var other => throw new ArgumentOutOfRangeException($"Expected A, B, or C, got {other}.")
                },
                Part1YouPlay = strategyText[2] switch
                {
                    'Y' => Play.Paper,
                    'X' => Play.Rock,
                    'Z' => Play.Scissors,
                    var other => throw new ArgumentOutOfRangeException($"Expected X, Y, or Z, got {other}.")
                },
                Part2RequiredResult = strategyText[2] switch
                {
                    'Y' => Result.Draw,
                    'X' => Result.Lose,
                    'Z' => Result.Win,
                    var other => throw new ArgumentOutOfRangeException($"Expected X, Y, or Z, got {other}.")
                }
            };

            return result;
        }

        public enum Play
        {
            Rock,
            Paper,
            Scissors
        }

        public enum Result
        {
            Lose,
            Draw,
            Win
        }
    }
}