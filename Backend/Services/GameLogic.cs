using System.Collections.Concurrent;
using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services;

public class GameLogic
{
    public const char Empty = ' ';
    public const char PlayerX = 'X';
    public const char PlayerO = 'O';

    private readonly ConcurrentDictionary<string, char[,]> _games = new();

    public GameResponse CreateGame()
    {
        var gameId = Guid.NewGuid().ToString("N");
        _games[gameId] = NewBoard();
        return BuildResponse(gameId, "Player X's turn.");
    }

    public GameResponse? ProcessHumanMove(MoveRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.GameId) || !_games.TryGetValue(request.GameId, out var board))
        {
            return null;
        }

        if (!IsValidMove(board, request.Row, request.Col))
        {
            return BuildResponse(request.GameId, "Invalid move. Cell is occupied or out of bounds.", "Invalid");
        }

        var player = request.Player == PlayerO ? PlayerO : PlayerX;
        board[request.Row, request.Col] = player;

        var isWin = CheckWinner(board, player);
        if (isWin)
        {
            var status = player == PlayerX ? "XWin" : "OWin";
            var winnerLabel = player == PlayerX ? "X" : "O";
            return BuildResponse(request.GameId, $"Player {winnerLabel} wins!", status, winnerLabel);
        }

        if (IsBoardFull(board))
        {
            return BuildResponse(request.GameId, "It's a draw!", "Draw");
        }

        var nextPlayer = player == PlayerX ? PlayerO : PlayerX;
        return BuildResponse(request.GameId, $"Player {nextPlayer}'s turn.");
    }

    public void ResetGame(string gameId)
    {
        if (_games.ContainsKey(gameId))
        {
            _games[gameId] = NewBoard();
        }
    }

    public void RemoveGame(string gameId)
    {
        _games.TryRemove(gameId, out _);
    }

    private static char[,] NewBoard()
    {
        var board = new char[3, 3];
        for (var r = 0; r < 3; r++)
        {
            for (var c = 0; c < 3; c++)
            {
                board[r, c] = Empty;
            }
        }

        return board;
    }

    private static bool IsValidMove(char[,] board, int row, int col)
    {
        if (row < 0 || row > 2 || col < 0 || col > 2)
        {
            return false;
        }

        return board[row, col] == Empty;
    }

    public static bool CheckWinner(char[,] board, char player)
    {
        for (var i = 0; i < 3; i++)
        {
            if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player)
            {
                return true;
            }

            if (board[0, i] == player && board[1, i] == player && board[2, i] == player)
            {
                return true;
            }
        }

        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
        {
            return true;
        }

        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
        {
            return true;
        }

        return false;
    }

    public static bool IsBoardFull(char[,] board)
    {
        for (var r = 0; r < 3; r++)
        {
            for (var c = 0; c < 3; c++)
            {
                if (board[r, c] == Empty)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static List<(int Row, int Col)> GetAvailableMoves(char[,] board)
    {
        var moves = new List<(int, int)>();
        for (var r = 0; r < 3; r++)
        {
            for (var c = 0; c < 3; c++)
            {
                if (board[r, c] == Empty)
                {
                    moves.Add((r, c));
                }
            }
        }

        return moves;
    }

    private GameResponse BuildResponse(
        string gameId,
        string message,
        string status = "InProgress",
        string? winner = null,
        int? aiRow = null,
        int? aiCol = null)
    {
        _games.TryGetValue(gameId, out var board);
        return new GameResponse
        {
            GameId = gameId,
            Board = ToJagged(board ?? NewBoard()),
            Status = status,
            Winner = winner,
            Message = message,
            AiRow = aiRow,
            AiCol = aiCol
        };
    }

    private static string[][] ToJagged(char[,] board)
    {
        var result = new string[3][];
        for (var r = 0; r < 3; r++)
        {
            result[r] = new string[3];
            for (var c = 0; c < 3; c++)
            {
                var cell = board[r, c];
                result[r][c] = cell == Empty ? "" : cell.ToString();
            }
        }

        return result;
    }
}
