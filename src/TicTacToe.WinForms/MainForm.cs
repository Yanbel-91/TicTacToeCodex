using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TicTacToe.Core;
using TicTacToe.Core.Bots;

namespace TicTacToe.WinForms;

public sealed class MainForm : Form, IGameView
{
    private readonly Button[,] _boardButtons = new Button[3, 3];
    private readonly Label _statusLabel = new() { AutoSize = true };
    private readonly ComboBox _botSelector = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly CheckBox _botStartsCheckBox = new() { Text = "Bot beginnt", AutoSize = true };
    private readonly Button _newGameButton = new() { Text = "Neues Spiel" };

    private readonly GameCoordinator _coordinator;

    public MainForm()
    {
        Text = "Tic Tac Toe";
        MinimumSize = new Size(420, 520);

        var session = new GameSession();
        var bots = new List<IGameBot>
        {
            new RandomBot(PlayerMark.O),
            new HeuristicBot(PlayerMark.O),
            new MinimaxBot(PlayerMark.O)
        };

        _coordinator = new GameCoordinator(session, this, bots);
        InitializeLayout(bots);
        StartNewGame();
    }

    private void InitializeLayout(IReadOnlyList<IGameBot> bots)
    {
        var outerLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1,
            Padding = new Padding(12)
        };

        outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        outerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

        var topRow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        topRow.Controls.Add(new Label { Text = "Bot", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 4, 0) });
        foreach (var bot in bots)
        {
            _botSelector.Items.Add(bot.Name);
        }

        _botSelector.SelectedIndex = 0;
        _botSelector.SelectedIndexChanged += (_, _) => UpdateBotSelection();
        _botStartsCheckBox.CheckedChanged += (_, _) => StartNewGame();
        _newGameButton.Click += (_, _) => StartNewGame();

        topRow.Controls.Add(_botSelector);
        topRow.Controls.Add(_botStartsCheckBox);
        topRow.Controls.Add(_newGameButton);

        outerLayout.Controls.Add(topRow, 0, 0);

        var boardLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 3,
            Padding = new Padding(4),
            BackColor = Color.LightGray
        };

        for (var i = 0; i < 3; i++)
        {
            boardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            boardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        }

        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                var button = new Button
                {
                    Dock = DockStyle.Fill,
                    Font = new Font(FontFamily.GenericSansSerif, 26, FontStyle.Bold),
                    Tag = new BoardPosition(row, col),
                    Margin = new Padding(6)
                };

                button.Click += (_, _) => OnCellClicked(button);
                _boardButtons[row, col] = button;
                boardLayout.Controls.Add(button, col, row);
            }
        }

        outerLayout.Controls.Add(boardLayout, 0, 1);

        var statusPanel = new Panel { Dock = DockStyle.Fill };
        _statusLabel.Text = "Bereit";
        statusPanel.Controls.Add(_statusLabel);
        outerLayout.Controls.Add(statusPanel, 0, 2);

        Controls.Add(outerLayout);
    }

    private void UpdateBotSelection()
    {
        if (_botSelector.SelectedIndex >= 0)
        {
            var selectedBot = _coordinator.Bots.ElementAt(_botSelector.SelectedIndex);
            _coordinator.SetBot(selectedBot);
        }
    }

    private void StartNewGame()
    {
        UpdateBotSelection();
        _coordinator.StartNewGame(_botStartsCheckBox.Checked);
    }

    private void OnCellClicked(Button button)
    {
        if (button.Tag is BoardPosition position)
        {
            _coordinator.HandleHumanMove(position);
        }
    }

    public void Render(GameSnapshot snapshot)
    {
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                var position = new BoardPosition(row, col);
                var mark = snapshot.GetCell(position);
                var button = _boardButtons[row, col];
                button.Text = mark switch
                {
                    PlayerMark.X => "X",
                    PlayerMark.O => "O",
                    _ => string.Empty
                };

                button.Enabled = snapshot.Result.ResultType == GameResultType.InProgress && mark == PlayerMark.None;
            }
        }
    }

    public void ShowStatus(string message)
    {
        _statusLabel.Text = message;
    }
}
