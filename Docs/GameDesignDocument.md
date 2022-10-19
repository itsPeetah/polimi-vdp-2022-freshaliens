# FRESHALIENS

# Game Design Document

## 1 Introduction

Freshaliens is a 2D cooperative platform where each player will control one of the two character. The main characters are two little aliens,
sent to explore the universe and conquer planets, as their race has always done. The two aliens are not the same: one is able to jump higher and
overcome obstacles with his movement skills, the other one is a good shooter able to use his trusted weapon to kill enemies.

### 1.1 Plot

In a remote galaxy there is an ancient alien race that use to conquer as many planets as they can. Their children are trained since their birth to conquer planets and when they turn 10 are sent to conquer their first planets.
Two cute aliens, Gün and Jümp, are the main character of our aventure.  
Gün has been trained to kill, he has no fear and has learnt to use all kind of alien weapons but he always use his favorite one.
Jümp has been trained to overcome every obstacle he can face. Has no fear of highness and there is no wall he cannot climb.
Gün and Jümp have also learnt to collaborate together as a couple of partners sholud ever do.
Will they be able to conquer all their assigned planets?

## 2 Description of the game
### 2.1 Technical specifics

Freshaliens is a PC offline game where the players play using the keyboard and the mouse as input, also controller input will be provided and suggested.  
It is developed using Unity as graphic engine and C# scripts.  
The game has a 2D graphic with an horizontal scrolling view (like Super Mario Bros for instance).

## 3 Gameplay design

### 3.1 Basic Mechanics

|              "Soldier"              |                 "Ninja"                  |
| :---------------------------------: | :--------------------------------------: |
|       Can use the basic jump        |          Can use the basic jump          |
|                  /                  | Can use the double jump (or higher jump) |
|                  /                  |    Can climb walls by jumping on them    |
| Can shoot to enemies with his gun\* | Can only stun enemies with melee attacks |

*Gün's gun will upgrade in the game, getting more powerful and a cooler design.

### 3.2 Cooperative Mechanics

- The two aliens have 3 shared lives: each time one of them is hit, one of the lives is lost, once all the lives are lost the level restart from above.
  When someone is hit he will blink for some seconds and there will be no respawn, if someone falls he will respawn in a chosen point.
- Gün can jump higher thanks to Jümp: when they are near have to press JUMP in the same moment and Jümp will make Gün jump higher.
- (Advanced) Once Jümp threw Gün, he can jump toward him and repeat the jump on air.

### 3.3 Cooperative situations

- While Jümp search a way to go on, enemies attacks: they can attack Jümp, Gün or both. The solider will must defend themselves.
- Gün could must shoot to the environment in order to allow Jümp to proceed, the whole mechanic could be required to be executed in a synchronous way.
- In situation with several enemies (and boss fights) Jümp can use the environment to harm them.
- Friendly fire (not default but can be enable)
- In the level can be found Powerful Ammo which can allow Gün to shoot a Powerful Shot.
  If Jümp found one of them must bring it to Gün to make him use it.

### 3.4 Levels Organization

Each planet to conquer represent a level. The planets will be distributed in solar systems. (Ideally) There are 3 solar systems, each one composed of 4 planets, for a total of 12 levels.
At the and of the last planet of each solar system there will be a Boss Fight.

### 3.5 Level Design

In order to give the idea of conquering a planet, each level of the game is shaped as a round world: the two aliens must round it all, killing all enemies and overcoming all the obstacles. They land on a point on the planet with their ship and, rounding it, they will return to the starting point and plant their flag.
The planets of the same solar system will be characterized by the same design: rocky, desert, iced, jungle.

### 3.6 Controls

KEYBOARD: Jümp will use W-A-D to move and jump, Gün will use ← ↑ → to move and jump and the mouse to aim and shoot.
The aim system of Gün will be radial: he has a pointer and will use the mouse to choose the direction and the left button to shoot.
Some other keys could be used for special actions.  
CONTROLLER: Jümp will use the arrows (or the left stick) to move and one button to jump. Gün will use the arrows (or the left stick) to move, the right stick to aim and a right trigger to shoot.

### 3.7 Enemies

On the planets there are different kind of enemies (each one with its own attack style, maybe? could be cool). Flying and not flying enemies.

### 3.8 Bosses

### 3.9 Music

### 3.10 Assets

### 3.11 Menu
Once the game starts it is shown the main menu. From it is possible to start a new game, load a previous one or change the settings.  
When the game starts it will show the actual solar system and the player have to choose the level to play.  
