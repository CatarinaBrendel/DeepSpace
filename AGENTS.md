# AGENTS.md --- Deep Space Programming / Exploration Game

## 1. Project Vision

This project is a sandbox programming and automation game centered
around:

-   deep-space exploration
-   spacecraft operation and maintenance
-   scientific observation
-   sample collection
-   sample analysis
-   automation
-   player-authored software

The player operates a research spacecraft and progressively writes
software to automate its systems.

The central fantasy is:

> I built the software that keeps this spacecraft running.

Player-written software is not merely a way to solve isolated
programming puzzles. It becomes part of the persistent spacecraft and is
itself a major form of progression.

The game is inspired by programming/automation games such as
Code:Terraform and the announced Delta 4, but should develop its own
identity around spacecraft simulation, exploration, scientific
discovery, and long-term player-created automation.

## 2. Core Design Principle

The most important rule is:

> The game presents situations, not programming puzzles.

Do **not** structure gameplay primarily as "write a loop to solve this
challenge." Create a simulated environment with understandable systems
and problems. The player may respond manually, write automation, or rely
on automation they previously built.

The satisfaction should come from watching systems created by the player
successfully handle situations.

## 3. Player Progression

Progression is not primarily a conventional XP/level system.

``` text
Knowledge
    ↓
Better player-written software
    ↓
More autonomous spacecraft
    ↓
Longer / more difficult missions
    ↓
More extreme environments
    ↓
Better scientific observations
    ↓
New discoveries
    ↓
New technology and hardware
    ↓
Greater exploration capability
```

The player's codebase is effectively part of the save game. Scripts must
persist between missions. Do not routinely reset the player's software
environment between scenarios.

A mature player environment might eventually resemble:

``` text
/scripts
    power.py
    thermal.py
    navigation.py
    survey.py
    science.py
    maintenance.py
    telemetry.py

/lib
    power_management.py
    diagnostics.py
    navigation_utils.py

/missions
    tau_ceti.py
```

## 4. Core Gameplay Loop

``` text
Prepare
   ↓
Travel
   ↓
Detect
   ↓
Investigate
   ↓
Sample
   ↓
Analyze
   ↓
Discover
   ↓
Upgrade
   ↓
Explore farther
```

Individual stages should interact with spacecraft systems and player
automation. Exploration should create new engineering problems rather
than merely unlock harder predefined coding exercises.

## 5. Spacecraft Simulation

The spacecraft should behave as an interconnected simulation.

### Power

Potential concepts:

-   generation
-   reactor / solar generation
-   batteries
-   consumption
-   maximum output
-   load shedding
-   emergency power
-   power priorities

### Thermal

Potential concepts:

-   component temperature
-   coolant
-   radiators
-   environmental temperature
-   solar exposure
-   thermal limits
-   emergency shutdown

Power and thermal management should strongly interact. Running
scientific instruments may consume significant power and generate
significant heat.

### Fuel / Reaction Mass

Potential concepts:

-   fuel
-   reaction mass
-   engine efficiency
-   burns
-   delta-v
-   maneuver planning

Exact simulation fidelity will be decided during implementation.
Gameplay clarity takes precedence over unnecessary physical complexity.

### Hull and Components

Potential concepts:

-   component condition
-   degradation
-   damage
-   maintenance
-   repair
-   spare parts
-   environmental hazards

Failures should generally produce consequences rather than immediate
game-over states.

### Data Storage

Scientific instruments generate data. Storage should eventually matter
for raw observations, processed observations, sample results, logs,
telemetry, software, and databases.

### Compute

The spacecraft computer is a gameplay resource. Potential resources
include CPU, memory, and storage.

Player scripts and scientific analysis may consume computational
resources. Hardware can eventually be upgraded.

Do not treat CPU/memory merely as decorative UI values if they are
introduced. They should have meaningful simulation consequences.

### Communications

Potential later systems:

-   bandwidth
-   communication delay
-   transmission windows
-   antenna orientation
-   signal strength
-   telemetry
-   scientific data transmission

Deep-space communications should eventually make local data processing
useful.

## 6. Time Is a Core Mechanic

The game uses simulated time.

Potential time scales:

``` text
PAUSED
×1
×10
×100
×1,000
×10,000
```

Long journeys become practical because players can accelerate time.

> High time acceleration should require confidence in automation.

Poor automation may allow problems to develop rapidly under accelerated
time. This creates a natural incentive for writing robust software.

## 7. Simulation Time vs Real Time

Simulation systems **must** operate against game time rather than
directly using wall-clock time.

Conceptually:

