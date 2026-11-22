
namespace ConnectFour.Models;

public class GameState
{
    public const int Rows = 6;
    public const int Columns = 7;

    // 0 = empty, 1 = Player 1, 2 = Player 2
    public int[,] Board { get; private set; } = new int[Rows, Columns];

    public int CurrentPlayer { get; private set; } = 1;

    public int? Winner { get; private set; }

    public bool IsDraw { get; private set; }

    public List<Move> History { get; private set; } = new();

    public bool IsGameOver => Winner.HasValue || IsDraw;

    public bool DropPiece(int column)
    {
        if (column < 0 || column >= Columns || IsGameOver)
            return false;

        for (int row = Rows - 1; row >= 0; row--)
        {
            if (Board[row, column] == 0)
            {
                Board[row, column] = CurrentPlayer;

                History.Add(new Move(
                    History.Count + 1,
                    CurrentPlayer,
                    column + 1
                ));

                if (CheckWinner(row, column))
                {
                    Winner = CurrentPlayer;
                }
                else if (History.Count == Rows * Columns)
                {
                    IsDraw = true;
                }
                else
                {
                    CurrentPlayer = CurrentPlayer == 1 ? 2 : 1;
                }

                return true;
            }
        }

        return false;
    }

    private bool CheckWinner(int row, int column)
    {
        int player = Board[row, column];

        (int row, int col)[] directions =
        {
            (0, 1),  // Horizontal
            (1, 0),  // Vertical
            (1, 1),  // Diagonal down
            (1, -1)  // Diagonal up
        };

        foreach (var (rowDirection, colDirection) in directions)
        {
            int count = 1;

            count += CountDirection(
                row, column, rowDirection, colDirection, player);

            count += CountDirection(
                row, column, -rowDirection, -colDirection, player);

            if (count >= 4)
                return true;
        }

        return false;
    }

    private int CountDirection(
        int row,
        int column,
        int rowDirection,
        int colDirection,
        int player)
    {
        int count = 0;

        int currentRow = row + rowDirection;
        int currentColumn = column + colDirection;

        while (
            currentRow >= 0 &&
            currentRow < Rows &&
            currentColumn >= 0 &&
            currentColumn < Columns &&
            Board[currentRow, currentColumn] == player)
        {
            count++;
            currentRow += rowDirection;
            currentColumn += colDirection;
        }

        return count;
    }

    public void Reset()
    {
        Board = new int[Rows, Columns];
        CurrentPlayer = 1;
        Winner = null;
        IsDraw = false;
        History.Clear();
    }
}

public record Move(int Number, int Player, int Column);
