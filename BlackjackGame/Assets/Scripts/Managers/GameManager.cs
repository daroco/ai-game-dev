using UnityEngine;
using System;
using BlackjackGame.Core;
using BlackjackGame.Data;

namespace BlackjackGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public event Action OnGameStart;
        public event Action OnGamePause;
        public event Action OnGameResume;
        public event Action OnGameEnd;
        public event Action<string> OnGameMessage;
        
        public enum GameState
        {
            MainMenu,
            Matchmaking,
            Playing,
            Paused,
            GameOver
        }
        
        private GameState _currentState;
        private BlackjackGame _blackjackGame;
        private int _localPlayerIndex;
        
        public GameState CurrentState
        {
            get => _currentState;
            private set
            {
                _currentState = value;
                OnStateChanged(value);
            }
        }
        
        public BlackjackGame BlackjackGame => _blackjackGame;
        public int LocalPlayerIndex => _localPlayerIndex;
        public bool IsLocalPlayerTurn => _blackjackGame?.GetCurrentPlayer() == _localPlayerIndex;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeGame();
        }
        
        private void InitializeGame()
        {
            _blackjackGame = new BlackjackGame();
            _blackjackGame.OnStateChanged += OnBlackjackStateChanged;
            _blackjackGame.OnCardDealt += OnCardDealt;
            _blackjackGame.OnGameEnded += OnBlackjackGameEnded;
            
            CurrentState = GameState.MainMenu;
        }
        
        public void StartMatchmaking()
        {
            CurrentState = GameState.Matchmaking;
            
            if (MatchmakingManager.Instance != null)
            {
                MatchmakingManager.Instance.OnMatchFound += OnMatchFound;
                MatchmakingManager.Instance.QuickMatch();
            }
        }
        
        private void OnMatchFound()
        {
            // Determine player index based on network role
            if (NetworkManager.Instance != null)
            {
                _localPlayerIndex = NetworkManager.Instance.IsHost ? 1 : 2;
                Debug.Log($"Local player is Player {_localPlayerIndex}");
            }
            else
            {
                _localPlayerIndex = 1; // Default for testing
            }
            
            StartGame();
        }
        
        public void StartGame()
        {
            CurrentState = GameState.Playing;
            _blackjackGame.StartNewRound();
            OnGameStart?.Invoke();
            
            SendGameMessage("Game started! Good luck!");
        }
        
        public void PauseGame()
        {
            CurrentState = GameState.Paused;
            Time.timeScale = 0f;
            OnGamePause?.Invoke();
        }
        
        public void ResumeGame()
        {
            CurrentState = GameState.Playing;
            Time.timeScale = 1f;
            OnGameResume?.Invoke();
        }
        
        public void EndGame()
        {
            CurrentState = GameState.GameOver;
            OnGameEnd?.Invoke();
        }
        
        public void PlayerHit()
        {
            if (!IsLocalPlayerTurn)
            {
                Debug.LogWarning("Not your turn!");
                return;
            }
            
            _blackjackGame.Hit(_localPlayerIndex);
            
            // Send action to network
            if (NetworkManager.Instance != null && NetworkManager.Instance.IsConnected)
            {
                var message = new NetworkMessage(NetworkMessage.MessageType.PlayerAction, $"Hit:{_localPlayerIndex}");
                NetworkManager.Instance.SendMessage(message);
            }
        }
        
        public void PlayerStand()
        {
            if (!IsLocalPlayerTurn)
            {
                Debug.LogWarning("Not your turn!");
                return;
            }
            
            _blackjackGame.Stand(_localPlayerIndex);
            
            // Send action to network
            if (NetworkManager.Instance != null && NetworkManager.Instance.IsConnected)
            {
                var message = new NetworkMessage(NetworkMessage.MessageType.PlayerAction, $"Stand:{_localPlayerIndex}");
                NetworkManager.Instance.SendMessage(message);
            }
        }
        
        private void OnBlackjackStateChanged(BlackjackGame.GameState newState)
        {
            Debug.Log($"Blackjack state changed to: {newState}");
            
            string message = newState switch
            {
                BlackjackGame.GameState.Player1Turn => "Player 1's turn",
                BlackjackGame.GameState.Player2Turn => "Player 2's turn",
                BlackjackGame.GameState.ShowingResults => "Revealing results...",
                _ => ""
            };
            
            if (!string.IsNullOrEmpty(message))
            {
                SendGameMessage(message);
            }
        }
        
        private void OnCardDealt(int playerIndex, Card card)
        {
            Debug.Log($"Player {playerIndex} dealt: {card}");
            SendGameMessage($"Player {playerIndex} received {card}");
        }
        
        private void OnBlackjackGameEnded(BlackjackGame.GameResult result, int player1Score, int player2Score)
        {
            string resultMessage = result switch
            {
                BlackjackGame.GameResult.Player1Wins => $"Player 1 wins! ({player1Score} vs {player2Score})",
                BlackjackGame.GameResult.Player2Wins => $"Player 2 wins! ({player2Score} vs {player1Score})",
                BlackjackGame.GameResult.Push => $"Push! Both players have {player1Score}",
                _ => "Game ended"
            };
            
            SendGameMessage(resultMessage);
            EndGame();
        }
        
        private void SendGameMessage(string message)
        {
            OnGameMessage?.Invoke(message);
        }
        
        private void OnStateChanged(GameState newState)
        {
            Debug.Log($"Game state changed to: {newState}");
        }
        
        private void OnDestroy()
        {
            if (_blackjackGame != null)
            {
                _blackjackGame.OnStateChanged -= OnBlackjackStateChanged;
                _blackjackGame.OnCardDealt -= OnCardDealt;
                _blackjackGame.OnGameEnded -= OnBlackjackGameEnded;
            }
        }
    }
}
