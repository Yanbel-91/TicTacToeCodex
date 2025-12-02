namespace TicTacToe.Core;

public static class GameRules
{
    public static GameResult Evaluate(PlayerMark[,] cells)
    {
        for (var row = 0; row < 3; row++)
        {
            if (cells[row, 0] != PlayerMark.None && cells[row, 0] == cells[row, 1] && cells[row, 1] == cells[row, 2])
            {
                return GameResult.Win(cells[row, 0]);
            }
        }

        for (var col = 0; col < 3; col++)
        {
            if (cells[0, col] != PlayerMark.None && cells[0, col] == cells[1, col] && cells[1, col] == cells[2, col])
            {
                return GameResult.Win(cells[0, col]);
            }
        }

        if (cells[0, 0] != PlayerMark.None && cells[0, 0] == cells[1, 1] && cells[1, 1] == cells[2, 2])
        {
            return GameResult.Win(cells[0, 0]);
        }

        if (cells[0, 2] != PlayerMark.None && cells[0, 2] == cells[1, 1] && cells[1, 1] == cells[2, 0])
        {
            return GameResult.Win(cells[0, 2]);
        }

        var hasEmpty = false;
        foreach (var cell in cells)
        {
            if (cell == PlayerMark.None)
            {
                hasEmpty = true;
                break;
            }
        }

        return hasEmpty ? GameResult.InProgress() : GameResult.Draw();
    }

    public static PlayerMark Next(PlayerMark mark) => mark.Opponent();
}
