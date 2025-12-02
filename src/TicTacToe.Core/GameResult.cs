namespace TicTacToe.Core;

public enum GameResultType
{
    InProgress,
    Draw,
    Win
}

public sealed record GameResult(GameResultType ResultType, PlayerMark Winner)
{
    public bool IsTerminal => ResultType != GameResultType.InProgress;

    public static GameResult InProgress() => new(GameResultType.InProgress, PlayerMark.None);

    public static GameResult Draw() => new(GameResultType.Draw, PlayerMark.None);

    public static GameResult Win(PlayerMark winner) => new(GameResultType.Win, winner);
}
