# Contributing to ai-game-dev

Thank you for your interest in contributing to this Unity C# game development repository!

## Getting Started

1. **Fork and Clone**: Fork this repository and clone it locally
2. **Create a Branch**: Create a feature branch for your changes
3. **Make Changes**: Follow the coding standards and guidelines
4. **Test**: Ensure all tests pass and add new tests as needed
5. **Submit PR**: Create a pull request with a clear description

## Creating Unity Projects

When creating a new Unity project in this repository:

1. Use the initialization script:
   ```bash
   .github/scripts/init-unity-project.sh YourProjectName
   ```

2. Open the project in Unity and configure settings:
   - Set Unity version (2021.3 LTS or later recommended)
   - Configure project settings (Quality, Physics, Input)
   - Add necessary Unity packages

3. Follow the project structure defined in [UNITY_PROJECT_TEMPLATE.md](UNITY_PROJECT_TEMPLATE.md)

## Coding Standards

### C# Naming Conventions

- **Classes**: `PascalCase` (e.g., `PlayerController`, `GameManager`)
- **Methods**: `PascalCase` (e.g., `MovePlayer()`, `TakeDamage()`)
- **Public Properties**: `PascalCase` (e.g., `Health`, `MoveSpeed`)
- **Private/Local Variables**: `camelCase` (e.g., `playerHealth`, `moveSpeed`)
- **Private Fields**: `_camelCase` (e.g., `_velocity`, `_isGrounded`)
- **Constants**: `PascalCase` or `UPPER_CASE` (e.g., `MaxHealth`, `MAX_SPEED`)

### Unity Best Practices

1. **Component Caching**: Cache component references in `Awake()` or `Start()`, not in `Update()`
   ```csharp
   private Rigidbody _rigidbody;
   
   private void Awake()
   {
       _rigidbody = GetComponent<Rigidbody>();
   }
   ```

2. **Serialize Fields**: Use `[SerializeField]` for private fields that need Inspector visibility
   ```csharp
   [SerializeField] private float _moveSpeed = 5f;
   ```

3. **Component Dependencies**: Use `[RequireComponent]` to enforce component dependencies
   ```csharp
   [RequireComponent(typeof(Rigidbody))]
   public class PlayerController : MonoBehaviour
   ```

4. **Namespaces**: Use namespaces to organize code and prevent conflicts
   ```csharp
   namespace YourProject.Player
   {
       public class PlayerController : MonoBehaviour
       {
           // ...
       }
   }
   ```

5. **XML Documentation**: Document public APIs with XML comments
   ```csharp
   /// <summary>
   /// Moves the player in the specified direction.
   /// </summary>
   /// <param name="direction">The movement direction</param>
   public void Move(Vector3 direction)
   {
       // ...
   }
   ```

## Code Organization

### Folder Structure

Organize scripts by feature/responsibility:

```
Assets/
└── Scripts/
    ├── Core/          # Core game systems
    ├── Managers/      # Singleton managers
    ├── Player/        # Player-specific code
    ├── Enemies/       # Enemy AI and behavior
    ├── UI/            # User interface
    ├── Utilities/     # Helper/utility scripts
    └── Data/          # ScriptableObjects
```

### File Naming

- C# scripts: `PascalCase.cs` (e.g., `PlayerController.cs`)
- Scene files: `PascalCase.unity` (e.g., `MainMenu.unity`)
- Prefabs: `PascalCase.prefab` (e.g., `EnemySpawner.prefab`)

## Testing

### Writing Tests

1. Place tests in `Assets/Tests/`
   - `EditMode/` for non-MonoBehaviour tests
   - `PlayMode/` for gameplay tests

2. Use descriptive test names:
   ```csharp
   [Test]
   public void PlayerController_Jump_IncreasesVerticalVelocity()
   {
       // Arrange
       var player = CreateTestPlayer();
       
       // Act
       player.Jump();
       
       // Assert
       Assert.Greater(player.Velocity.y, 0);
   }
   ```

3. Follow the Arrange-Act-Assert pattern

### Running Tests

Run tests using Unity Test Runner:
- Window → General → Test Runner
- Select EditMode or PlayMode
- Click "Run All" or "Run Selected"

## Git Workflow

### Branches

- `main`: Stable, production-ready code
- `develop`: Integration branch for features
- `feature/feature-name`: Individual features
- `bugfix/bug-description`: Bug fixes

### Commit Messages

Write clear, descriptive commit messages:

```
Add player jump mechanic

- Implement jump input handling
- Add ground check using raycast
- Configure jump force in inspector
```

Format:
- First line: Brief summary (50 chars or less)
- Blank line
- Detailed description if needed

### Pull Requests

1. **Title**: Clear, descriptive title
2. **Description**: 
   - What changes were made
   - Why these changes were necessary
   - Any relevant issue numbers
3. **Testing**: Describe how changes were tested
4. **Screenshots**: Include for visual changes

## Unity-Specific Guidelines

### Scene Management

- Keep scenes organized and documented
- Use empty GameObjects as folders in hierarchy
- Name objects descriptively
- Use prefabs for reusable objects

### Performance

- Avoid `GetComponent()` in `Update()` loops
- Use object pooling for frequently instantiated objects
- Profile before optimizing (Unity Profiler)
- Cache transform/component references

### Assets

- Use consistent naming for assets
- Organize assets in appropriate folders
- Optimize textures and audio for target platforms
- Use Addressables for large projects

### Version Control

- Commit only source files and text-based assets
- Binary assets should use Git LFS
- Include `ProjectSettings/` in commits
- Never commit `Library/`, `Temp/`, or `Obj/` folders

## Documentation

Update documentation when:
- Adding new systems or features
- Changing existing APIs
- Adding new Unity packages
- Modifying project structure

Required documentation:
- README.md for each Unity project
- XML comments for public APIs
- Code comments for complex algorithms
- Setup instructions for special requirements

## Questions?

If you have questions:
1. Check existing documentation first
2. Review [copilot-instructions.md](.github/copilot-instructions.md)
3. Look at existing code for examples
4. Open an issue for discussion

## Code Review

All contributions will be reviewed for:
- Code quality and style
- Unity best practices
- Test coverage
- Documentation
- Performance considerations

Thank you for contributing!
