namespace TicTacToe.Api.Models;

public class MoveRequest
{
    public string? GameId { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public char Player { get; set; }
}
