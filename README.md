# Redline Run

Redline Run is a fast-paced arcade driving game that combines obstacle avoidance, resource management, bidirectional shooting, and continuous police pursuit.

The player must survive a 90-second drive, manage limited fuel and ammunition, and reach the finish line before being caught by the police or running out of fuel.

## Play the Game

- [Play the WebGL build](https://csci-526.github.io/Team21-He_Ali/)
- [Play on Unity Play](https://play.unity.com/api/v1/games/game/89f21a1c-9f23-48b2-a5fe-1f46184a3b38/build/latest/frame)
- [Watch the gameplay video](https://youtu.be/1QeiQiOD2BY)
- [Read the descriptive document](https://docs.google.com/document/d/158VhF6BgA7K4MI-C9BUlZdGnkoFmhDPkxYktXaQlbyE/edit?usp=sharing)

## Gameplay

Drive along an obstacle-filled road while a police vehicle continuously pursues you from behind. Avoiding obstacles helps maintain your distance, while collisions allow the police to close the gap.

Fuel decreases throughout the run, so fuel pickups are required to keep moving. Ammunition is limited to three rounds and can be used in two ways:

- Shoot forward to destroy an obstacle and clear the road.
- Shoot backward to temporarily slow the pursuing police.

Because both actions consume the same limited ammunition, players must decide whether the immediate danger is ahead or behind them.

## Core Features

- Fast-paced arcade driving
- Continuous police pursuit
- Obstacle avoidance and destruction
- Fuel management and fuel pickups
- Limited ammunition and ammo pickups
- Forward and backward shooting
- Increasing pursuit pressure
- Risk-versus-reward resource collection
- Timed finish-line objective

## Controls

| Action | Control |
| --- | --- |
| Move left | `A` or `Left Arrow` |
| Move right | `D` or `Right Arrow` |
| Shoot forward | `W` or `Up Arrow` |
| Shoot backward | `S` or `Down Arrow` |
| Start the game | `Enter` or the **Start** button |

## Objective

Reach the finish line before the 90-second run ends.

The game ends if:

- The police catch the player.
- The player runs out of fuel.

## Core Design Twist

The central twist is limited resource management under pursuit.

Ammunition serves two competing purposes: clearing obstacles ahead and slowing the police behind. Skilled driving conserves ammunition, while shooting provides temporary safety at the cost of a scarce resource. Fuel and ammunition pickups may also require moving into more dangerous positions, creating additional risk-versus-reward decisions.

## Built With

- Unity
- C#
- Universal Render Pipeline
- WebGL

## Team

### Khalid Ali

[GitHub profile](https://github.com/Data-Driven-Motors-G80)

- Refined obstacle placement and behavior
- Created the user interface, start screen, and finish sequence
- Implemented fuel depletion and fuel pickups
- Implemented ammunition pickups and shooting
- Created the moving course and course-pacing system
- Improved player, obstacle, lighting, and impact visuals

### Tianyu He

[GitHub profile](https://github.com/TianyuHe11)

- Created the initial game scene
- Implemented player movement
- Developed police movement and chase behavior
- Added police-catch and game-over logic
- Implemented the police slowdown caused by backward projectiles

## Repository

[CSCI-526/Team21-He_Ali](https://github.com/CSCI-526/Team21-He_Ali)

## WebGL Deployment

1. In Unity, open **File > Build Profiles**.
2. Select **Web** and switch to that platform.
3. Under **Player > Web > Publishing Settings**, set **Compression Format** to **Disabled** for GitHub Pages compatibility.
4. Build the project into the repository's `docs` folder.
5. If Unity creates `docs/docs`, move that nested build into the top-level `docs` folder.
6. Confirm that `docs/.nojekyll` exists.
7. Commit and push the updated project and `docs` directory.
8. Configure GitHub Pages to deploy from the `main` branch and `/docs` folder.
