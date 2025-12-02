namespace TicTacToe.Core.Bots;

public sealed class RandomBot : IGameBot
{
    public RandomBot(PlayerMark marker)
    {
        Marker = marker;
    }

    public string Name => "Zufall";

    public PlayerMark Marker { get; }

    public BoardPosition SelectMove(GameSnapshot snapshot)
    {
        var options = snapshot.GetEmptyPositions().ToList();
        if (options.Count == 0)
        {
            return default;
        }

        return options[Random.Shared.Next(options.Count)];
    }
}
