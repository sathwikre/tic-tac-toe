//const API_BASE = "http://localhost:5180/api/game";
const API_BASE = "https://tic-tac-toe-hxlx.onrender.com";

const scores = { x: 0, o: 0, draws: 0 };
let gameId = null;
let gameOver = false;
let isBusy = false;
let currentPlayer = "X";

const boardEl = document.getElementById("board");
const statusEl = document.getElementById("status");
const restartBtn = document.getElementById("restart-btn");
const xWinsEl = document.getElementById("x-wins");
const oWinsEl = document.getElementById("o-wins");
const drawsEl = document.getElementById("draws");

function updateScoreboard() {
  xWinsEl.textContent = scores.x;
  oWinsEl.textContent = scores.o;
  drawsEl.textContent = scores.draws;
}

function renderBoard(board) {
  boardEl.innerHTML = "";
  for (let row = 0; row < 3; row++) {
    for (let col = 0; col < 3; col++) {
      const value = board[row][col];
      const cell = document.createElement("button");
      cell.type = "button";
      cell.className = "cell";
      cell.setAttribute("role", "gridcell");
      cell.dataset.row = row;
      cell.dataset.col = col;

      if (value === "X") {
        cell.textContent = "X";
        cell.classList.add("x");
        cell.disabled = true;
      } else if (value === "O") {
        cell.textContent = "O";
        cell.classList.add("o");
        cell.disabled = true;
      } else {
        cell.textContent = "";
        // Only lock the board when the game is over.
        // In-flight requests are already blocked by the isBusy guard in onCellClick().
        cell.disabled = gameOver;
        cell.addEventListener("click", () => onCellClick(row, col));
      }

      boardEl.appendChild(cell);
    }
  }
}

function setStatus(message, tone = "default") {
  statusEl.textContent = message;
  statusEl.style.color =
    tone === "win" ? "#22c55e" : tone === "lose" ? "#f43f5e" : tone === "draw" ? "#8b9cb3" : "#22c55e";
}

async function api(path, options = {}) {
  const response = await fetch(`${API_BASE}${path}`, {
    headers: { "Content-Type": "application/json" },
    ...options,
  });

  if (!response.ok) {
    const err = await response.json().catch(() => ({}));
    throw new Error(err.message || `Request failed (${response.status})`);
  }

  return response.json();
}

async function startNewGame() {
  isBusy = true;
  gameOver = false;
  currentPlayer = "X";
  setStatus("Starting new game…");

  try {
    const data = await api("/new", { method: "POST" });
    gameId = data.gameId;
    renderBoard(data.board);
    setStatus("Player X's turn.");
  } catch (err) {
    setStatus("Cannot reach API. Start the backend on port 5180.", "lose");
    console.error(err);
  } finally {
    isBusy = false;
  }
}

async function onCellClick(row, col) {
  if (gameOver || isBusy || !gameId) return;

  isBusy = true;
  setStatus(`Player ${currentPlayer} moved…`);

  try {
    const data = await api("/move", {
      method: "POST",
      body: JSON.stringify({ gameId, row, col, player: currentPlayer }),
    });

    renderBoard(data.board);

    if (data.status === "XWin") {
      gameOver = true;
      scores.x++;
      updateScoreboard();
      setStatus(data.message || "Player X wins!", "win");
    } else if (data.status === "OWin") {
      gameOver = true;
      scores.o++;
      updateScoreboard();
      setStatus(data.message || "Player O wins!", "lose");
    } else if (data.status === "Draw") {
      gameOver = true;
      scores.draws++;
      updateScoreboard();
      setStatus(data.message, "draw");
    } else {
      currentPlayer = currentPlayer === "X" ? "O" : "X";
      setStatus(`Player ${currentPlayer}'s turn.`);
    }
  } catch (err) {
    setStatus(err.message || "Move failed.", "lose");
    console.error(err);
  } finally {
    isBusy = false;
  }
}

async function restartGame() {
  if (gameId) {
    try {
      await api(`/reset/${gameId}`, { method: "POST" });
    } catch {
      /* fall through to new game */
    }
  }
  await startNewGame();
}

restartBtn.addEventListener("click", restartGame);
updateScoreboard();
startNewGame();
