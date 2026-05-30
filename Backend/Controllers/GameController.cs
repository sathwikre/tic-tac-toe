using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Models;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly GameLogic _gameLogic;

    public GameController(GameLogic gameLogic)
    {
        _gameLogic = gameLogic;
    }

    [HttpPost("new")]
    public ActionResult<GameResponse> NewGame()
    {
        var response = _gameLogic.CreateGame();
        return Ok(response);
    }

    [HttpPost("move")]
    public ActionResult<GameResponse> Move([FromBody] MoveRequest request)
    {
        var response = _gameLogic.ProcessHumanMove(request);
        if (response is null)
        {
            return NotFound(new { message = "Game not found. Start a new game." });
        }

        if (response.Status == "Invalid")
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("reset/{gameId}")]
    public ActionResult<GameResponse> Reset(string gameId)
    {
        _gameLogic.ResetGame(gameId);
        return Ok(new GameResponse
        {
            GameId = gameId,
            Board = EmptyBoard(),
            Status = "InProgress",
            Message = "Board reset. Your turn."
        });
    }

    private static string[][] EmptyBoard()
    {
        return
        [
            ["", "", ""],
            ["", "", ""],
            ["", "", ""]
        ];
    }
}
