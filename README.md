# Final Trek: Zombie Frontier

A post-apocalyptic survival game inspired by Oregon Trail, built in Unity. Travel across a zombie-ravaged country, manage resources, and survive three unique minigames — turret defense, side-scrolling combat, and car repair — across 10 cities.

> **CS2 End-of-Year Project**  
> **Team:** Quinn Marcello, Nicholas Doan, Samuel Ferrera, Josh Hill

---

## Download

[**Download Final Trek: Zombie Frontier v1.0**](https://github.com/quinnmarcello-cyber/final-trek-zombie-frontier/releases/download/V1.0/Final.Trek.-.Zombie.Frontier.zip)

---

## Gameplay Overview

You are a survivor traveling across a zombie-ravaged country, moving from city to city on a hand-drawn map. At each stop you can scavenge for food, find gas, check your inventory, or view your progress on the map. Resources deplete as you travel and difficulty scales the further you get. Make it to the final city to win.

---

## Game Flow

```
Story Slides → Travel / Choices Screen → Minigame or Map → Results Screen → Next City
```

- **Story Slides** — Intro cutscene setting up the world
- **Choices Screen** — Pick an activity at each city stop (each minigame is only available once per city)
- **Results Screen** — Displays resources gained or lost after each minigame
- **Map Screen** — Shows your journey progress across 10 cities with a blinking current location marker

---

## Minigames

### Turret Defense — Find Gas
Defend your car from zombies attacking from all directions. The car sits in the center of the screen with a rotating turret. Hold left click to fire and slow down incoming zombies. Survive the full timer to earn gas. If a zombie reaches the car, gas is lost.

### Side Scroller — Scavenge for Food
A side-scrolling shooter where the player moves left to right and shoots zombies as they descend down the screen. Survive the wave to earn food supplies.

### Car Repair — Repair Minigame
A precision-based minigame similar to the fishing mechanic in Stardew Valley. Keep a moving indicator inside a green zone to successfully repair the car. Timing and focus are key.

---

## Controls

| Input | Action |
|---|---|
| Mouse | Aim turret (Turret Defense) |
| Left Click (Hold) | Fire (Turret Defense) |
| Enter | Confirm / Continue |
| 1 / 2 / 3 / 4 | Select option on Choices screen |

---

## Features

- Scrolling map system driving progression across 10 cities
- Three unique interactive minigames with distinct mechanics
- Resource management — gas, food, and scrap metal deplete and are earned through minigames
- Per-city minigame lock — each minigame can only be played once per city stop
- Wave-based difficulty scaling — zombie speed, count, and type increase over time
- Two zombie types with directional walk animations
- Dynamic map progression with line reveals and blinking city marker
- Story slides intro sequence
- Fully custom pixel art and UI
- Sound effects and music throughout

---

## Team

Quinn Marcello, Nicholas Doan, Samuel Ferrera, Josh Hill

### My Contributions (Josh Hill)

- Built the turret defense minigame including enemy AI, bullet pooling, wave management, and difficulty scaling
- Implemented the map screen with 10 city progression, dynamic line reveals, and blinking location marker
- Built the choices screen, results screen, and scene flow between all minigames
- Integrated all sound effects and music throughout the game
- Created custom pixel art used throughout the game
- Pitched the initial concept, led beta testing with peers, and handled bug fixes before final submission

---

## Built With

- Unity (2D)
- C#
- TextMeshPro
- Unity Input System
- Unity Animator

---

## Setup & Running

1. Clone the repository
2. Open the project in Unity Hub
3. Make sure all scenes are added to File > Build Settings
4. Open the StorySlides scene and hit Play to start from the beginning

---

## Screenshots

<img width="1248" height="832" alt="Main Menu" src="https://github.com/user-attachments/assets/75f942fa-d26d-4185-96e3-1d3871b8e4bd" />

<img width="943" height="535" alt="Gameplay 1" src="https://github.com/user-attachments/assets/3e0c4a34-6b9f-4c87-be17-0c46790eee91" />

<img width="944" height="537" alt="Gameplay 2" src="https://github.com/user-attachments/assets/f25c1cc3-9f5c-4a1b-a831-6b09caad3853" />
