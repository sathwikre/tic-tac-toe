namespace TicTacToe.Api.Models;

public class GameResponse
{
    public string GameId { get; set; } = string.Empty;
    public string[][] Board { get; set; } = Array.Empty<string[]>();
    public string Status { get; set; } = "InProgress";
    public string? Winner { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? AiRow { get; set; }
    public int? AiCol { get; set; }
}
