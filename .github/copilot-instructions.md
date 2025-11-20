# GitHub Copilot Instructions for Unity C# Projects

This repository is designed for creating and modifying Unity C# game development projects.

## Project Structure

Unity projects should follow this standard structure:
- `Assets/` - Contains all game assets, scripts, scenes, prefabs, materials, etc.
  - `Scripts/` - C# scripts for game logic
  - `Scenes/` - Unity scene files (.unity)
  - `Prefabs/` - Reusable game objects
  - `Materials/` - Material files
  - `Textures/` - Image files
  - `Audio/` - Sound effects and music
- `ProjectSettings/` - Unity project settings
- `Packages/` - Unity package manager dependencies

## C# Coding Standards

When creating or modifying Unity C# scripts:

1. **Naming Conventions**:
   - Classes: PascalCase (e.g., `PlayerController`, `GameManager`)
   - Methods: PascalCase (e.g., `MovePlayer()`, `TakeDamage()`)
   - Variables: camelCase (e.g., `playerHealth`, `moveSpeed`)
   - Constants: PascalCase or UPPER_CASE (e.g., `MaxHealth` or `MAX_HEALTH`)
   - Private fields: camelCase with underscore prefix (e.g., `_velocity`)

2. **MonoBehaviour Lifecycle**:
   - Use `Awake()` for initialization before Start()
   - Use `Start()` for initialization after all objects are created
   - Use `Update()` for per-frame logic
   - Use `FixedUpdate()` for physics-related updates
   - Use `OnDestroy()` for cleanup

3. **Component Architecture**:
   - Keep scripts focused and single-purpose
   - Use `[SerializeField]` for private fields that need Inspector visibility
   - Use `[RequireComponent]` to enforce dependencies
   - Cache component references in `Awake()` or `Start()`

4. **Performance Best Practices**:
   - Avoid `GetComponent()` calls in `Update()` - cache references
   - Use object pooling for frequently instantiated objects
   - Minimize allocations in update loops
   - Use `CompareTag()` instead of string comparison

5. **Code Organization**:
   - Group related functionality into folders
   - Use namespaces to prevent naming conflicts
   - Comment complex algorithms and non-obvious code
   - Use regions sparingly and only for large classes

## Common Unity Patterns

### Singleton Pattern
```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```

### Component Communication
- Prefer direct references over `FindObjectOfType()`
- Use events/delegates for loose coupling
- Use ScriptableObjects for data sharing

### Input Handling
- Use Unity's new Input System when possible
- Handle input in `Update()` for immediate response
- Use `Input.GetButtonDown()` for one-time actions

## File Templates

### Basic MonoBehaviour Script
```csharp
using UnityEngine;

public class NewScript : MonoBehaviour
{
    [SerializeField] private float _exampleFloat = 1.0f;
    
    private void Awake()
    {
        // Initialize before Start
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
```

### ScriptableObject
```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "Game/New Data")]
public class NewScriptableObject : ScriptableObject
{
    public string dataName;
    public int dataValue;
}
```

## Testing

- Use Unity Test Framework for unit and integration tests
- Place tests in `Assets/Tests/` directory
- Use EditMode tests for non-MonoBehaviour code
- Use PlayMode tests for gameplay logic

## Build Configuration

- Keep build settings in version control
- Use preprocessor directives for platform-specific code
- Document any custom build scripts or processes
- Use Addressables for efficient asset management in larger projects

## Version Control

- Use `.gitignore` to exclude Unity-generated files
- Commit only source assets and text-based files
- Use Git LFS for large binary assets
- Include `ProjectSettings/` in version control

## When Creating New Projects

1. Set up proper folder structure in `Assets/`
2. Configure project settings (Quality, Physics, Input, etc.)
3. Add appropriate .gitignore file
4. Set up assembly definitions for faster compilation
5. Configure code editor integration (VS Code, Visual Studio, Rider)

## Common Issues and Solutions

- **Missing References**: Ensure serialized fields are assigned in Inspector
- **Null Reference Exceptions**: Check initialization order and use null checks
- **Performance Issues**: Profile using Unity Profiler before optimizing
- **Build Errors**: Check for platform-specific issues and missing dependencies
