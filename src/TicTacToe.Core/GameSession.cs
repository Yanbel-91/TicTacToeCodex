namespace TicTacToe.Core;

public sealed class GameSession
{
    private readonly Board _board = new();

    public GameSession()
    {
        CurrentPlayer = PlayerMark.X;
        Result = GameResult.InProgress();
    }

    public event EventHandler<GameStateChangedEventArgs>? StateChanged;

    public PlayerMark CurrentPlayer { get; private set; }

    public GameResult Result { get; private set; }

    public void Reset(PlayerMark startingPlayer)
    {
        _board.Reset();
        CurrentPlayer = startingPlayer;
        Result = GameResult.InProgress();
        RaiseStateChanged();
    }

    public bool TryMakeMove(BoardPosition position)
    {
        if (Result.IsTerminal || !_board.TrySet(position, CurrentPlayer))
        {
            return false;
        }

        Result = GameRules.Evaluate(_board.Snapshot());
        if (!Result.IsTerminal)
        {
            CurrentPlayer = GameRules.Next(CurrentPlayer);
        }

        RaiseStateChanged();
        return true;
    }

    public GameSnapshot CreateSnapshot() => new(_board.Snapshot(), CurrentPlayer, Result);

    private void RaiseStateChanged()
    {
        StateChanged?.Invoke(this, new GameStateChangedEventArgs(CreateSnapshot()));
    }
}

public sealed class GameStateChangedEventArgs : EventArgs
{
    public GameStateChangedEventArgs(GameSnapshot snapshot)
    {
        Snapshot = snapshot;
    }

    public GameSnapshot Snapshot { get; }
}
