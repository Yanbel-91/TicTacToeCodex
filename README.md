# TicTacToeCodex

Windows-Forms-Umsetzung eines Tic-Tac-Toe-Spiels mit drei Bot-Schwierigkeitsgraden.

## Projekte
- `TicTacToe.Core` – Spiellogik, Bot-Strategien und Zustandsverwaltung.
- `TicTacToe.WinForms` – UI-Schicht mit einem einfachen 3x3-Spielfeld und Bot-Auswahl.

## Voraussetzungen
- .NET 8 SDK auf Windows (Windows Forms erfordert Windows-Targeting).

## Build
```
dotnet build TicTacToeCodex.sln
```

## Ausführen
```
dotnet run --project src/TicTacToe.WinForms/TicTacToe.WinForms.csproj
```
