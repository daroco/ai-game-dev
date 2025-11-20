# Unity C# Code Templates

Common code templates for Unity development. Use these as starting points for new scripts.

## MonoBehaviour Scripts

### Basic MonoBehaviour
```csharp
using UnityEngine;

namespace YourProject
{
    public class NewScript : MonoBehaviour
    {
        [SerializeField] private float _exampleValue = 1.0f;
        
        private void Awake()
        {
            // Initialize components
        }
        
        private void Start()
        {
            // Initialize after all objects are ready
        }
        
        private void Update()
        {
            // Per-frame logic
        }
    }
}
```

### Singleton Manager
```csharp
using UnityEngine;

namespace YourProject.Managers
{
    public class SingletonManager : MonoBehaviour
    {
        public static SingletonManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        
        private void Initialize()
        {
            // Setup manager
        }
    }
}
```

### Player Controller (2D)
```csharp
using UnityEngine;

namespace YourProject.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController2D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;
        
        [Header("Ground Check")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;
        
        private Rigidbody2D _rigidbody;
        private bool _isGrounded;
        private float _horizontalInput;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
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
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                Jump();
            }
        }
        
        private void Move()
        {
            _rigidbody.velocity = new Vector2(_horizontalInput * _moveSpeed, _rigidbody.velocity.y);
        }
        
        private void Jump()
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
        }
        
        private void CheckGround()
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        }
    }
}
```

### Player Controller (3D)
```csharp
using UnityEngine;

namespace YourProject.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController3D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _turnSmoothTime = 0.1f;
        
        [Header("Gravity")]
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _jumpHeight = 3f;
        
        [Header("Ground Check")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundDistance = 0.4f;
        [SerializeField] private LayerMask _groundMask;
        
        private CharacterController _controller;
        private Transform _camera;
        private Vector3 _velocity;
        private bool _isGrounded;
        private float _turnSmoothVelocity;
        
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _camera = Camera.main.transform;
        }
        
        private void Update()
        {
            CheckGround();
            HandleMovement();
            ApplyGravity();
            HandleJump();
        }
        
        private void CheckGround()
        {
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
            
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        }
        
        private void HandleMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _controller.Move(moveDir.normalized * _moveSpeed * Time.deltaTime);
            }
        }
        
        private void ApplyGravity()
        {
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
        
        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }
        }
    }
}
```

## Data Classes

### ScriptableObject Data
```csharp
using UnityEngine;

namespace YourProject.Data
{
    [CreateAssetMenu(fileName = "NewData", menuName = "Game/Data")]
    public class GameData : ScriptableObject
    {
        [Header("Basic Info")]
        public string dataName;
        [TextArea(3, 5)]
        public string description;
        
        [Header("Values")]
        public int intValue;
        public float floatValue;
        public bool boolValue;
    }
}
```

### Configuration ScriptableObject
```csharp
using UnityEngine;

namespace YourProject.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Player Settings")]
        public float playerSpeed = 5f;
        public float playerJumpForce = 10f;
        public int playerMaxHealth = 100;
        
        [Header("Enemy Settings")]
        public float enemySpeed = 3f;
        public int enemyDamage = 10;
        
        [Header("Game Settings")]
        public int maxLevel = 10;
        public float difficultyMultiplier = 1.1f;
    }
}
```

### Item Data
```csharp
using UnityEngine;

namespace YourProject.Data
{
    public enum ItemType
    {
        Consumable,
        Weapon,
        Armor,
        Quest,
        Misc
    }
    
    [CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
    public class ItemData : ScriptableObject
    {
        [Header("Basic Info")]
        public string itemName;
        public ItemType itemType;
        [TextArea(3, 5)]
        public string description;
        public Sprite icon;
        
        [Header("Properties")]
        public int value;
        public float weight;
        public bool isStackable;
        public int maxStackSize = 99;
    }
}
```

## Interface Examples

### Damageable Interface
```csharp
using UnityEngine;

namespace YourProject.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        void Die();
        float CurrentHealth { get; }
        float MaxHealth { get; }
    }
}
```

### Interactable Interface
```csharp
using UnityEngine;

namespace YourProject.Interfaces
{
    public interface IInteractable
    {
        void Interact(GameObject interactor);
        string GetInteractionPrompt();
        bool CanInteract(GameObject interactor);
    }
}
```

