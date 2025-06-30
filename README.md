# VRoom - Virtual Reality Interior Design Experience

VRoom is an immersive **Virtual Reality (VR)** application built with **Unity 6000.0.45f1** for Meta Quest devices. It enables users to design, customize, and interact with interior environments using natural hand tracking and gesture-based controls, providing an intuitive spatial computing experience for interior design and room planning.

## 🎯 Purpose

VRoom empowers users to visualize and design interior spaces in virtual reality. Users can modify room dimensions, place and manipulate furniture, change materials and textures, and save their designs - all through natural hand interactions without the need for controllers.

## 🚀 Key Features

### Core Functionality
- 🖐️ **Hand Tracking & Gesture Controls** - Fully controller-free interaction using Meta's hand tracking
- 🏠 **Dynamic Room Editing** - Reshape room dimensions and modify architectural elements
- 🛋️ **Furniture Management** - Spawn, position, scale, rotate, and delete furniture items
- 🎨 **Material & Texture System** - Apply different materials to walls, floors, and furniture
- 💾 **Save & Load System** - Persist room configurations and restore previous designs
- 🚶 **Teleportation Movement** - Navigate through rooms using gesture-based teleportation

### User Interface
- 📱 **Radial Menu System** - Category-based furniture selection interface
- 🎛️ **Furniture Menu** - Browse and select from various furniture categories
- 🧱 **Material Selection UI** - Choose textures and materials for surfaces
- 🗑️ **Delete Manager** - Remove objects with intuitive selection

### VR Integration
- 🥽 **Meta Quest Optimized** - Built specifically for Meta Quest 2/3/Pro devices
- 👋 **Hand Pose Recognition** - Advanced gesture detection and hand pose tracking
- � **Mixed Reality Support** - Passthrough and mixed reality capabilities
- 📳 **Haptic Feedback** - Enhanced tactile feedback via bHaptics integration

## 🛠️ Technical Stack

- **Unity 6000.0.45f1** - Latest Unity engine with enhanced VR support
- **Meta XR SDK v68.0.2** - Complete Meta VR development framework
- **OpenXR Plugin v1.12.0** - Cross-platform VR runtime
- **XR Interaction Toolkit v3.0.4** - Unity's VR interaction framework
- **bHaptics SDK** - Advanced haptic feedback system
- **Meta Hand Tracking SDK** - Precise hand and finger tracking

### Key Dependencies
```json
{
  "com.meta.xr.sdk.all": "68.0.2",
  "com.unity.xr.openxr": "1.12.0",
  "com.unity.xr.interaction.toolkit": "3.0.4",
  "com.unity.inputsystem": "1.7.0",
  "com.unity.xr.management": "4.4.1"
}
```

## 🏗️ Architecture Overview

### Core Systems
- **Room Management** (`RoomReshaper`, `ReshapeRoom`) - Handles room geometry and spatial modifications
- **Furniture System** (`FurnitureSpawner`, `FurnitureGrabTransformer`) - Manages furniture lifecycle and interactions  
- **UI Controllers** (`InstantiatePrefabUI`, `InstantiateTexturesUI`) - Handles user interface and menu systems
- **Persistence Layer** (`SaveAndLoad`) - Manages scene state serialization and restoration
- **Interaction Framework** - Hand tracking, gesture recognition, and VR input handling

### Project Structure
```
Assets/
├── Scripts/
│   ├── FurnitureGrabTransformer.cs    # Furniture manipulation logic
│   ├── FurnitureSpawner.cs            # Furniture instantiation system
│   ├── ScriptsConrado/                # Room editing functionality
│   │   ├── RoomReshaper.cs
│   │   ├── ReshapeRoom.cs
│   │   └── OpenRoomEditMode.cs
│   └── ScriptsMario/                  # UI and save system
│       ├── InstantiatePrefabUI.cs
│       ├── InstantiateTexturesUI.cs
│       ├── DeleteManager.cs
│       └── Save/SaveAndLoad.cs
├── Prefabs/                           # Reusable game objects
├── Scenes/                            # VR scenes and environments
└── Resources/                         # Runtime-loaded assets
```

## 🎮 Interaction Modes

| Mode | Description | Hand Gestures |
|------|-------------|---------------|
| **Navigation** | Move around the room | Point to teleport |
| **Room Edit** | Modify room dimensions and structure | Grab and drag room corners/edges |
| **Furniture Place** | Add new furniture items | Select from radial menu, position with hands |
| **Furniture Edit** | Transform existing furniture | Grab to move, pinch to scale, twist to rotate |
| **Material Edit** | Change surface textures | Select materials from UI, apply to surfaces |
| **Delete Mode** | Remove objects from scene | Point and select objects to delete |

## 📦 Installation & Setup