``` csharp
public interface IGameClock
{
    DateTimeOffset Now { get; }
    double TimeScale { get; }
}
```

Player script operations such as:

``` python
sleep(60)
```

should represent 60 seconds of simulation time, not 60 seconds of
real-world waiting.

Simulation determinism should be treated as an important architectural
goal. Avoid scattering real-time timers throughout simulation code.
Prefer controlled simulation stepping and/or discrete-event simulation.

## 8. Scientific Exploration

Science is one of the primary rewards for exploration.

Potential observations include:

-   stars
-   planets
-   moons
-   asteroids
-   comets
-   atmospheres
-   mineral deposits
-   unusual radiation
-   chemical signatures
-   organic compounds
-   anomalies
-   unknown objects

Scanning initially reveals incomplete information. Further observation
increases knowledge. The player decides which objects deserve
investigation.

Science should involve uncertainty: properties may be confirmed,
probable, uncertain, or unknown.

## 9. Samples

Physical samples are an important gameplay system.

Samples may come from asteroids, planetary surfaces, moons, atmospheres,
dust, ice, and potentially biological material.

Different laboratory processes reveal different information.

Analysis may consume:

-   time
-   power
-   CPU
-   storage
-   reagents
-   instrument lifetime
-   sample mass

Blindly running every possible analysis should not always be optimal.

Players should eventually be able to build their own sample-processing
pipelines.

## 10. Exploration Structure

The likely long-term direction is:

> Hand-authored anchor systems combined with procedural systems and
> handcrafted discoveries embedded within the broader universe.

Most discoveries should be scientifically ordinary. Rare discoveries
should therefore feel significant.

Do not place spectacular anomalies everywhere.

## 11. Failures

Failures should usually create engineering problems rather than
immediate game-over screens.

Prefer cascading, recoverable consequences. A mistake can become
knowledge permanently encoded into the player's software. This is
desirable.

Catastrophic failure may exist eventually, but should not be the default
consequence for ordinary programming mistakes.

## 12. Player Computer

The spacecraft contains a virtual computing environment.

The player should feel as though they are operating the spacecraft
through its computer rather than interacting with a collection of
abstract programming puzzles.

A Unix-like environment is currently the preferred direction.

Example:

``` text
captain@odyssey:~$ ls
bin  data  missions  samples  scripts  telemetry

captain@odyssey:~$ cd scripts
captain@odyssey:~/scripts$ python survey.py
Survey daemon started.
Listening for scanner events...
```

Potential shell commands include normal filesystem/process commands plus
ship-specific commands such as `ship`, `power`, `nav`, `scan`,
`samples`, `lab`, `telemetry`, and `status`.

The exact command set will emerge from implementation.

## 13. Programming Model

Python-like scripting is currently the preferred player-facing language.

Example:

``` python
from ship import power, thermal

reactor = power.get("reactor")
battery = power.get("battery")

while True:
    if battery.charge < 0.25:
        reactor.output = 0.80

    if thermal.core > 500:
        reactor.output = 0.30

    sleep(5)
```

However, the technical execution model has **not** yet been finalized.

Do not simply launch arbitrary unrestricted Python processes on the
host.

The scripting environment should support:

-   sandboxing
-   execution limits
-   CPU accounting
-   memory accounting
-   cancellation
-   simulated sleep
-   simulated time
-   events
-   deterministic behavior where practical
-   script lifecycle management
-   controlled filesystem access
-   diagnostics

Do not assume Python.NET, IronPython, external CPython, a custom
interpreter, or another implementation until this decision has
explicitly been made.

## 14. Programming Progression

Players may initially write simple polling loops. More advanced
capabilities may later include:

-   event-driven programming
-   scheduled tasks
-   multiple processes
-   libraries/modules
-   reusable components
-   message buses / inter-process communication
-   background services

The player's codebase may organically evolve from simple scripts into a
small software architecture.

Do not force advanced architecture too early.

## 15. Technology Stack

### Backend / Simulation

**C# / .NET**

The C# side owns authoritative game state and simulation, including
spacecraft systems, game clock, navigation, exploration, science,
samples, events, scripting integration, missions, and persistence.

### Frontend

**Vue + TypeScript**

The frontend is primarily a workstation/cockpit interface. It displays
authoritative state, hosts the editor and terminal, provides
science/mission interfaces, and sends player commands.

> Do not move authoritative simulation logic into Vue merely because it
> is convenient.

### Desktop Runtime

**Tauri**

The game should be designed as a desktop application from the beginning.
It is not primarily a browser game.

### Communication

