namespace TicTacToe.Core;

public interface IGameBot
{
    string Name { get; }

    PlayerMark Marker { get; }

    BoardPosition SelectMove(GameSnapshot snapshot);
}
