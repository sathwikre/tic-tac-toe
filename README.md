# Tic-Tac-Toe vs AI

Full-stack Tic-Tac-Toe with an ASP.NET Core Web API backend and a vanilla HTML/CSS/JavaScript frontend. You play as **X**; the AI plays as **O** using win → block → random strategy.

## Project structure

```
tictaktoi/
├── Backend/
│   ├── Controllers/GameController.cs
│   ├── Services/GameLogic.cs
│   ├── Services/AIPlayer.cs
│   ├── Models/MoveRequest.cs
│   ├── Models/GameResponse.cs
│   ├── Program.cs
│   └── TicTacToe.Api.csproj
├── Frontend/
│   ├── index.html
│   ├── style.css
│   └── script.js
└── README.md
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A modern web browser
- Optional: [Live Server](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) or Python for serving the frontend

## Setup and run

### 1. Start the API

```bash
cd Backend
dotnet restore
dotnet run
```

The API listens at **http://localhost:5180**. Swagger UI: http://localhost:5180/swagger

### 2. Serve the frontend

The frontend must be opened from a web origin (not `file://`) so `fetch` and CORS work reliably.

**Option A — VS Code Live Server:** Open `Frontend/index.html` → “Open with Live Server”.

**Option B — Python:**

```bash
cd Frontend
python -m http.server 5500
```

Open http://localhost:5500

**Option C — npx:**

```bash
cd Frontend
npx --yes serve -p 5500
```

If your API runs on a different port, edit `API_BASE` in `Frontend/script.js`.

## API endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/game/new` | Create a new game; returns `gameId` and empty board |
| `POST` | `/api/game/move` | Body: `{ "gameId", "row", "col" }` — player move + AI reply |
| `POST` | `/api/game/reset/{gameId}` | Clear the board for the same session |

### Example: new game

```json
{
  "gameId": "abc123...",
  "board": [["", "", ""], ["", "", ""], ["", "", ""]],
  "status": "InProgress",
  "message": "Your turn. You are X."
}
```

### Example: after a move

```json
{
  "gameId": "...",
  "board": [["X", "", ""], ["", "O", ""], ["", "", ""]],
  "status": "InProgress",
  "message": "Your turn.",
  "aiRow": 1,
  "aiCol": 1
}
```

`status` values: `InProgress`, `PlayerWin`, `AiWin`, `Draw`, `Invalid`.

## How the AI works

`AIPlayer` evaluates all eight winning lines:

1. **Win** — If two cells are `O` and one is empty, play the empty cell.
2. **Block** — If two cells are `X` and one is empty, play the empty cell.
3. **Random** — Otherwise pick a random free cell.

## Architecture notes

- **GameLogic** — Board state (in-memory per `gameId`), validation, win/draw detection, orchestrates human then AI turns.
- **AIPlayer** — Pure strategy; no HTTP knowledge.
- **GameController** — Thin REST layer returning JSON.
- **Frontend** — Renders the 3×3 grid, calls the API with `fetch()`, tracks local scoreboard, highlights the AI’s last move.

Games are stored in memory and are lost when the API restarts.

## Troubleshooting

| Issue | Fix |
|-------|-----|
| “Cannot reach API” | Run `dotnet run` in `Backend` and confirm port `5180` |
| CORS errors | API enables CORS for all origins in development |
| Wrong API URL | Update `API_BASE` in `script.js` |
