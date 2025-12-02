namespace TicTacToe.Core;

public readonly record struct BoardPosition(int Row, int Column)
{
    public bool IsInsideBoard => Row is >= 0 and < 3 && Column is >= 0 and < 3;

    public override string ToString() => $"({Row},{Column})";
}
