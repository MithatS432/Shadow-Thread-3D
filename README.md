# Shadow-Thread-3D
A game where we hunt by wandering around

# Game Explanation
Trying to survive by hunting and avoiding collisions with animals.

# Controller
W,A,S,D or Arrow Keys to Movement
Space to Jump 
Run Fast to LeftShift
Fire1 to Attack
Alpha1& Alpha2 to Switch Wepaon(Sword and Bow)

# What I use in the game
1-)Abstraction-->The timeline doesn't know how the player is moving.It only triggers the "intro finished" event.PlayerMovement's internal logic is completely abstract.

2-)Encapsulation-->PlayerMovement keeps track of its own movement status (canMove).IntroController doesn't know how the movement works; it only enables/disables it.All the details of the movement are isolated within PlayerMovement.

3-)AI Heat Map-->Idle,Wander,Flee,Dead and A heatmap is a spatial density matrix that represents the frequency with which agents visit surrounding cells.
“Where do the animals roam most?”
“Where are the empty spaces?”
“Which areas does the player dominate?”

Thesis + graphs + statistics-->FSM,Data-driven AI,Emergent Behavior,Separation of Concerns,Rule-based system

4-)Timeline & Cinemachine-->The camera system consists of two virtual cameras

Intro Cinemachine Camera.Used only during the intro cinematic.Controlled via Timeline (Cinemachine Track).

Player Cinemachine CameraUsed during gameplay.Activated after the intro finishes.

Camera switching is handled by Cinemachine Priority, not by enabling/disabling cameras.

5-)Analysis & Profiler-->PlayerLoop Analysis (~2.5 ms) & Rendering Pipeline Analysis

The PlayerLoop represents Unity’s main execution cycle, responsible for updating gameplay logic, physics, animations, AI systems, and input processing.

Observed Value:PlayerLoop execution time: ~2.5 ms (CPU)

Interpretation:This value indicates a healthy CPU workload.The game logic, AI behaviors (FSM-based animal AI), physics updates, and input handling are efficiently structured.No excessive allocations or expensive per-frame operations were detected.

Key Contributors Inside PlayerLoop:Update() and FixedUpdate() methodsAI state transitions (Walk / Run)Character movement and collision checksHeatmap data sampling for AI analysis

Rendering Pipeline Analysis

The Rendering Pipeline is responsible for drawing all visible objects, lighting, shadows, and post-processing effects.

Observed Value:Rendering Pipeline cost: ~2.5 ms

Interpretation:Rendering workload is balanced and within real-time constraints.Scene complexity (terrain, animals, environment props such as trees, rocks, cabins) does not exceed GPU budget.No major overdraw or excessive draw calls were detected.

Rendering Characteristics:Forward rendering pathLimited real-time lightsEfficient use of materials and meshesNo unnecessary dynamic shadows on minor objects


Optimization Decisions

a-)FSM-based AI preferred over behavior trees to reduce overhead.

b-)CharacterController / Rigidbody usage optimized to avoid unnecessary physics calculations.

c-)Cinemachine used only during intro; disabled from runtime logic afterwards.

d-)No per-frame object instantiation in gameplay-critical systems.

e-)Audio and VFX triggered event-based, not frame-based.


## 📊 Project Images For Graphics

### System & AI Diagram
![Mermaid Diagram](Images%20For%20Graphics/Mermaid%20Diagrams.png)

### Night Environment
![Night Scene](Images%20For%20Graphics/Night.png)

### AI Path Visualization
![Paths](Images%20For%20Graphics/Paths.png)

### Alternative Path Analysis
![Paths2](Images%20For%20Graphics/Paths2.png)

