using UnityEngine;
using UnityEngine.UI;
using BlackjackGame.Managers;

namespace BlackjackGame.UI
{
    /// <summary>
    /// Manages the main menu UI
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button _quickMatchButton;
        [SerializeField] private Button _hostGameButton;
        [SerializeField] private Button _joinGameButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private GameObject _menuPanel;
        
        private void Start()
        {
            SetupButtons();
            ShowMenu();
        }
        
        private void SetupButtons()
        {
            if (_quickMatchButton != null)
            {
                _quickMatchButton.onClick.AddListener(OnQuickMatchClicked);
            }
            
            if (_hostGameButton != null)
            {
                _hostGameButton.onClick.AddListener(OnHostGameClicked);
            }
            
            if (_joinGameButton != null)
            {
                _joinGameButton.onClick.AddListener(OnJoinGameClicked);
            }
            
            if (_quitButton != null)
            {
                _quitButton.onClick.AddListener(OnQuitClicked);
            }
        }
        
        private void OnQuickMatchClicked()
        {
            Debug.Log("Quick Match clicked");
            HideMenu();
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartMatchmaking();
            }
        }
        
        private void OnHostGameClicked()
        {
            Debug.Log("Host Game clicked");
            HideMenu();
            
            if (MatchmakingManager.Instance != null)
            {
                MatchmakingManager.Instance.CreateGame();
            }
        }
        
        private void OnJoinGameClicked()
        {
            Debug.Log("Join Game clicked");
            HideMenu();
            
            if (MatchmakingManager.Instance != null)
            {
                // In a real implementation, this would show a server browser
                MatchmakingManager.Instance.JoinGame("127.0.0.1");
            }
        }
        
        private void OnQuitClicked()
        {
            Debug.Log("Quit clicked");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        
        public void ShowMenu()
        {
            if (_menuPanel != null)
            {
                _menuPanel.SetActive(true);
            }
        }
        
        public void HideMenu()
        {
            if (_menuPanel != null)
            {
                _menuPanel.SetActive(false);
            }
        }
    }
}
