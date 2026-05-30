# Tic-Tac-Toe Web Application

## Project Overview

This project is a simple Tic-Tac-Toe web application built using ASP.NET Core Web API for the backend and HTML, CSS, and JavaScript for the frontend.

The application allows two players (X and O) to play Tic-Tac-Toe on the same device. The backend manages the game state, validates moves, checks win/draw conditions, and returns the updated board to the frontend through REST APIs.

## Technologies Used

### Backend

* C#
* ASP.NET Core Web API
* Swagger

### Frontend

* HTML
* CSS
* JavaScript

## Project Structure

```text
Backend/
├── Controllers/
│   └── GameController.cs
├── Models/
│   ├── MoveRequest.cs
│   └── GameResponse.cs
├── Services/
│   └── GameLogic.cs
├── Program.cs
└── appsettings.json

Frontend/
├── index.html
├── style.css
└── script.js
```

## Features

* Start a new game
* Two-player gameplay (X and O)
* Move validation
* Win detection
* Draw detection
* Reset game functionality
* REST API integration
* Responsive web interface

## How to Run the Project

### 1. Start the Backend API

Open a terminal and navigate to the Backend folder:

```powershell
cd c:\deep\tictaktoi\Backend
dotnet run
```

After the application starts, you should see an output similar to:

```text
Now listening on: http://localhost:5180
```

### 2. Start the Frontend

Open another terminal and navigate to the Frontend folder:

```powershell
cd c:\deep\tictaktoi\Frontend
python -m http.server 5501
```

If Python is installed using the Windows launcher:

```powershell
py -m http.server 5501
```

### 3. Open the Application

Open the following URL in your browser:

```text
http://127.0.0.1:5501/
```

The game board should appear and be ready to play.

## API Endpoints

### Create New Game

```http
POST /api/game/new
```

Creates a new game and returns a unique Game ID.

### Make a Move

```http
POST /api/game/move
```

Request Body:

```json
{
  "gameId": "abc123",
  "row": 0,
  "col": 1,
  "player": "X"
}
```

### Reset a Game

```http
POST /api/game/reset/{gameId}
```

Resets the board while keeping the same Game ID.

## Game Status Values

The API can return the following status values:

* InProgress
* XWin
* OWin
* Draw
* Invalid

## Swagger Documentation

Swagger is available at:

```text
http://localhost:5180/swagger
```

It can be used to test API endpoints directly from the browser.

## Common Issues

### Frontend loads but moves are not working

Ensure that the backend API is running before opening the frontend.

### Unable to connect to the API

Verify that the backend is running on the expected port and that the API URL configured in `script.js` matches the backend URL.

### Build errors related to locked files

Stop any previously running API processes and rebuild the application.

## Future Enhancements

* AI opponent
* Score tracking
* Persistent game storage using a database
* Multiplayer support
* Improved UI and animations

## Requirements

* .NET 8 SDK
* Python (for serving frontend files)
* Modern web browser
