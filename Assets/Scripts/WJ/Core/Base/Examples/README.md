# Testing Instructions for Base Managers

## Required Setup
1. Create the following scenes in your Unity project:
   - MainTestScene
   - LoadingScene
   - TransitionTestScene

## WJBaseCameraManager Testing
1. In MainTestScene:
   - Create an empty GameObject named "CameraManager"
   - Add your camera implementation (extending WJBaseCameraManager) to it
   - Create a target object (e.g., cube) to follow
   - Set up camera bounds if needed
2. Test the following:
   - Camera following behavior
   - Bounds constraints
   - Smooth following
   - Camera offset adjustments

## WJBaseNetworkManager Testing
1. In MainTestScene:
   - Create an empty GameObject named "NetworkManager"
   - Add your network implementation (extending WJBaseNetworkManager) to it
   - Configure network settings (room name, auto-connect, etc.)
2. Test the following:
   - Connection/disconnection
   - Room joining/leaving
   - Network callbacks
   - Error handling

## WJBaseObjectPool Testing
1. In MainTestScene:
   - Create an empty GameObject named "ObjectPool"
   - Add WJBaseObjectPool component
   - Create test prefabs (e.g., projectiles, effects)
   - Configure pool settings (size, prefabs)
2. Test the following:
   - Object spawning
   - Object despawning
   - Pool expansion
   - IPoolable callbacks

## WJBaseTransitionManager Testing
1. Setup:
   - Create LoadingScene with loading UI
   - Create TransitionTestScene as destination
   - Create transition panel prefab (with CanvasGroup)
2. In MainTestScene:
   - Create an empty GameObject named "TransitionManager"
   - Add your transition implementation (extending WJBaseTransitionManager)
   - Assign transition panel and settings
3. Test the following:
   - Scene transitions
   - Loading screen
   - Fade effects
   - Transition callbacks

## Example Prefabs
1. TransitionPanel Prefab:
```
- Canvas (Screen Space - Overlay)
  - TransitionPanel
    - Image (Black, full screen)
    - CanvasGroup component
```

2. NetworkPlayer Prefab:
```
- NetworkPlayer
  - Mesh/Sprite
  - Network components
  - INetworkPoolable implementation (if using pool)
```

3. PooledObject Prefab:
```
- PooledObject
  - Mesh/Sprite
  - IPoolable implementation
```

## Common Issues and Solutions
1. Camera not following:
   - Check if target is assigned
   - Verify smooth following settings
   - Check camera bounds configuration

2. Network connection issues:
   - Verify network settings
   - Check room name configuration
   - Ensure proper initialization order

3. Object pool issues:
   - Check pool size settings
   - Verify prefab references
   - Ensure proper IPoolable implementation

4. Transition issues:
   - Verify scene names in build settings
   - Check transition panel references
   - Verify CanvasGroup setup

## Testing Checklist
- [ ] Camera following target smoothly
- [ ] Camera bounds working correctly
- [ ] Network connection successful
- [ ] Network callbacks firing
- [ ] Objects spawning from pool
- [ ] Objects returning to pool
- [ ] Scene transitions working
- [ ] Loading screen displaying
- [ ] Transition animations smooth

Please report any issues or unexpected behavior during testing.
