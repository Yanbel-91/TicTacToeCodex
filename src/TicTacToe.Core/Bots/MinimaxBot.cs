namespace TicTacToe.Core.Bots;

public sealed class MinimaxBot : IGameBot
{
    public MinimaxBot(PlayerMark marker)
    {
        Marker = marker;
    }

    public string Name => "Optimal";

    public PlayerMark Marker { get; }

    public BoardPosition SelectMove(GameSnapshot snapshot)
    {
        var bestScore = int.MinValue;
        var bestMove = default(BoardPosition);

        foreach (var position in snapshot.GetEmptyPositions())
        {
            var simulated = snapshot.ApplyMove(position, Marker);
            var score = Minimax(simulated, maximizing: false);
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = position;
            }
        }

        return bestMove;
    }

    private int Minimax(GameSnapshot snapshot, bool maximizing)
    {
        if (snapshot.Result.IsTerminal)
        {
            return EvaluateTerminal(snapshot.Result);
        }

        var currentMark = maximizing ? Marker : Marker.Opponent();
        var bestScore = maximizing ? int.MinValue : int.MaxValue;

        foreach (var position in snapshot.GetEmptyPositions())
        {
            var simulated = snapshot.ApplyMove(position, currentMark);
            var score = Minimax(simulated, !maximizing);
            if (maximizing)
            {
                bestScore = Math.Max(bestScore, score);
            }
            else
            {
                bestScore = Math.Min(bestScore, score);
            }
        }

        return bestScore;
    }

    private int EvaluateTerminal(GameResult result)
    {
        return result.ResultType switch
        {
            GameResultType.Draw => 0,
            GameResultType.Win when result.Winner == Marker => 1,
            GameResultType.Win => -1,
            _ => 0
        };
    }
}
