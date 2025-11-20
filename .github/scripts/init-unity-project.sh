#!/bin/bash
# Initialize Unity Project Structure
# Usage: ./init-unity-project.sh <project-name>

set -e

if [ -z "$1" ]; then
    echo "Usage: $0 <project-name>"
    echo "Example: $0 MyAwesomeGame"
    exit 1
fi

PROJECT_NAME="$1"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"
PROJECT_DIR="$REPO_ROOT/$PROJECT_NAME"

echo "Creating Unity project structure for: $PROJECT_NAME"
echo "Project directory: $PROJECT_DIR"

# Create main directories
mkdir -p "$PROJECT_DIR/Assets/Scripts/Core"
mkdir -p "$PROJECT_DIR/Assets/Scripts/Managers"
mkdir -p "$PROJECT_DIR/Assets/Scripts/Player"
mkdir -p "$PROJECT_DIR/Assets/Scripts/Enemies"
mkdir -p "$PROJECT_DIR/Assets/Scripts/UI"
mkdir -p "$PROJECT_DIR/Assets/Scripts/Utilities"
mkdir -p "$PROJECT_DIR/Assets/Scripts/Data"
mkdir -p "$PROJECT_DIR/Assets/Scenes"
mkdir -p "$PROJECT_DIR/Assets/Prefabs/Player"
mkdir -p "$PROJECT_DIR/Assets/Prefabs/Enemies"
mkdir -p "$PROJECT_DIR/Assets/Prefabs/UI"
mkdir -p "$PROJECT_DIR/Assets/Prefabs/Environment"
mkdir -p "$PROJECT_DIR/Assets/Materials"
mkdir -p "$PROJECT_DIR/Assets/Textures"
mkdir -p "$PROJECT_DIR/Assets/Audio/Music"
mkdir -p "$PROJECT_DIR/Assets/Audio/SFX"
mkdir -p "$PROJECT_DIR/Assets/Animations"
mkdir -p "$PROJECT_DIR/Assets/Fonts"
mkdir -p "$PROJECT_DIR/Assets/Resources"
mkdir -p "$PROJECT_DIR/Assets/Tests/EditMode"
mkdir -p "$PROJECT_DIR/Assets/Tests/PlayMode"
mkdir -p "$PROJECT_DIR/ProjectSettings"
mkdir -p "$PROJECT_DIR/Packages"

echo "✓ Directory structure created"

# Create README for the project
cat > "$PROJECT_DIR/README.md" << EOF
# $PROJECT_NAME

Unity game development project.

## Project Structure

- **Assets/Scripts/** - All C# scripts
- **Assets/Scenes/** - Unity scenes
- **Assets/Prefabs/** - Reusable game objects
- **Assets/Tests/** - Unit and integration tests

## Getting Started

1. Open this project in Unity (2021.3 LTS or later recommended)
2. Open the main scene in Assets/Scenes/
3. Press Play to test

## Development

See the [Unity Project Template]($REPO_ROOT/.github/UNITY_PROJECT_TEMPLATE.md) for coding standards and best practices.

## Testing

Run tests using Unity Test Runner:
- Window -> General -> Test Runner
- Select EditMode or PlayMode tabs
- Click "Run All"

## Build

Build settings are configured in Unity:
- File -> Build Settings
- Select target platform
- Click "Build" or "Build and Run"
EOF

echo "✓ Project README created"

# Create .gitkeep files to preserve empty directories
find "$PROJECT_DIR/Assets" -type d -empty -exec touch {}/.gitkeep \;

echo "✓ Added .gitkeep files to empty directories"

# Create basic assembly definition for Scripts
cat > "$PROJECT_DIR/Assets/Scripts/Scripts.asmdef" << EOF
{
    "name": "$PROJECT_NAME.Scripts",
    "rootNamespace": "$PROJECT_NAME",
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
EOF

echo "✓ Assembly definition created for Scripts"

# Create assembly definition for Tests
cat > "$PROJECT_DIR/Assets/Tests/Tests.asmdef" << EOF
{
    "name": "$PROJECT_NAME.Tests",
    "rootNamespace": "$PROJECT_NAME.Tests",
    "references": [
        "UnityEngine.TestRunner",
        "UnityEditor.TestRunner",
        "$PROJECT_NAME.Scripts"
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
EOF

echo "✓ Assembly definition created for Tests"

# Create a basic GameManager script
cat > "$PROJECT_DIR/Assets/Scripts/Managers/GameManager.cs" << 'EOF'
using UnityEngine;
using System;

namespace NAMESPACE_PLACEHOLDER.Managers
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
EOF

# Replace namespace placeholder
sed -i "s/NAMESPACE_PLACEHOLDER/$PROJECT_NAME/g" "$PROJECT_DIR/Assets/Scripts/Managers/GameManager.cs"

echo "✓ GameManager script created"

# Create a basic example test
cat > "$PROJECT_DIR/Assets/Tests/EditMode/ExampleEditModeTest.cs" << 'EOF'
using NUnit.Framework;
using UnityEngine;

namespace NAMESPACE_PLACEHOLDER.Tests
{
    public class ExampleEditModeTest
    {
        [Test]
        public void Example_Test_Passes()
        {
            Assert.IsTrue(true);
        }
    }
}
EOF

sed -i "s/NAMESPACE_PLACEHOLDER/$PROJECT_NAME/g" "$PROJECT_DIR/Assets/Tests/EditMode/ExampleEditModeTest.cs"

echo "✓ Example test created"

echo ""
echo "========================================"
echo "Unity project structure created successfully!"
echo "========================================"
echo "Project location: $PROJECT_DIR"
echo ""
echo "Next steps:"
echo "1. Open Unity Hub"
echo "2. Click 'Add' and select: $PROJECT_DIR"
echo "3. Open the project in Unity"
echo "4. Start developing!"
echo ""
echo "For coding standards and best practices, see:"
echo "$REPO_ROOT/.github/UNITY_PROJECT_TEMPLATE.md"
