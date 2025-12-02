using TicTacToe.Core;
using TicTacToe.Core.Bots;

namespace TicTacToe.WinForms;

internal sealed class GameCoordinator
{
    private readonly GameSession _session;
    private readonly IGameView _view;
    private readonly IReadOnlyList<IGameBot> _bots;
    private IGameBot _activeBot;

    private const PlayerMark HumanMark = PlayerMark.X;

    public GameCoordinator(GameSession session, IGameView view, IReadOnlyList<IGameBot> bots)
    {
        _session = session;
        _view = view;
        _bots = bots;
        _activeBot = bots.First();
        _session.StateChanged += SessionOnStateChanged;
    }

    public IEnumerable<IGameBot> Bots => _bots;

    public void SetBot(IGameBot bot)
    {
        _activeBot = bot;
        RunBotIfNeeded();
    }

    public void StartNewGame(bool botStarts)
    {
        var startingPlayer = botStarts ? _activeBot.Marker : HumanMark;
        _session.Reset(startingPlayer);
        RunBotIfNeeded();
    }

    public void HandleHumanMove(BoardPosition position)
    {
        if (_session.Result.IsTerminal)
        {
            return;
        }

        if (_session.CurrentPlayer != HumanMark)
        {
            _view.ShowStatus("Der Bot ist am Zug.");
            return;
        }

        if (_session.TryMakeMove(position))
        {
            RunBotIfNeeded();
        }
        else
        {
            _view.ShowStatus("Zug nicht möglich. Feld ist belegt.");
        }
    }

    private void SessionOnStateChanged(object? sender, GameStateChangedEventArgs e)
    {
        _view.Render(e.Snapshot);
        _view.ShowStatus(BuildStatusMessage(e.Snapshot));
    }

    private string BuildStatusMessage(GameSnapshot snapshot)
    {
        return snapshot.Result.ResultType switch
        {
            GameResultType.Draw => "Unentschieden.",
            GameResultType.Win when snapshot.Result.Winner == HumanMark => "Du hast gewonnen!",
            GameResultType.Win => $"{_activeBot.Name} gewinnt.",
            _ => snapshot.CurrentPlayer == HumanMark ? "Du bist am Zug." : $"{_activeBot.Name} denkt nach..."
        };
    }

    private void RunBotIfNeeded()
    {
        if (_session.Result.IsTerminal || _session.CurrentPlayer != _activeBot.Marker)
        {
            return;
        }

        var move = _activeBot.SelectMove(_session.CreateSnapshot());
        if (!_session.TryMakeMove(move))
        {
            _view.ShowStatus("Bot konnte keinen gültigen Zug finden.");
        }
    }
}
