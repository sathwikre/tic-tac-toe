namespace TicTacToe.Api.Services;

public class AIPlayer
{
    private static readonly (int Row, int Col)[][] WinLines =
    {
        new[] { (0, 0), (0, 1), (0, 2) },
        new[] { (1, 0), (1, 1), (1, 2) },
        new[] { (2, 0), (2, 1), (2, 2) },
        new[] { (0, 0), (1, 0), (2, 0) },
        new[] { (0, 1), (1, 1), (2, 1) },
        new[] { (0, 2), (1, 2), (2, 2) },
        new[] { (0, 0), (1, 1), (2, 2) },
        new[] { (0, 2), (1, 1), (2, 0) }
    };

    public (int Row, int Col) ChooseMove(char[,] board)
    {
        var winningMove = FindStrategicMove(board, GameLogic.PlayerO);
        if (winningMove.HasValue)
        {
            return winningMove.Value;
        }

        var blockingMove = FindStrategicMove(board, GameLogic.PlayerX);
        if (blockingMove.HasValue)
        {
            return blockingMove.Value;
        }

        var available = GameLogic.GetAvailableMoves(board);
        var index = Random.Shared.Next(available.Count);
        return available[index];
    }

    private static (int Row, int Col)? FindStrategicMove(char[,] board, char player)
    {
        foreach (var line in WinLines)
        {
            var matchCount = 0;
            (int Row, int Col)? emptyCell = null;

            foreach (var (row, col) in line)
            {
                if (board[row, col] == player)
                {
                    matchCount++;
                }
                else if (board[row, col] == GameLogic.Empty)
                {
                    emptyCell = (row, col);
                }
                else
                {
                    matchCount = -1;
                    break;
                }
            }

            if (matchCount == 2 && emptyCell.HasValue)
            {
                return emptyCell;
            }
        }

        return null;
    }
}
