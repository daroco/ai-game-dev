using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackjackGame.Managers
{
    /// <summary>
    /// Manages peer-to-peer networking for the blackjack game
    /// Uses a simple message-based system for P2P communication
    /// </summary>
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance { get; private set; }
        
        public enum NetworkRole
        {
            None,
            Host,
            Client
        }
        
        [Header("Network Settings")]
        [SerializeField] private int _port = 7777;
        [SerializeField] private string _hostAddress = "127.0.0.1";
        
        private NetworkRole _currentRole;
        private bool _isConnected;
        private Queue<NetworkMessage> _messageQueue;
        
        public NetworkRole CurrentRole => _currentRole;
        public bool IsConnected => _isConnected;
        public bool IsHost => _currentRole == NetworkRole.Host;
        
        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<NetworkMessage> OnMessageReceived;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            _messageQueue = new Queue<NetworkMessage>();
        }
        
        /// <summary>
        /// Starts hosting a game session
        /// </summary>
        public void StartHost()
        {
            _currentRole = NetworkRole.Host;
            _isConnected = true;
            
            Debug.Log($"Started hosting on port {_port}");
            OnConnected?.Invoke();
        }
        
        /// <summary>
        /// Connects to a host as a client
        /// </summary>
        public void StartClient(string hostAddress = null)
        {
            _currentRole = NetworkRole.Client;
            _hostAddress = hostAddress ?? _hostAddress;
            
            // Simulate connection delay
            Invoke(nameof(CompleteClientConnection), 0.5f);
        }
        
        private void CompleteClientConnection()
        {
            _isConnected = true;
            Debug.Log($"Connected to host at {_hostAddress}:{_port}");
            OnConnected?.Invoke();
        }
        
        /// <summary>
        /// Disconnects from the current session
        /// </summary>
        public void Disconnect()
        {
            _isConnected = false;
            _currentRole = NetworkRole.None;
            _messageQueue.Clear();
            
            Debug.Log("Disconnected from network session");
            OnDisconnected?.Invoke();
        }
        
        /// <summary>
        /// Sends a message to the other player
        /// </summary>
        public void SendMessage(NetworkMessage message)
        {
            if (!_isConnected)
            {
                Debug.LogWarning("Cannot send message - not connected");
                return;
            }
            
            // In a real implementation, this would send over the network
            // For this example, we'll simulate instant delivery
            Debug.Log($"Sending message: {message.MessageType}");
            
            // Simulate network delay and delivery
            Invoke(nameof(SimulateMessageReceived), 0.1f);
        }
        
        private void SimulateMessageReceived()
        {
            // This would be triggered by actual network messages
            // For now, it's a placeholder for the P2P architecture
        }
        
        /// <summary>
        /// Processes received network messages
        /// </summary>
        private void Update()
        {
            while (_messageQueue.Count > 0)
            {
                NetworkMessage message = _messageQueue.Dequeue();
                OnMessageReceived?.Invoke(message);
            }
        }
        
        /// <summary>
        /// Simulates receiving a message (for testing)
        /// </summary>
        public void SimulateReceiveMessage(NetworkMessage message)
        {
            _messageQueue.Enqueue(message);
        }
    }
    
    /// <summary>
    /// Represents a network message in the P2P system
    /// </summary>
    [System.Serializable]
    public class NetworkMessage
    {
        public enum MessageType
        {
            GameStart,
            CardDealt,
            PlayerAction,
            GameEnd,
            KeepAlive
        }
        
        public MessageType Type;
        public string Data;
        
        public NetworkMessage(MessageType type, string data = "")
        {
            Type = type;
            Data = data;
        }
    }
}
