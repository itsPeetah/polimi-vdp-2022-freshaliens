# FRESHALIENS
# Game Design Document

## 1 Introduction
Freshaliens is a 2D cooperative platform where each player will control one of the two character. The main characters are two little aliens,
sent to explore the universe and conquer planets, as their race has always done. The two aliens are not the same: one is able to jump higher and
overcome obstacles with his movement skills, the other one is a good shooter able to use his trusted weapon to kill enemies.
## 2 Description of the game
### 2.1 Technical specifics
Freshaliens is a PC offline game where the players play using the keyboard and the mouse as input, also controller input will be provided and suggested.
It is developed using Unity as graphic engine and C# scripts.
The game has a 2D graphic with an horizontal scrolling view (like Super Mario Bros for instance).
## 3 Gameplay design
### 3.1 Basic Mechanics
|             "Soldier"              |                 "Ninja"                  |
|:----------------------------------:|:----------------------------------------:|
|       Can use the basic jump       |          Can use the basic jump          |
|                 /                  | Can use the double jump (or higher jump) |
|                 /                  |    Can climb walls by jumping on them    |
| Can shoot to enemies with his gun* | Can only stun enemies with melee attacks |

*The soldier's gun will upgrade in the game, getting more powerful and a cooler design.

### 3.2 Cooperative Mechanics
- The two aliens have 3 shared lives: each time one of them is hit, one of the lives is lost, once all the lives are lost the level restart from above. 
When someone is hit he will blink for some seconds and there will be no respawn, if someone falls he will respawn in a chosen point.
- The soldier can jump higher thanks to the ninja: when they are near have to press JUMP in the same moment and the ninja will make the soldier jump higher. 
- (Advanced) Once the ninja threw the soldier, he can jump toward him and repeat the jump on air.

### 3.3 Cooperative situations
- While the ninja search a way to go on, enemies attacks: they can attack the ninja, the soldier or both. The solider will must defend themselves.
- The soldier could must shoot to the environment in order to allow the ninja to proceed, the whole mechanic could be required to be executed in a synchronous way.
- In situation with several enemies (and boss fights) the ninja can use the environment to harm them.
- Friendly fire (not default but can be enable)
- In the level can be found Powerful Ammo which can allow the Soldier to shoot a Powerful Shot.
If the ninja found one of them must bring it to the Soldier to make him use it.
### 3.4 Level Organization
Each planet to conquer represent a level. The planets will be distributed in solar systems. (Ideally) There are 3 solar systems, each one composed of 4 planets, for a total of 12 levels.
At the and of the last planet of each solar system there will be a Boss Fight.

### 3.5 Level Design
In order to give the idea of conquering a planet, each level of the game is shaped as a round world: the two aliens must round it all, killing all enemies and
overcoming all the obstacles. They land on a point on the planet with their ship and, rounding it, they will return to the starting point.
The planets of the same solar system will be characterized by the same design: rocky, desert, iced, jungle.

### 3.6 Controls
KEYBOARD: The Ninja will use W-A-D to move and jump, the soldier will use ← ↑ → to move and jump and the mouse to aim and shoot.
The aim system of the soldier will be radial: he has a pointer and will use the mouse to choose the direction and the left button to shoot.
Some other keys could be used for special actions.
CONTROLLER: The ninja will use the arrows (or the left stick) to move and one button to jump. The Soldier will use the arrows (or the left stick) to move, the right stick to aim
and a right trigger to shoot.





