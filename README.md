Tech Art Test – Tripledot

Overview
This project was created as part of the Tech Art Test assignment.
It demonstrates a modular and clean approach to UI implementation in Unity, focusing on structure, scalability, and performance.

Unity Version
Unity 2022.3.x (LTS)

Project Structure
Assets/
├── Animations/        - Animation clips and controllers for UI transitions
├── Art/               - Graphic assets and textures
│   ├── Backgrounds/
│   ├── Effects/
│   ├── Icons/
│   ├── Rewards/
│   └── Textures/
├── Atlases/           - Sprite atlases for batching optimization
├── Fonts/             - Font files and TextMeshPro Font Assets
│   ├── Lato-Black/
│   └── Yorkie-SemiBold/
├── Materials/         - Common UI materials
├── Prefabs/           - Prefab files for screens, popups, and UI elements
│   ├── HomeScreen_BottomBarTab
│   ├── HomeScreenWindow_UI
│   ├── LevelCompletedWindow_UI
│   ├── SettingsWindow_UI
│   ├── RewardBigCont
│   ├── RewardSmallCont
│   └── SettingsWindow_Elements...
├── Scenes/            - Main scene
├── Scripts/           - C# scripts for UI logic and control
├── Shaders/           - Custom shaders
└── TextMesh Pro/      - TMP configuration and settings

Features
- Adaptive UI layout supporting different resolutions and safe areas
- Reusable UI prefabs (no prefab variants used)
- Toggle-based bottom navigation bar
- Modular popup system with reusable background and animation logic
- Level Completed screen with reward animation and basic particle system
- Font Assets with Latin, Cyrillic, and extended European character sets
- Optimized textures using 2x scale, 9-slice, and small white base textures for tinting

Logic and Interaction
- Bottom bar: implemented with ToggleGroup, only one active tab at a time
- Settings button: opens the Settings Popup
- Level Complete button: opens the Level Completed screen
- Close actions: Settings closes via cross button; Level Complete closes via Home button
- All UI animations (show/hide) are handled via Unity Animator

Texture and Atlas Optimization
- Textures are packed into atlases to reduce draw calls
- 9-slice and white 4x4 textures are used for scalable and tintable elements
- Assets prepared in 2x resolution (Pixels Per Unit = 200) for consistent scaling
- Compression and visual quality tested on device builds

Fonts
- Lato Black
- Yorkie SemiBold

All assets were provided as part of the original test assignment.