### Prerequisites
- **Unity Hub** with Unity 6000.0.45f1 installed
- **Meta Quest Link** or **Meta Quest Developer Hub**
- **Meta Quest 2/3/Pro** with developer mode enabled
- **Android Build Support** module for Unity

### Quick Start
1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd VRoom
```

2. **Open in Unity:**
   - Launch Unity Hub
   - Click "Open" and select the VRoom project folder
   - Unity will automatically import packages and dependencies

3. **Configure Build Settings:**
   - Go to `File > Build Settings`
   - Select "Android" platform
   - Switch platform if not already selected
   - Set Texture Compression to "ASTC"

4. **XR Settings:**
   - Navigate to `Edit > Project Settings > XR Plug-in Management`
   - Enable "OpenXR" for Android
   - Under OpenXR settings, enable "Meta Quest Support"
   - Enable "Hand Tracking" in OpenXR features

5. **Build and Deploy:**
   - Connect your Meta Quest device via USB
   - Enable Developer Mode on your Quest
   - Click "Build and Run" in Unity

## 🎯 Usage Guide

### Getting Started
1. **Put on your Meta Quest headset** and launch the VRoom application
2. **Enable hand tracking** in your Quest settings for the best experience
3. **Calibrate your play area** to ensure safe movement within the virtual space

### Basic Controls
- **Teleportation**: Point your index finger at the desired location and make a pinching gesture
- **Open Menus**: Hold your palm up and make a pinching gesture to access the radial menu
- **Grab Objects**: Pinch and hold to grab furniture or room elements
- **Scale Objects**: While grabbing, move your hands closer or farther apart
- **Rotate Objects**: Twist your wrist while holding an object

### Room Editing Workflow
1. **Enter Room Edit Mode** via the main menu
2. **Grab room corners or edges** to reshape the room dimensions
3. **Apply materials** by selecting from the texture menu
4. **Save your changes** using the save system

### Furniture Management
1. **Open Furniture Menu** using the radial interface
2. **Browse categories** (chairs, tables, decorations, etc.)
3. **Select and place** furniture items in your desired locations
4. **Fine-tune positioning** using grab and transform gestures
5. **Delete unwanted items** using the delete mode

## 🔧 Development

### Project Configuration
- **Unity Version**: 6000.0.45f1 (required for optimal Meta XR SDK compatibility)
- **Target Platform**: Android (Meta Quest)
- **Minimum API Level**: 29
- **Scripting Backend**: IL2CPP
- **Api Compatibility**: .NET Standard 2.1

### Key Scripts Overview
- `FurnitureGrabTransformer.cs` - Handles furniture manipulation and transformation
- `FurnitureSpawner.cs` - Manages furniture instantiation and placement
- `RoomReshaper.cs` - Controls room geometry modification
- `SaveAndLoad.cs` - Handles scene persistence and state management
- `InstantiatePrefabUI.cs` - Manages prefab selection and UI interactions

### Building for Meta Quest
```bash
# Via Unity CLI (optional)
Unity -batchmode -quit -projectPath . -buildTarget Android -executeMethod BuildScript.BuildAndroid
```

## 🤝 Contributing

1. **Fork the repository**
2. **Create a feature branch** (`git checkout -b feature/amazing-feature`)
3. **Commit your changes** (`git commit -m 'Add amazing feature'`)
4. **Push to the branch** (`git push origin feature/amazing-feature`)
5. **Open a Pull Request**

### Development Guidelines
- Follow Unity's coding conventions
- Test on actual Meta Quest hardware
- Ensure hand tracking compatibility
- Document new features and APIs
- Maintain backward compatibility where possible

## 🐛 Troubleshooting

### Common Issues
- **Hand tracking not working**: Ensure hand tracking is enabled in Quest settings and OpenXR features
- **Build errors**: Verify Unity version (6000.0.45f1) and all required packages are installed
- **Performance issues**: Check texture compression settings and optimize mesh complexity
- **Menu not appearing**: Calibrate hand tracking and ensure proper gesture recognition

### Performance Optimization
- Enable **Occlusion Culling** for better rendering performance
- Use **Level of Detail (LOD)** for complex furniture models
- Optimize **texture resolutions** for target hardware
- Implement **object pooling** for frequently spawned items

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🏆 Acknowledgments

- **Meta** for the XR SDK and development tools
- **Unity Technologies** for the powerful VR development platform
- **bHaptics** for haptic feedback integration
- **OpenXR** for cross-platform VR runtime support

## 📞 Support

For questions, issues, or contributions:
- Create an issue on GitHub
- Check the [Wiki](wiki) for detailed documentation
- Join our community discussions

---

**VRoom** - Transforming interior design through immersive virtual reality experiences.
