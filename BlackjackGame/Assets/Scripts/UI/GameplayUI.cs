using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlackjackGame.Managers;
using BlackjackGame.Core;
using BlackjackGame.Data;

namespace BlackjackGame.UI
{
    /// <summary>
    /// Manages the gameplay UI
    /// </summary>
    public class GameplayUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button _hitButton;
        [SerializeField] private Button _standButton;
        [SerializeField] private TextMeshProUGUI _player1ScoreText;
        [SerializeField] private TextMeshProUGUI _player2ScoreText;
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TextMeshProUGUI _turnIndicatorText;
        [SerializeField] private GameObject _gameplayPanel;
        [SerializeField] private Transform _player1CardsContainer;
        [SerializeField] private Transform _player2CardsContainer;
        [SerializeField] private GameObject _cardPrefab;
        
        private void Start()
        {
            SetupButtons();
            SubscribeToEvents();
            HideGameplay();
        }
        
        private void SetupButtons()
        {
            if (_hitButton != null)
            {
                _hitButton.onClick.AddListener(OnHitClicked);
            }
            
            if (_standButton != null)
            {
                _standButton.onClick.AddListener(OnStandClicked);
            }
        }
        
        private void SubscribeToEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += OnGameStart;
                GameManager.Instance.OnGameEnd += OnGameEnd;
                GameManager.Instance.OnGameMessage += OnGameMessage;
            }
        }
        
        private void OnGameStart()
        {
            ShowGameplay();
            UpdateUI();
        }
        
        private void OnGameEnd()
        {
            DisableButtons();
        }
        
        private void OnGameMessage(string message)
        {
            if (_messageText != null)
            {
                _messageText.text = message;
            }
        }
        
        private void OnHitClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PlayerHit();
                UpdateUI();
            }
        }
        
        private void OnStandClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PlayerStand();
                UpdateUI();
            }
        }
        
        private void Update()
        {
            UpdateUI();
            UpdateButtonStates();
        }
        
        private void UpdateUI()
        {
            if (GameManager.Instance?.BlackjackGame == null)
                return;
            
            BlackjackGame game = GameManager.Instance.BlackjackGame;
            
            // Update scores
            if (_player1ScoreText != null)
            {
                _player1ScoreText.text = $"Player 1: {game.Player1Hand.GetValue()}";
            }
            
            if (_player2ScoreText != null)
            {
                _player2ScoreText.text = $"Player 2: {game.Player2Hand.GetValue()}";
            }
            
            // Update turn indicator
            if (_turnIndicatorText != null)
            {
                int currentPlayer = game.GetCurrentPlayer();
                if (currentPlayer > 0)
                {
                    bool isLocalTurn = GameManager.Instance.IsLocalPlayerTurn;
                    _turnIndicatorText.text = isLocalTurn ? "Your Turn" : "Opponent's Turn";
                }
                else
                {
                    _turnIndicatorText.text = "";
                }
            }
        }
        
        private void UpdateButtonStates()
        {
            bool canAct = GameManager.Instance != null && 
                          GameManager.Instance.IsLocalPlayerTurn &&
                          GameManager.Instance.CurrentState == GameManager.GameState.Playing;
            
            if (_hitButton != null)
            {
                _hitButton.interactable = canAct;
            }
            
            if (_standButton != null)
            {
                _standButton.interactable = canAct;
            }
        }
        
        private void DisableButtons()
        {
            if (_hitButton != null)
            {
                _hitButton.interactable = false;
            }
            
            if (_standButton != null)
            {
                _standButton.interactable = false;
            }
        }
        
        public void ShowGameplay()
        {
            if (_gameplayPanel != null)
            {
                _gameplayPanel.SetActive(true);
            }
        }
        
        public void HideGameplay()
        {
            if (_gameplayPanel != null)
            {
                _gameplayPanel.SetActive(false);
            }
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart -= OnGameStart;
                GameManager.Instance.OnGameEnd -= OnGameEnd;
                GameManager.Instance.OnGameMessage -= OnGameMessage;
            }
        }
    }
}
