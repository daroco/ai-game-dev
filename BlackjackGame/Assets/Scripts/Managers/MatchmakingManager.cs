using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackjackGame.Managers
{
    /// <summary>
    /// Manages peer-to-peer matchmaking for finding game opponents
    /// </summary>
    public class MatchmakingManager : MonoBehaviour
    {
        public static MatchmakingManager Instance { get; private set; }
        
        public enum MatchmakingState
        {
            Idle,
            Searching,
            MatchFound,
            Connected
        }
        
        [Header("Matchmaking Settings")]
        [SerializeField] private float _searchTimeout = 30f;
        
        private MatchmakingState _currentState;
        private float _searchStartTime;
        private List<string> _availableHosts;
        
        public MatchmakingState CurrentState => _currentState;
        
        public event Action OnMatchFound;
        public event Action OnSearchTimeout;
        public event Action<string> OnSearchStatusChanged;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            _availableHosts = new List<string>();
        }
        
        /// <summary>
        /// Starts searching for an opponent
        /// </summary>
        public void StartMatchmaking()
        {
            if (_currentState == MatchmakingState.Searching)
            {
                Debug.LogWarning("Already searching for match");
                return;
            }
            
            _currentState = MatchmakingState.Searching;
            _searchStartTime = Time.time;
            
            Debug.Log("Starting matchmaking...");
            OnSearchStatusChanged?.Invoke("Searching for opponent...");
            
            // Simulate finding available hosts
            RefreshAvailableHosts();
        }
        
        /// <summary>
        /// Cancels the current matchmaking search
        /// </summary>
        public void CancelMatchmaking()
        {
            if (_currentState != MatchmakingState.Searching)
                return;
            
            _currentState = MatchmakingState.Idle;
            Debug.Log("Matchmaking cancelled");
            OnSearchStatusChanged?.Invoke("Search cancelled");
        }
        
        /// <summary>
        /// Creates a new game session as host
        /// </summary>
        public void CreateGame()
        {
            NetworkManager.Instance.StartHost();
            _currentState = MatchmakingState.Connected;
            
            Debug.Log("Created new game session");
            OnSearchStatusChanged?.Invoke("Waiting for opponent to join...");
        }
        
        /// <summary>
        /// Joins an existing game session
        /// </summary>
        public void JoinGame(string hostAddress)
        {
            NetworkManager.Instance.StartClient(hostAddress);
            _currentState = MatchmakingState.MatchFound;
            
            Debug.Log($"Joining game at {hostAddress}");
            OnSearchStatusChanged?.Invoke("Connecting to opponent...");
            
            // Simulate successful connection
            Invoke(nameof(CompleteMatchmaking), 1f);
        }
        
        /// <summary>
        /// Simulates automatic matchmaking by joining first available host
        /// </summary>
        public void QuickMatch()
        {
            StartMatchmaking();
            
            // Simulate finding a match after a short delay
            Invoke(nameof(SimulateMatchFound), 2f);
        }
        
        private void SimulateMatchFound()
        {
            if (_currentState != MatchmakingState.Searching)
                return;
            
            _currentState = MatchmakingState.MatchFound;
            OnSearchStatusChanged?.Invoke("Match found! Connecting...");
            OnMatchFound?.Invoke();
            
            // Auto-join the match
            JoinGame("127.0.0.1");
        }
        
        private void CompleteMatchmaking()
        {
            _currentState = MatchmakingState.Connected;
            OnSearchStatusChanged?.Invoke("Connected! Starting game...");
        }
        
        /// <summary>
        /// Refreshes the list of available game hosts
        /// </summary>
        private void RefreshAvailableHosts()
        {
            // In a real implementation, this would query a matchmaking server
            // or use local network discovery
            _availableHosts.Clear();
            
            // Simulate finding some hosts
            _availableHosts.Add("127.0.0.1");
            
            Debug.Log($"Found {_availableHosts.Count} available hosts");
        }
        
        private void Update()
        {
            if (_currentState == MatchmakingState.Searching)
            {
                float elapsedTime = Time.time - _searchStartTime;
                
                // Check for timeout
                if (elapsedTime >= _searchTimeout)
                {
                    _currentState = MatchmakingState.Idle;
                    Debug.Log("Matchmaking search timed out");
                    OnSearchTimeout?.Invoke();
                    OnSearchStatusChanged?.Invoke("Search timed out. Please try again.");
                }
            }
        }
        
        /// <summary>
        /// Gets the list of available hosts
        /// </summary>
        public IReadOnlyList<string> GetAvailableHosts()
        {
            return _availableHosts.AsReadOnly();
        }
    }
}
