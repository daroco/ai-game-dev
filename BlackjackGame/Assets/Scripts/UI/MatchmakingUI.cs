using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlackjackGame.Managers;

namespace BlackjackGame.UI
{
    /// <summary>
    /// Manages the matchmaking UI
    /// </summary>
    public class MatchmakingUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject _matchmakingPanel;
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Image _loadingSpinner;
        
        private float _spinnerRotation;
        
        private void Start()
        {
            if (_cancelButton != null)
            {
                _cancelButton.onClick.AddListener(OnCancelClicked);
            }
            
            SubscribeToEvents();
            HideMatchmaking();
        }
        
        private void SubscribeToEvents()
        {
            if (MatchmakingManager.Instance != null)
            {
                MatchmakingManager.Instance.OnSearchStatusChanged += UpdateStatus;
                MatchmakingManager.Instance.OnMatchFound += OnMatchFound;
            }
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += OnGameStart;
            }
        }
        
        private void Update()
        {
            if (_loadingSpinner != null && _matchmakingPanel != null && _matchmakingPanel.activeSelf)
            {
                _spinnerRotation -= 180f * Time.deltaTime;
                _loadingSpinner.transform.rotation = Quaternion.Euler(0, 0, _spinnerRotation);
            }
        }
        
        private void UpdateStatus(string status)
        {
            if (_statusText != null)
            {
                _statusText.text = status;
            }
            
            ShowMatchmaking();
        }
        
        private void OnMatchFound()
        {
            if (_statusText != null)
            {
                _statusText.text = "Match found! Connecting...";
            }
        }
        
        private void OnGameStart()
        {
            HideMatchmaking();
        }
        
        private void OnCancelClicked()
        {
            if (MatchmakingManager.Instance != null)
            {
                MatchmakingManager.Instance.CancelMatchmaking();
            }
            
            HideMatchmaking();
        }
        
        public void ShowMatchmaking()
        {
            if (_matchmakingPanel != null)
            {
                _matchmakingPanel.SetActive(true);
            }
        }
        
        public void HideMatchmaking()
        {
            if (_matchmakingPanel != null)
            {
                _matchmakingPanel.SetActive(false);
            }
        }
        
        private void OnDestroy()
        {
            if (MatchmakingManager.Instance != null)
            {
                MatchmakingManager.Instance.OnSearchStatusChanged -= UpdateStatus;
                MatchmakingManager.Instance.OnMatchFound -= OnMatchFound;
            }
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart -= OnGameStart;
            }
        }
    }
}
