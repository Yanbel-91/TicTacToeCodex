namespace TicTacToe.Core;

public sealed class GameSnapshot
{
    public GameSnapshot(PlayerMark[,] cells, PlayerMark currentPlayer, GameResult result)
    {
        Cells = cells;
        CurrentPlayer = currentPlayer;
        Result = result;
    }

    public PlayerMark[,] Cells { get; }

    public PlayerMark CurrentPlayer { get; }

    public GameResult Result { get; }

    public PlayerMark GetCell(BoardPosition position) => Cells[position.Row, position.Column];

    public IEnumerable<BoardPosition> GetEmptyPositions()
    {
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                if (Cells[row, col] == PlayerMark.None)
                {
                    yield return new BoardPosition(row, col);
                }
            }
        }
    }

    public GameSnapshot ApplyMove(BoardPosition position, PlayerMark mark)
    {
        var nextCells = new PlayerMark[3, 3];
        Array.Copy(Cells, nextCells, Cells.Length);
        nextCells[position.Row, position.Column] = mark;
        var result = GameRules.Evaluate(nextCells);
        var nextPlayer = result.IsTerminal ? CurrentPlayer : GameRules.Next(mark);
        return new GameSnapshot(nextCells, nextPlayer, result);
    }
}
