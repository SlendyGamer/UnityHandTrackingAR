Unity UI Menu System (C# Scripts)
=================================

## Setup Instructions

1. Import the `Scripts` folder into your Unity project's `Assets` directory.

2. Attach `PauseManager.cs` to an empty GameObject in your game scene.

   - Link the pause menu UI to the `pauseMenu` field in the Inspector.

3. Use `MenuManager.cs` for your Main Menu UI buttons to load scenes or open option panels.

Feel free to customize the logic and UI according to your project's needs.

This package includes ready-to-use Unity C# scripts for implementing a complete UI menu system.

Included Scripts:
-----------------
1. PauseManager.cs
   - Handles pausing and resuming the game using the Escape key.
   - Displays a pause menu and provides a Quit Game option.

2. MainMenu.cs
   - Loads a game scene and exits the application.

3. OptionsMenu.cs
   - Controls volume, screen resolution, and fullscreen toggle using Unity's UI components.

Usage Instructions:
-------------------
1. Attach the scripts to appropriate GameObjects in your Unity scene.
2. Set up the UI Canvas with buttons and panels for pause menu, main menu, and options menu.
3. Link UI elements (buttons, sliders, dropdowns) to the public functions in each script.

Compatibility:
--------------
- Unity 2019.4+
- Works on PC/Mac/Linux standalone platforms.

Customize the UI appearance and scene names according to your game project.