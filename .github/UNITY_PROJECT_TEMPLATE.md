# Unity Project Template Structure

This document describes the standard structure for Unity C# projects in this repository.

## Directory Structure

```
ProjectName/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/              # Core game systems
│   │   ├── Managers/          # Manager scripts (GameManager, AudioManager, etc.)
│   │   ├── Player/            # Player-related scripts
│   │   ├── Enemies/           # Enemy scripts
│   │   ├── UI/                # UI scripts
│   │   ├── Utilities/         # Helper and utility scripts
│   │   └── Data/              # ScriptableObjects and data classes
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── Game.unity
│   │   └── ...
│   ├── Prefabs/
│   │   ├── Player/
│   │   ├── Enemies/
│   │   ├── UI/
│   │   └── Environment/
│   ├── Materials/
│   ├── Textures/
│   ├── Audio/
│   │   ├── Music/
│   │   └── SFX/
│   ├── Animations/
│   ├── Fonts/
│   ├── Resources/             # Runtime-loaded assets
│   └── Tests/                 # Unit and integration tests
│       ├── EditMode/
│       └── PlayMode/
├── ProjectSettings/           # Unity project settings (commit these)
├── Packages/                  # Package Manager dependencies
└── README.md                  # Project-specific readme
```

## Essential Scripts Templates

### GameManager.cs
A central game manager to control game state and flow.

```csharp
using UnityEngine;
using System;

namespace GameName.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public event Action OnGameStart;
        public event Action OnGamePause;
        public event Action OnGameResume;
        public event Action OnGameEnd;
        
        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            GameOver
        }
        
        private GameState _currentState;
        public GameState CurrentState
        {
            get => _currentState;
            private set
            {
                _currentState = value;
                OnStateChanged(value);
            }
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public void StartGame()
        {
            CurrentState = GameState.Playing;
            OnGameStart?.Invoke();
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
        
        private void OnStateChanged(GameState newState)
        {
            Debug.Log($"Game state changed to: {newState}");
        }
    }
}
```

### PlayerController.cs
Basic player movement controller.

```csharp
using UnityEngine;

namespace GameName.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;
        
        [Header("Ground Check")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;
        
        private Rigidbody _rigidbody;
        private bool _isGrounded;
        private Vector3 _moveInput;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            HandleInput();
            CheckGround();
        }
        
        private void FixedUpdate()
        {
            Move();
        }
        
        private void HandleInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            _moveInput = new Vector3(horizontal, 0f, vertical).normalized;
            
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                Jump();
            }
        }
        
        private void Move()
        {
            Vector3 movement = _moveInput * _moveSpeed;
            _rigidbody.velocity = new Vector3(movement.x, _rigidbody.velocity.y, movement.z);
        }
        
        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
        
        private void CheckGround()
        {
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
            }
        }
    }
}
```

### ScriptableObject Data Template

```csharp
using UnityEngine;

namespace GameName.Data
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Game/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Basic Info")]
        public string itemName;
        [TextArea(3, 5)]
        public string description;
        public Sprite icon;
        
        [Header("Stats")]
        public int value;
        public float weight;
        
        [Header("Flags")]
        public bool isStackable;
        public int maxStackSize = 99;
    }
}
```

## Assembly Definitions

For better compile times, create assembly definitions:

**Assets/Scripts/Scripts.asmdef**
```json
{
    "name": "GameName.Scripts",
    "rootNamespace": "GameName",
    "references": [],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "overrideReferences": false,
    "precompiledReferences": [],
    "autoReferenced": true,
    "defineConstraints": [],
    "versionDefines": [],
    "noEngineReferences": false
}
```

**Assets/Tests/Tests.asmdef**
```json
{
    "name": "GameName.Tests",
    "rootNamespace": "GameName.Tests",
    "references": [
        "UnityEngine.TestRunner",
        "UnityEditor.TestRunner",
        "GameName.Scripts"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "overrideReferences": true,
    "precompiledReferences": [
        "nunit.framework.dll"
    ],
    "autoReferenced": false,
    "defineConstraints": [
        "UNITY_INCLUDE_TESTS"
    ],
    "versionDefines": [],
    "noEngineReferences": false
}
```

## Testing Setup

### Edit Mode Test Example
```csharp
using NUnit.Framework;
using GameName.Data;

namespace GameName.Tests
{
    public class ItemDataTests
    {
        [Test]
        public void ItemData_DefaultValues_AreValid()
        {
            var itemData = ScriptableObject.CreateInstance<ItemData>();
            Assert.IsNotNull(itemData);
            Assert.AreEqual(99, itemData.maxStackSize);
        }
    }
}
```

### Play Mode Test Example
```csharp
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GameName.Player;

namespace GameName.Tests
{
    public class PlayerControllerTests
    {
        [UnityTest]
        public IEnumerator PlayerController_Instantiates_Successfully()
        {
            var playerObject = new GameObject("Player");
            var controller = playerObject.AddComponent<PlayerController>();
            playerObject.AddComponent<Rigidbody>();
            
            yield return null;
            
            Assert.IsNotNull(controller);
            Object.Destroy(playerObject);
        }
    }
}
```

## Project Settings Checklist

When setting up a new Unity project:

1. **Quality Settings**: Configure quality levels for different platforms
2. **Physics Settings**: Set appropriate collision layers and physics parameters
3. **Input Manager**: Define input axes (or use new Input System)
4. **Tags and Layers**: Set up custom tags and layers
5. **Player Settings**: Configure company name, product name, and version
6. **Build Settings**: Add scenes and configure platform-specific settings
7. **Time Settings**: Set fixed timestep if needed

## Recommended Packages

Essential Unity packages to consider:
- **TextMeshPro**: For better text rendering
- **Cinemachine**: For advanced camera control
- **New Input System**: For modern input handling
- **ProBuilder**: For level design prototyping
- **Post Processing**: For visual effects
- **Unity Test Framework**: For testing (already included)

## Best Practices

1. Use meaningful scene names and organize them logically
2. Create prefabs for all reusable objects
3. Use layers and tags appropriately
4. Keep scene hierarchy organized with empty game objects as folders
5. Use consistent naming conventions across all assets
6. Document complex systems with XML comments
7. Set up continuous integration for automated testing
8. Use version control for all text-based assets
