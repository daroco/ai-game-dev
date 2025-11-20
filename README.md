# ai-game-dev

A repository for creating and modifying Unity C# game development projects with GitHub Copilot agent support.

## Overview

This repository provides infrastructure and tooling to help GitHub Copilot agents effectively create and modify Unity C# projects. It includes:

- **Copilot Instructions**: Guidelines for working with Unity C# projects
- **Project Templates**: Standard Unity project structure and code templates
- **Initialization Scripts**: Tools to quickly scaffold new Unity projects
- **Build Configuration**: .gitignore and best practices for Unity development

## Quick Start

### Creating a New Unity Project

Use the initialization script to create a new Unity project with the proper structure:

```bash
.github/scripts/init-unity-project.sh MyGameName
```

This will create a complete Unity project structure at `./MyGameName/` with:
- Organized asset folders (Scripts, Scenes, Prefabs, etc.)
- Assembly definitions for better compile times
- Basic GameManager script
- Test infrastructure setup
- Project-specific README

### Project Structure

Each Unity project created in this repository follows a standard structure:

```
ProjectName/
├── Assets/
│   ├── Scripts/         # C# scripts organized by feature
│   ├── Scenes/          # Unity scene files
│   ├── Prefabs/         # Reusable game objects
│   ├── Materials/       # Material assets
│   ├── Textures/        # Image assets
│   ├── Audio/           # Sound and music
│   └── Tests/           # Unit and integration tests
├── ProjectSettings/     # Unity project configuration
└── Packages/            # Unity Package Manager dependencies
```

## Documentation

- **[Copilot Instructions](.github/copilot-instructions.md)**: Guidelines for GitHub Copilot when working with Unity C# code
- **[Unity Project Template](.github/UNITY_PROJECT_TEMPLATE.md)**: Detailed project structure, coding standards, and best practices

## Coding Standards

Unity C# scripts in this repository follow these conventions:

- **Classes**: PascalCase (e.g., `PlayerController`)
- **Methods**: PascalCase (e.g., `MovePlayer()`)
- **Variables**: camelCase (e.g., `playerHealth`)
- **Private fields**: camelCase with underscore prefix (e.g., `_velocity`)

See the [Copilot Instructions](.github/copilot-instructions.md) for complete coding standards.

## Version Control

This repository uses a Unity-specific `.gitignore` that excludes:
- Build artifacts (`Library/`, `Temp/`, `Obj/`, `Build/`)
- IDE files (`.vs/`, `.vscode/`)
- Generated project files (`*.csproj`, `*.sln`)

Binary assets should use Git LFS for optimal version control.

## Development Workflow

1. Create a new Unity project using the initialization script
2. Open the project in Unity (2021.3 LTS or later recommended)
3. Follow the coding standards in the Copilot Instructions
4. Use the Unity Test Framework for testing
5. Commit changes following the repository's .gitignore patterns

## GitHub Copilot Integration

This repository is optimized for GitHub Copilot agents. The `.github/copilot-instructions.md` file provides:

- Unity project structure guidelines
- C# coding standards for Unity
- Common Unity patterns and templates
- MonoBehaviour lifecycle best practices
- Testing and build configuration guidance

## Contributing

When adding new Unity projects to this repository:

1. Use the `init-unity-project.sh` script to maintain consistency
2. Follow the established coding standards
3. Include tests for game logic
4. Document any custom build processes
5. Keep the project structure organized

## Resources

- [Unity Documentation](https://docs.unity3d.com/)
- [Unity C# Scripting Reference](https://docs.unity3d.com/ScriptReference/)
- [Unity Test Framework](https://docs.unity3d.com/Packages/com.unity.test-framework@latest)

## License

See LICENSE file for details.