Current preferred architecture:

``` text
Tauri
  │
  └── Vue / TypeScript
          │
          │ local API / WebSocket
          ▼
      .NET Server
          │
          ▼
       Engine
          │
          ▼
        SQLite
```

A local HTTP/WebSocket boundary is currently preferred over tightly
coupling game logic to Tauri-specific IPC because it provides a clean
frontend/backend separation and keeps the simulation independently
testable.

This is a direction rather than an immutable decision if implementation
evidence suggests a better approach.

## 16. Persistence

SQLite is currently the expected persistence mechanism.

Persistent data will eventually include game state, spacecraft state,
player filesystem/scripts, discoveries, samples, missions, scientific
knowledge, equipment, upgrades, and exploration history.

Do not create a large speculative database schema before the domain
model exists.

## 17. Conceptual Solution Structure

``` text
DeepSpace.sln

DeepSpace.Domain
├── Ship/
├── Components/
├── Science/
├── Navigation/
├── Exploration/
├── Samples/
└── Time/

DeepSpace.Engine
├── Simulation/
├── Events/
├── Scripting/
└── Missions/

DeepSpace.Persistence

DeepSpace.Server

DeepSpace.Web
└── Vue + TypeScript

src-tauri/
```

Names may change once the actual project is created.

Do not assume a class or directory exists simply because it appears in
this conceptual structure.

## 18. UI Direction

The UI should be:

-   minimal
-   functional
-   information-dense without becoming cluttered
-   dark
-   technical
-   credible
-   restrained
-   primarily a spacecraft workstation

Avoid excessive decorative sci-fi HUD elements and generic "AI-generated
sci-fi dashboard" aesthetics.

The interface should look like software designed to operate a spacecraft
rather than a movie HUD.

The explored layout includes:

-   persistent ship/system status
-   central contextual workspace
-   editor
-   terminal/console
-   mission information
-   events/logs
-   science interfaces
-   time controls

### No Map-Centric UI

A specific design decision has already been made:

> Do not make maps a prominent part of the main UI.

Earlier UI concepts included system/orbital maps. That direction was
rejected.

Navigation and exploration should instead be communicated through useful
instrumentation, target data, telemetry, distances, observations, visual
context, and appropriate specialized views when needed.

Do not automatically add galaxy maps, orbital maps, minimaps, or
persistent system maps to the primary interface.

A specialized navigation visualization may eventually exist if gameplay
requires it, but maps are not part of the main UI identity.

### Space Visuals

The UI may include contextual views of stars, planets, moons, asteroids,
and spacecraft surroundings.

These should support the exploration fantasy without replacing the
technical interface.

The visual viewport is not intended to become a manual flight simulator.
The player primarily operates the spacecraft through systems,
automation, instrumentation, and software.

## 19. MVP Philosophy

Do **not** begin by building:

-   a procedural galaxy
-   dozens of star systems
-   complex missions
-   a huge component catalog
-   advanced sample chemistry
-   a full Unix clone
-   a large upgrade tree

First prove that programming the spacecraft is enjoyable.

## 20. First Vertical Slice

The initial prototype should contain approximately:

-   one spacecraft
-   one generator
-   one battery
-   one consumer
-   basic thermal behavior
-   simulation clock with at least ×1 / ×10 / ×100 and likely pause
-   CodeMirror editor
-   player script that can read battery state and control the generator
-   ship status UI sufficient to observe the consequences of player code

Conceptual player script:

``` python
while True:
    if battery.charge < 0.30:
        generator.output = 1.0

    if battery.charge > 0.90:
        generator.output = 0.25

    sleep(5)
```

The first validation question is:

> Is writing a small `power.py`, running it, accelerating simulation
> time, and watching the spacecraft respond satisfying?

If the answer is no, adding planets, procedural generation, missions,
samples, or additional systems will not solve the fundamental problem.

## 21. Likely Expansion Order

``` text
Power
  ↓
Thermal
  ↓
Simulation time
  ↓
Script lifecycle
  ↓
Navigation
  ↓
Travel
  ↓
Scanning
  ↓
Scientific observations
  ↓
Sample collection
  ↓
Sample analysis
  ↓
Maintenance / degradation
  ↓
More complex exploration
```

This is guidance, not a mandatory roadmap.

## 22. Development Rules for AI Agents

### Inspect Before Assuming

Never invent existing implementation details.

Before proposing modifications to existing code, inspect the relevant
files.

Do **not** assume:

-   class names
-   interfaces
-   method signatures
-   namespaces
-   DTOs
-   API routes
-   database entities
-   Vue components
-   stores
-   directory structures
-   configuration
-   dependencies