### Implementation Example
```csharp
using UnityEngine;
using YourProject.Interfaces;

namespace YourProject.Entities
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _maxHealth = 100f;
        private float _currentHealth;
        
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        
        private void Awake()
        {
            _currentHealth = _maxHealth;
        }
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
```

## Event Systems

### Event Channel (ScriptableObject Event)
```csharp
using UnityEngine;
using UnityEngine.Events;

namespace YourProject.Events
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Game/Event Channel")]
    public class GameEvent : ScriptableObject
    {
        private event UnityAction _listeners;
        
        public void Raise()
        {
            _listeners?.Invoke();
        }
        
        public void RegisterListener(UnityAction listener)
        {
            _listeners += listener;
        }
        
        public void UnregisterListener(UnityAction listener)
        {
            _listeners -= listener;
        }
    }
}
```

### Event Listener
```csharp
using UnityEngine;
using UnityEngine.Events;
using YourProject.Events;

namespace YourProject.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent _event;
        [SerializeField] private UnityEvent _response;
        
        private void OnEnable()
        {
            _event.RegisterListener(OnEventRaised);
        }
        
        private void OnDisable()
        {
            _event.UnregisterListener(OnEventRaised);
        }
        
        private void OnEventRaised()
        {
            _response?.Invoke();
        }
    }
}
```

## Utility Scripts

### Object Pool
```csharp
using System.Collections.Generic;
using UnityEngine;

namespace YourProject.Utilities
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _initialSize = 10;
        
        private Queue<GameObject> _pool = new Queue<GameObject>();
        
        private void Start()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                CreateNewObject();
            }
        }
        
        private GameObject CreateNewObject()
        {
            GameObject obj = Instantiate(_prefab);
            obj.SetActive(false);
            _pool.Enqueue(obj);
            return obj;
        }
        
        public GameObject Get()
        {
            GameObject obj = _pool.Count > 0 ? _pool.Dequeue() : CreateNewObject();
            obj.SetActive(true);
            return obj;
        }
        
        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
```

### Singleton Base Class
```csharp
using UnityEngine;

namespace YourProject.Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        
                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject(typeof(T).Name);
                            _instance = singleton.AddComponent<T>();
                            DontDestroyOnLoad(singleton);
                        }
                    }
                    
                    return _instance;
                }
            }
        }
        
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}
```

## Testing Templates

### Edit Mode Test
```csharp
using NUnit.Framework;
using UnityEngine;
using YourProject.Data;

namespace YourProject.Tests
{
    public class DataTests
    {
        [Test]
        public void ItemData_DefaultValues_AreValid()
        {
            // Arrange
            var item = ScriptableObject.CreateInstance<ItemData>();
            
            // Act & Assert
            Assert.IsNotNull(item);
            Assert.AreEqual(99, item.maxStackSize);
        }
        
        [Test]
        public void ItemData_SetName_UpdatesCorrectly()
        {
            // Arrange
            var item = ScriptableObject.CreateInstance<ItemData>();
            string expectedName = "Test Item";
            
            // Act
            item.itemName = expectedName;
            
            // Assert
            Assert.AreEqual(expectedName, item.itemName);
        }
    }
}
```

### Play Mode Test
```csharp
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YourProject.Player;

namespace YourProject.Tests
{
    public class PlayerTests
    {
        private GameObject _playerObject;
        private PlayerController _controller;
        
        [SetUp]
        public void Setup()
        {
            _playerObject = new GameObject("TestPlayer");
            _controller = _playerObject.AddComponent<PlayerController>();
            _playerObject.AddComponent<Rigidbody>();
        }
        
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(_playerObject);
        }
        
        [UnityTest]
        public IEnumerator Player_Instantiates_Successfully()
        {
            yield return null;
            Assert.IsNotNull(_controller);
        }
        
        [UnityTest]
        public IEnumerator Player_HasRequiredComponents()
        {
            yield return null;
            Assert.IsNotNull(_playerObject.GetComponent<Rigidbody>());
        }
    }
}
```

## Usage Notes

- Replace `YourProject` namespace with your actual project name
- Customize serialized fields for your specific needs
- Add XML documentation comments for public APIs
- Follow the coding standards in copilot-instructions.md
- Test all scripts before committing to version control
