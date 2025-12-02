namespace TicTacToe.Core;

public enum PlayerMark
{
    None = 0,
    X = 1,
    O = 2
}

public static class PlayerMarkExtensions
{
    public static PlayerMark Opponent(this PlayerMark mark) => mark switch
    {
        PlayerMark.X => PlayerMark.O,
        PlayerMark.O => PlayerMark.X,
        _ => PlayerMark.None
    };
}
