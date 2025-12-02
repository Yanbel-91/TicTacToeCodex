using System.Collections.Generic;

namespace TicTacToe.Core;

public sealed class Board
{
    private readonly PlayerMark[,] _cells = new PlayerMark[3, 3];

    public PlayerMark this[int row, int column] => _cells[row, column];

    public bool IsEmpty(BoardPosition position)
    {
        return position.IsInsideBoard && _cells[position.Row, position.Column] == PlayerMark.None;
    }

    public bool TrySet(BoardPosition position, PlayerMark mark)
    {
        if (!position.IsInsideBoard || mark == PlayerMark.None || !IsEmpty(position))
        {
            return false;
        }

        _cells[position.Row, position.Column] = mark;
        return true;
    }

    public IReadOnlyCollection<BoardPosition> GetEmptyPositions()
    {
        var positions = new List<BoardPosition>();
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                if (_cells[row, col] == PlayerMark.None)
                {
                    positions.Add(new BoardPosition(row, col));
                }
            }
        }

        return positions;
    }

    public PlayerMark[,] Snapshot()
    {
        var clone = new PlayerMark[3, 3];
        Array.Copy(_cells, clone, _cells.Length);
        return clone;
    }

    public void Reset()
    {
        Array.Clear(_cells, 0, _cells.Length);
    }
}
