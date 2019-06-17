# Report for HCI Project

- [Report for HCI Project](#Report-for-HCI-Project)
  - [Introduction](#Introduction)
    - [The Structure and Modules of the Program](#The-Structure-and-Modules-of-the-Program)
      - [Menu Scene](#Menu-Scene)
      - [Main Scene](#Main-Scene)
  - [The Implemented Requirements](#The-Implemented-Requirements)
    - [Launch Game](#Launch-Game)
    - [Keyboard Control](#Keyboard-Control)
    - [Kinect Control](#Kinect-Control)
    - [Moving Effect](#Moving-Effect)
    - [Collision Detection](#Collision-Detection)
    - [Track Generation](#Track-Generation)
    - [Score System](#Score-System)
    - [Background Music](#Background-Music)
  - [Evaluation](#Evaluation)
    - [Advantages](#Advantages)
    - [Disadvantages](#Disadvantages)
  - [Further Refinement](#Further-Refinement)

## Introduction

**Border Escape** is a 3D Somatosensory Game based on **Unity 3D**. Other than using keyboard and mouse as input devices, the game features using **Kinect** as the motion sensing input device to detect and recognize player's body movement and gesture. The basic functionalities and operations are detailed in the [README file](README.md), the following parts will address the issues of the structure and the modules of the project.

### The Structure and Modules of the Program

In our Unity project, each object is bound with a corresponding controller.

We adopt the *EmptyGO* structure, which means that we create several empty *GameObject*, and then hang several logic control scripts on them. We can find them with *GameObject.Find()* function.

#### Menu Scene

- **View (Objects)**
  - **BackGround:** Use a Western wilderness scene as the background.
  - **Leading Role:** The role that the player controls in the game.
  - **Start Button:** The button that controls the beginning of the game.
  - **Instruction Button:** The button that displays the menu of the instruction.
  - **Exit Button:** The button that enables the player to quit the game.
  - **Instruction Menu:** The menu that contains the text displayed on the screen, which gives the player the instruction of keyboard operation.

- **Controller (Scripts)**
  - **MenuController:** Feedback on the player's input (either from keyboard or *Kinect*)
  - **KinectController:** Detect player's motion and analyse the gestures of the player.

#### Main Scene

- **View (Objects)**
  - **Track:**
    - **Ground:** The track of the game (the cement road).
    - **Environment:** The environment besides the track (the houses, the aircraft, and the trees, etc.).
    - **Obstacles:** The obstacles on the track (the boxes and the bridges).
    - **Props:** The props on the road (the cartridge clips and the medical boxes).
  - **Role:** The role that the player controls in the game.
  - **Pause Button:** The button that enables the player to pause the game.
  - **Restart Button:** The button that enables the player to restart the game.
  - **Continue Button:** The button that enables the player to continue the game.
  - **Exit Button:** The button that enables the player to quit the game.
- **Controller (Scripts)**
  - **DataTransformer:** Maintain the data exchanged among these scripts.
  - **GameController**: The script file that controls the sequence of the game.
  - **KinectController:** The script file that detects the player's motions.
  - **KinectGestures:** The script file that analyses the player's gestures.
  - **PlayerGestureListener:** The script file that obtains the gestures of the player.
  - **TrackController:** The script file that controls the movement of the track along with changes the speed.
  - **PlayerController:** The script file that controls the role through keyboard and Kinect.
  - **ScoreController:** The script file that controls the score the player gains.
  - **GamingBGMController:** The script file that controls the BGM of the game.
  - **Props:** The script file bound to the prop objects.
  - **Roll:** Generate a new track randomly and destroy the old one.

## The Implemented Requirements

In this section, we will introduce the main game logics and how the functionalities are implemented. We will present part of the source code to illustrate the implementation.

### Launch Game

There are two ways to start the game.

In `MenuController.cs`, by means of judging whether the player enters the game through the action of Run, or clicking the start button, the game stores the mode of the game with a variant `PlayerPrefs` cross two scenes.

```c#
// Start with keyboard as the input device
public void StartBtn()
{
    //load game scene and use keyboard input;
    mode = 1;
    SceneManager.LoadScene("main", LoadSceneMode.Single);
    PlayerPrefs.SetInt("mode", mode);
}

// Start with Kinect as the input device
else if (gestureListener.IsRun())
{
    //load game scene and use kincet input;
    mode = 0;
    SceneManager.LoadScene("main", LoadSceneMode.Single);
    PlayerPrefs.SetInt("mode", mode);
}
```

In `PlayerController.cs`, the game obtains the `mode` value  and sets the input device.

```c#
//get mode(user selected) in menu scene and set useKinectInput
int mode = PlayerPrefs.GetInt("mode");
useKinectInput = mode == 0 ? true : false;
gestureListener = PlayerGestureListener.Instance;
```

### Keyboard Control

Listen to the keyboard input, identify its operation, and make the role perform according actions.

```c#
// Get keyboard input as "blank space"
if (useKeyboardInput)
{
    if (Input.GetButton("Jump"))
    {
        body.velocity = new Vector3(0, jumpForce, 0);
        GetComponent<AudioSource>().Play();
    }

    // Get keyboard input as "down arrow key"
    if (Input.GetAxis("Vertical") < 0)
    {
        Squat();
    }
    else
    {
        RunCollider.isTrigger = true;
        SquatCollider.isTrigger = false;
    }
}
```

### Kinect Control

Listen to the Kinect input, identify its operation, and make the role perform according actions.

```c#
if (useKinectInput)
{
    if (gestureListener.IsJump())
    {
      body.velocity = new Vector3(0, jumpForce, 0);
      GetComponent<AudioSource>().Play();
    }
    if (gestureListener.IsSquat())
    {
      Squat();
    }
    else
    {
      RunCollider.isTrigger = true;
      SquatCollider.isTrigger = false;
    }
}
```

### Moving Effect

The effect of the role moving is generated by the motion of the track as well as the environment, which is defined in `TrackController.cs` and `Roll.cs`. Every pass, the speed of the character multiplies by 1.2 times.

```c#
private void Update() {
    transform.position = new Vector3(
        transform.position.x + trackCtrl.currentSpeed,
        transform.position.y,
        transform.position.z
    );
	//destroy one completed track
    if(transform.position.x > length) {
        Destroy(this.gameObject);

        trackCtrl.RunOver();
    }
	//create the first track
    if(!hasCreated && transform.position.x > 0) {
        hasCreated = true;

        CreateTrack();
    }
}
```

### Collision Detection

The character has three forms of motion states: run, jump and squat, each state of motion corresponds to a rigid body form. The Unity has interface to detect collision. Every time the character gets involved into a collision with a rigid body, the system identifies the rigid body. If the rigid body is a obstacle, the game is over.

```c#
private void OnTriggerEnter(Collider other)
{
    //if player hit the obstacle, game over.
    if (other.CompareTag("Obstacle"))
    {
        Death();
        gameCtrl.Gameover();
    }
}
```

### Track Generation

We have designed five different tracks. When reaching the half of a track, the game randomly generates a new track and destroys the former track. 

```c#
//generate infinite random track in track prefabs
private void CreateTrack() {
    int index = Random.Range(0, tracks.Length);
	
	//ensure current track not to be instantiated again before completing it
	if(index == trackIndex){
		index++;
	}
	trackIndex = index;
	
    float xPos = transform.position.x - length;
    Vector3 pos = new Vector3(xPos, transform.position.y, transform.position.z);
    Instantiate(tracks[index], pos, Quaternion.identity);
}
```

### Score System

When the character gets involved into collision with a prop, the game identifies the type of the prop (cartridge clip or medical box). The player gains 2 points when getting a cartridge clip and gains 20 points when getting a medical box.

```c#
public void AddScore(int scoreValue)
{
    score += scoreValue;
    GetComponent<AudioSource>().Play();
}
```

### Background Music

The BGM of the game speeds up as the game goes on.

```c#
// Update is called once per frame
void Update () {
    if (m_play)
    {
        source.Pause();
    }
    else {
        if (!source.isPlaying && !waitForNextClip) {
            int index = currClipIndex++;
            if (currClipIndex >= clips.Length)
                currClipIndex = 0;
            source.clip = clips[currClipIndex];
            waitForNextClip = true;
        }
        if (waitForNextClip) {
            waitTime += Time.deltaTime;
            if (waitTime >= 10.0f)
            {
                waitTime = 0.0f;
                waitForNextClip = false;
            }
        }
        if (waitForNextClip)
            return;
        if(!source.isPlaying)
            source.Play();
    }
}
```

## Evaluation

The project is inspired by a popular game *Temple Run*, and we are aimed to improve its interactivity by introducing *Kinect* into our project. By comparison, the project bears some advantages as well as some disadvantages.

### Advantages

1. **Interactivity:**

   As stated before, the game supports two ways of interaction —— traditional keyboard and mouse control as well as Kinect control. This can largely enhance players' immersion and realism, and provide players with a brand new gaming experience.

2. **Playability:**

   We well-designed the game mode to ensure the game is playful. The game starts relatively easy in the beginning to let players become familiar with game operations. Then the pace of the game gradually speeds up to increase the difficulty of the game. The trace of the game is randomly generated so that it is not boring or insipid for players to play. What's more, the game track is infinitely long and players has unlimited access to props to constantly challenge high scores.

3. **Extensibility：**

   We designed friendly and coherent program interfaces. If some developers want to add new features and functionalities, or just add some new tracks and props, it is fairly easy for them to achieve these since there is no need to refactor the code and the program logic.

4. **Friendly User Interface:**

   The main scene of this game is set in a city, and the scene material comes from a popular game `People's Unknown Battle Ground (PUBG)`, which can make players feel familiar. And the game avoid any bloody or violent content to make it children-friendly.

5. **Healthy lifestyle:**

   Traditionally, video games are regarded to be time-consuming, meaningless, and unhealthy for life. However, somatosensory games reverse the situation. As we see, this game gives people opportunity to work out without going outdoors. In addition, this game trains your body's flexibility, enhances responsiveness, strengthens speed, increases amount of exercise and enhances organism resistance.

### Disadvantages

1. **Limited Game Modes:**

   At present, this game supports only one game mode, which may reduce the game playability.

2. **Lack of Instruction:**

   In this game, we contain little game instruction. We only provide players with the basic keyboard operation instructions. Players have to explore the rules of the game, which may result in frustrate enthusiasm.

3. **Inaccurate Identification:**

   In the course of the game, sometimes the detection of Kinect is not so sensitive, making some actions and gestures delay in identification.

4. **Lack of Multi-person Interaction:**

   At current stage, this game supports single player only. If there are some friends playing together, there should be many PCs and Kinects, and this is an awful situation.

5. **Monotonous Game Scene:**

   The game currently supports only two game props: cartridge clip and medical box.

   In addition, in the current game, the score does not appear so important and appealing. Once the player fails, the score returns to zero.

## Further Refinement

Since the program is developed in a short time and is not well established and perfect enough, some further refinement and improvements are supposed to be taken into consideration, which are listed as follows.

1. **Design New Game Modes**

   At present, the game supports only **Endurance Mode** where players run constantly to challenge high scores. We can also design other modes such as **Race Mode** (the shortest time to reach the end), **Prop Mode** (acquire and use powerful props during Parkour), **Escape Mode** (avoid storms or toxins through Parkour), and etc.

2. **Add Game Instructions**

   I'm not in favor of the practice of showing players a page detailing the operations of the game, but I suggest that when a player first comes into contact with the game and encounters certain obstacles, the game slows down, and gives guidance to the player on how to overcome the obstacles. After completing these operations in person, the player may quickly get acquainted with the game and grow interest in this game.

3. **Improve Recognition Accuracy**

   In our practice, the program can only recognize the actions and gestures defined officially by Kinect. In a bid to simplify the detection, improve recognition accuracy, and avoid action conflicts, we can define some special actions and gestures by detecting the motions of specific joints and bones.

4. **Design Multiplayer Game Mode**

   Kinect is capable of detecting two or more people's actions though its results may not be very satisfactory. The introduction of Multiplayer Game Mode can enhance the entertainment of the game.

5. **Refine Game Scene**

   The current game scene is monotonous. We can refresh the game scene by adding more various style scenes (such as the rain, the sunset, the fog, etc.), constantly changing the back ground color and the styles of tracks and obstacles to make the gaming process more dynamic and delightful.

   We can also add some functional props such as protective shield, one more life, and score doubling props. This can make the game more playable.
