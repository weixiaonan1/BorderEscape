# Report for HCI Project

[TOC]

## Introduction

**Running Man** is a 3D parkour game based on **Unity 3D**. Other than using keyboard and mouse as input devices, the game features using **Kinect** as the motion sensing input device to detect and recognize player's body movement and gesture. The basic fuctionalities and operations are detailed in the [README file](README.md), the following parts will address the structure and the modules of the project.

### The Structure of the Program



### The Modules of the Program



## The Implemented Requirements

### Keyboard Control

### Kinect Control

### Moving Effect

### Track Generation

### Score System

### Background Music

## Evaluation

The project is inspired by a popular game *Temple Run*, and we are aimed to improve its interactivity by introducing *Kinect* into our project. By comparison, the projects bears some advantages as well as some disadvantages.

### Advantages

1. **Interactivity:**

   As stated before, the game supports  two ways of interaction —— traditional keyboard and mouse as well as Kinect. This can largely enhance players' immersion and realism, and provide players with a brand new gaming experience.

2. **Playability:**

   We well-designed the game mode to ensure the game is playful. The game starts relatively easy in the begining to let players become familiar with game operations. Then the rhythm of the game gradually speeds up to increase the difficulty of the game. The trace of the game is randomly generated so that it is not boring or insipid for players to play. What's more, the game track is infinitely long and players has unlimited access to gold coins and treasure chests to consantly challenge high scores.

3. **Extensibility：**

   We designed friendly and easy-to-use program interfaces. If some developers want to add new features and functionalities, or add some new tracks and checkpoints, it is fairly easy for them to achieve these since there is no need to refactor the code and the program logic.

4. **Friendly User Interface:**

   The background of this game is verdant lawns, blue sky, and white clouds, which makes players relaxed and delighted. The interface of the game is colorful and bright, which makes it children-friendly.

5. **Healthy lifestyle:**

   Traditionally, vedio games are regarded to be time-consuming, meaningless, and unhealthy for life. However, somatosensory games reverse the situation. As we see, this game gives people opportunity to work out without going outdoors. In addition, this game trains your body's flexibility, enhances responsiveness, strengthens speed, increases amount of exercise and enhances organism resistance.

### Disadvantages

1. **Limited Game Modes:**

   At present, this game supports only one game mode, which may reduce the game playability.

2. **Lack of Instruction:**

   In this game, we do not contain any game instruction. Neither is there any keyboard instruction, nor action instruction. Players have to explore the rules of the game, which may result in frustrate enthusiasm.

3. **Inaccurate Identification:**

   In the course of the game, sometimes the detection of Kinect is not so sensitive, making some actions and gestures delay in identification.

4. **Lack of Multi-person Interaction:**

   At current stage, this game support single player only. If there are some friends playing together, there should be many PCs and Kinects, and this is an awful situation.

5. **Monotonous Game Scene:**

   The game currently supports only two game props: gold coins and treasure chests.

## Refinement

Since the program is developed in a short time and is not very matural and perfect enough, some further refinement and improvements are supposed to be taken into consideration, which are listed as follows.

1. **Design New Game Modes**

   At present, the game supports only **Endurance Mode** where players run constantly to challenge high scores. We can also design other modes such as **Race Mode** (the shortest time to reach the end), **Prop Mode** (acquire and use powerful props during Parkour), **Escape Mode** (avoid storms or toxins through Parkour), and etc.

2. **Add Game Instructions**

   I'm not in favor of the practice of showing players a page detailing the operations of the game, but I suggest that when a player first comes into contact with the game and encounters certain obstacles, the game slows down, and gives guidence to the player on how to overcome the obstacles. After completing these operations in person, the player may quickly get acquainted with the game and grow interest in this game.

3. **Improve Recognition Accuracy**

   In our practice, the program can only recognize the actions and gestures defined officially by Kinect. In a bid to improve recognition accuracy and avoid action conflicts, we can define some special actions and gestures by detecting the motions of specific joints and bones.

4. **Design Multiplayer Game Mode**

   Kinect is capable of detecting two or more people's actions though its results may not be very satisfactory. The introduction of Multiplayer Game Mode can enhance the entertainment of the game. 

5. **Refine Game Scene**

   The current game scene is monotonous. We can refresh the game scene by adding more various style scenes (such as the rain, the sunset, the fog, etc.), constantly changing the back ground color and the styles of tracks and obstacles to make the gaming process more dynamic and delightful.