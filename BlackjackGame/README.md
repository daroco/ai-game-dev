# BlackjackGame

A 1v1 peer-to-peer blackjack game built with Unity.

## Features

- **1v1 Multiplayer**: Play blackjack against another player
- **Peer-to-Peer Matchmaking**: Quick match system to find opponents
- **Classic Blackjack Rules**: 
  - Goal: Get as close to 21 as possible without going over
  - Face cards count as 10, Aces count as 1 or 11
  - Players take turns hitting or standing
  - Winner is determined by comparing hand values

## Project Structure

- **Assets/Scripts/Core/** - Core game logic (Deck, Hand, BlackjackGame)
- **Assets/Scripts/Data/** - Data models (Card, CardData)
- **Assets/Scripts/Managers/** - Game systems (GameManager, NetworkManager, MatchmakingManager)
- **Assets/Scripts/UI/** - UI components (MainMenuUI, GameplayUI, MatchmakingUI)
- **Assets/Tests/** - Unit and integration tests

## Getting Started

1. Open this project in Unity (2021.3 LTS or later recommended)
2. Open the main scene in Assets/Scenes/
3. Press Play to test
4. Click "Quick Match" to start a game

## How to Play

1. **Main Menu**: Choose from:
   - Quick Match: Automatically find and join a game
   - Host Game: Create a new game and wait for opponent
   - Join Game: Connect to an existing game

2. **Gameplay**:
   - Wait for your turn (indicated by "Your Turn" message)
   - Click "Hit" to draw another card
   - Click "Stand" to end your turn
   - Try to get closer to 21 than your opponent without going over

3. **Winning**:
   - Highest hand value without going over 21 wins
   - Going over 21 is a "bust" and you lose
   - If both players have the same value, it's a "push" (tie)
   - Blackjack (21 with 2 cards) beats a regular 21

## Architecture

### Core Systems

- **BlackjackGame**: Core game logic and rules
- **Deck**: Card deck management with shuffle
- **Hand**: Player hand with value calculation
- **Card**: Card data structure with suit and rank

### Networking

- **NetworkManager**: P2P connection management
- **MatchmakingManager**: Player matchmaking and lobby system
- **NetworkMessage**: Message protocol for game state sync

### UI Components

- **MainMenuUI**: Main menu navigation
- **GameplayUI**: In-game controls and display
- **MatchmakingUI**: Matchmaking status and controls

## Development

See the [Unity Project Template](/home/runner/work/ai-game-dev/ai-game-dev/.github/UNITY_PROJECT_TEMPLATE.md) for coding standards and best practices.

## Testing

Run tests using Unity Test Runner:
- Window -> General -> Test Runner
- Select EditMode or PlayMode tabs
- Click "Run All"

Test coverage includes:
- Card value calculations
- Hand evaluation (blackjack, bust detection)
- Deck shuffling and drawing
- Game state management

## Build

Build settings are configured in Unity:
- File -> Build Settings
- Select target platform
- Click "Build" or "Build and Run"

## Future Enhancements

- Visual card representations
- Sound effects and music
- Betting system
- Tournament mode
- Player statistics and leaderboards
- Advanced matchmaking with skill ratings
