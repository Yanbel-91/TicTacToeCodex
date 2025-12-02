using TicTacToe.Core;

namespace TicTacToe.WinForms;

internal interface IGameView
{
    void Render(GameSnapshot snapshot);

    void ShowStatus(string message);
}