If the required source has not been provided, ask for the exact relevant
file(s).

Preferred behavior:

> Please show me `SimulationEngine.cs` and `GameClock.cs`.

Not:

> Assuming your `SimulationManager` has a `Tick()` method...

### Request Narrow Context

When code is needed, request the smallest useful set of files. Do not
ask for the entire repository when a few files are sufficient.

### Preserve Existing Architecture

Do not casually introduce new abstraction layers, frameworks,
state-management libraries, dependency injection patterns, messaging
systems, or persistence technologies.

First understand the architecture already present.

### Avoid Premature Architecture

This project could easily become overengineered.

Avoid creating abstractions for hypothetical future features. Implement
the smallest architecture that cleanly supports current requirements.
Refactor when real patterns emerge.

### Simulation Logic Must Be Testable

Simulation systems should generally be executable without Vue, Tauri,
rendering, UI state, or real-world timers.

Prefer domain/engine tests that can advance simulation
deterministically.

### UI Is Not Authoritative

Never make Vue responsible for deciding simulation outcomes.

C# simulation determines outcomes; state/events are sent to the
frontend; Vue displays them.

### Gameplay Before Realism

Scientific and spacecraft concepts should feel credible, but this is a
game rather than aerospace engineering software.

When choosing between physically exact but opaque behavior and
simplified but internally consistent behavior that creates interesting
decisions, prefer the latter.

### Avoid Fake Complexity

Do not add numbers merely because spacecraft have many numbers.

Every exposed resource should ideally create a decision, tradeoff,
programming opportunity, diagnostic clue, or meaningful feedback.

### Automation Should Remain Optional Where Practical

The player should frequently be able to perform an operation manually
before automating it:

``` text
Understand system
      ↓
Operate manually
      ↓
Repeat operation
      ↓
Recognize pattern
      ↓
Automate
      ↓
Trust automation
```

Do not force players to automate something before they understand what
it does.

### Player Solutions Should Differ

Avoid APIs designed around one expected solution.

If batteries are running low, valid approaches might include increasing
generation, shutting down laboratory equipment, prioritizing life
support, reducing propulsion load, postponing analysis, or entering a
low-power state.

### Unexpected Situations Are Valuable

The game should periodically challenge assumptions in player automation.

Examples include degraded solar efficiency, damaged radiators, unusual
thermal environments, sensor failure, increased power consumption,
communication interruption, unexpected sample behavior, engine
degradation, and solar storms.

Do not simply generate random failures without understandable causes or
actionable consequences.

### Code Is Part of the Player's Story

Player scripts should accumulate history.

A mature script may contain:

``` python
# Added after Kepler-186 incident:
# Never start chromatography while radiator B is offline.
```

That is desirable.

The player's codebase should eventually tell the story of their
exploration career.

## 23. Long-Term Possibilities

These are ideas, not committed MVP features:

-   autonomous probes
-   multiple spacecraft
-   probe fleets
-   advanced scientific pipelines
-   remote spacecraft
-   communication delay
-   large datasets
-   local data processing
-   autonomous mission planning
-   programmable robotics
-   rover/drone operations
-   increasingly distant star systems
-   unknown phenomena
-   equipment manufacturing
-   custom spacecraft configurations
-   advanced onboard computing
-   inter-process communication
-   player-created libraries

Do not implement these merely because they are listed here.

## 24. Current Non-Decisions

The following areas remain intentionally undecided:

-   exact game title
-   exact Python/scripting runtime
-   detailed simulation tick architecture
-   final API shape
-   final persistence schema
-   exact physics fidelity
-   procedural generation implementation
-   mission framework
-   science classification model
-   component degradation model
-   final UI layout
-   exact progression economy
-   whether fleets will ultimately exist
-   exact communication model

Agents must not silently convert these open questions into established
project decisions.

When implementation reaches one of these decisions, discuss the
tradeoffs first.

## 25. Immediate Development Goal

Do not start with exploration.

Start with the smallest programmable spacecraft:

``` text
Tauri desktop application

        +

Vue / TypeScript UI

        +

CodeMirror editor

        +

.NET simulation

        +

Generator
Battery
Consumer

        +

Simulation time acceleration

        +

Player-written power automation
```

Once this loop feels satisfying, expand toward thermal management and
eventually exploration.

## 26. Guiding Question

When evaluating a feature, ask:

> Does this make the player feel more like they are programming and
> operating their own deep-space research vessel?

If not, reconsider whether the feature belongs in the game.
