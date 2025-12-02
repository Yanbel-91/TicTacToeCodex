namespace TicTacToe.Core.Bots;

public sealed class HeuristicBot : IGameBot
{
    public HeuristicBot(PlayerMark marker)
    {
        Marker = marker;
    }

    public string Name => "Taktik";

    public PlayerMark Marker { get; }

    public BoardPosition SelectMove(GameSnapshot snapshot)
    {
        var empty = snapshot.GetEmptyPositions().ToList();
        if (empty.Count == 0)
        {
            return default;
        }

        var winningMove = FindCriticalMove(snapshot, Marker);
        if (winningMove is { } win)
        {
            return win;
        }

        var blockingMove = FindCriticalMove(snapshot, Marker.Opponent());
        if (blockingMove is { } block)
        {
            return block;
        }

        var center = new BoardPosition(1, 1);
        if (empty.Contains(center))
        {
            return center;
        }

        var corners = new[]
        {
            new BoardPosition(0, 0),
            new BoardPosition(0, 2),
            new BoardPosition(2, 0),
            new BoardPosition(2, 2)
        };

        var availableCorner = corners.FirstOrDefault(empty.Contains);
        if (availableCorner.IsInsideBoard && empty.Contains(availableCorner))
        {
            return availableCorner;
        }

        return empty[Random.Shared.Next(empty.Count)];
    }

    private static BoardPosition? FindCriticalMove(GameSnapshot snapshot, PlayerMark target)
    {
        foreach (var position in snapshot.GetEmptyPositions())
        {
            var simulated = snapshot.ApplyMove(position, target);
            if (simulated.Result.ResultType == GameResultType.Win && simulated.Result.Winner == target)
            {
                return position;
            }
        }

        return null;
    }
}
