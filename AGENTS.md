# AGENTS.md --- DeepSpace

## 1. Project Vision

DeepSpace is a sandbox programming and automation game about operating a
deep-space research vessel.

The game centers on:

- spacecraft operation and maintenance
- player-authored automation
- deep-space travel and exploration
- scientific observation
- probe operations
- physical sample collection
- laboratory analysis
- scientific cataloguing and discovery

The central fantasy is:

> I built the software that keeps this spacecraft running.

Player-written software is not merely a way to solve isolated
programming puzzles. It becomes part of the persistent spacecraft and is
itself a major form of progression.

The project takes inspiration from programming/automation games such as
Code:Terraform and the announced Delta 4, but should develop its own
identity around spacecraft simulation, exploration, scientific
discovery, and long-term player-created automation.

---

## 2. Primary Design Principle

The most important rule is:

> The game presents situations, not programming puzzles.

Do not structure gameplay primarily as "write a loop to solve this
challenge."

Create a simulated spacecraft with understandable systems, resources,
constraints, and problems. The player may operate a system directly,
write automation for it, or rely on automation they previously built.

The satisfaction should come from watching software created by the
player successfully operate a spacecraft and handle situations that
would otherwise require attention.

A second fundamental rule is:

> First make a spacecraft worth automating. Then give the player reasons
> to automate it.

Missions, probes, laboratories, discoveries, and progression are
downstream systems. They must be built on top of a convincing spacecraft
simulation rather than becoming disconnected minigames.

---

## 3. Authority Boundary

The C# simulation is authoritative.

Conceptually:

```text
Player script
     │
     │ commands
     ▼
C# Domain
     │
     ▼
Simulation Engine
     │
     ▼
Authoritative state
     │
     ├── UI state
     ├── events
     └── script observations
```

Player scripts do not directly mutate simulation state.

For example, Python must not do this conceptually:

```python
battery.charge = 0.8
fuel.amount = 50
```

Instead, scripts issue commands to simulated components:

```python
generator.start()
engine.set_throttle(0.25)
radiator.deploy()
```

The C# simulation determines whether the command is valid and what
physical consequences follow.

Likewise, Vue is not authoritative. Vue displays state and sends
commands; it does not decide simulation outcomes.

---

## 4. Development Strategy

Build DeepSpace from the spacecraft outward.

Model systems to the level needed to create interesting player decisions and scripting opportunities—not to reproduce real-world engineering.

Do not begin with missions, procedural exploration, laboratories, or a
large scripting API.

The dependency order is:

```text
Simulation Foundation
        ↓
Electrical Power
        ↓
Ship Components
        ↓
Thermal
        ↓
Propulsion
        ↓
Basic Flight
        ↓
Life Support
        ↓
Resources / Storage
        ↓
Player Scripting Runtime
        ↓
Navigation / Destinations
        ↓
Probes
        ↓
Science
        ↓
Missions / Progression
```

This is a dependency roadmap, not a requirement that every item become a
large isolated development phase.

Each step should produce a small testable vertical slice before moving
on.

---

## 5. Current Immediate Goal --- Cold Ship

The current development focus is the spacecraft itself.

Do not start implementing missions, probes, samples, laboratories, or
science yet.

The first meaningful backend milestone is:

> A cold spacecraft can be powered up and its electrical state evolves
> correctly over simulation time.

Initial model:

```text
Spacecraft
│
└── Electrical
    ├── Generator
    ├── Battery
    └── Main Bus
```

Initial state might resemble:

```text
GENERATOR       OFF
MAIN BUS        OFF
BATTERY         87%
GENERATION      0.0 kW
LOAD            0.0 kW
```

Starting the generator should eventually lead to a meaningful state
transition such as:

```text
Generator
OFF → STARTING → ONLINE

               ↓

Main electrical bus
OFF → ENERGIZED

               ↓

Powered components may operate
```

The first implementation should prove:

- spacecraft state exists in the C# domain
- simulation time can advance deterministically
- generator state can change
- electrical generation is represented in power units
- electrical demand is represented in power units
- battery energy changes over time
- power surplus charges the battery
- power deficit discharges the battery
- insufficient supply has an explicit consequence
- all of this can be tested without Vue or Tauri

Do not introduce speculative complexity beyond what is required for this
milestone.

---

## 6. Simulation Foundation

Before the ship grows, establish a small simulation substrate.

Conceptually:

```text
Game
└── Spacecraft
    └── Components

Simulation
├── Game time
├── deterministic advancement
├── commands
├── state transitions
├── events
└── resource accounting
```

Simulation systems must use game time rather than scattered wall-clock
timers.

Deterministic simulation is an architectural goal.

A test should be able to do something conceptually like:

```csharp
simulation.Advance(TimeSpan.FromSeconds(10));
```

and obtain the same result every time given the same starting state and
commands.

Do not create a large generic simulation framework before concrete
systems demonstrate the abstractions actually needed.

---

## 7. Electrical Power

Electrical power is the first major spacecraft system because almost
every later system depends upon it.

Use meaningful physical quantities where they improve gameplay clarity:

```text
Generator output       kW
Component demand       kW
Battery capacity       kWh
Battery stored energy  kWh
Battery state of charge %
```

The fundamental relationship is:

```text
Generation - Demand = Electrical Balance

positive balance
    → available surplus
    → battery may charge

negative balance
    → battery supplies deficit

generation + available battery insufficient
    → power shortage / unavailable loads
```

Do not treat battery percentage as a generic pool from which every
component directly subtracts arbitrary values.

The battery stores energy. Components consume power over time.

The exact electrical model may remain simplified, but it should be
internally consistent.

### First powered consumer

Once generation, demand, and battery behavior work, add exactly one
powered component.

The purpose is to prove that components can participate in the
electrical simulation before introducing many ship systems.

---

## 8. Ship Components

The spacecraft will eventually contain many components, but avoid
inventing a large universal component hierarchy prematurely.

Useful component states may include:

```text
OFF
STARTING
ONLINE
STOPPING
FAULT
```

Not every component must use every state.

Potential early consumers include:

- flight computer
- thermal control
- life support
- propulsion controller

Starting a component should create real resource demand.

A cold-start sequence should eventually be meaningful because powering
every system simultaneously may not always be possible or desirable.

---

## 9. Thermal System

Thermal management follows electrical power.

Operating components generate heat.

Conceptually:

```text
Generator ─────┐
Computer ──────┼──→ Heat → Coolant → Radiator → Space
Engine ────────┘
```

Potential concepts include:

- component heat generation
- coolant temperature
- thermal capacity
- coolant loops
- radiators
- radiator deployment
- thermal limits
- shutdown conditions

Power and thermal behavior should strongly interact.

Do not simulate thermodynamics at engineering-software fidelity unless
that complexity creates useful gameplay.

The thermal milestone is:

> Electrical activity creates heat, and thermal-control components can
> manage it over simulation time.

---

## 10. Propulsion

Propulsion should be added only after power and basic thermal behavior
exist.

Initial structure may resemble:

```text
Propulsion
├── Main Engine
└── Fuel / Propellant Storage
```

Engine operation should depend on existing ship systems rather than
being an isolated toggle.

Potential dependencies:

```text
Electrical power ─┐
                  │
Fuel / propellant ├──→ Engine → Thrust
                  │
Thermal state ────┘
```

The initial engine does not need a highly detailed ignition simulation.

It should establish meaningful consequences:

- startup state
- electrical demand
- fuel consumption
- heat generation
- throttle
- thrust

Conceptual player interaction:

```python
engine.start()
engine.set_throttle(0.25)
```

The C# simulation determines whether the engine can start and what
happens afterward.

---

## 11. Basic Flight

DeepSpace is not intended to become a manual flight simulator.

Initial flight physics should be sufficient to make propulsion
physically meaningful.

Potential initial state:

```text
Position
Velocity
Acceleration
Mass
```

Engine thrust changes velocity. Fuel consumption changes mass. Time
changes position.

Start simple.

Do not implement full orbital mechanics merely because the setting is
space. Introduce additional physical fidelity only when it produces
useful decisions or supports navigation gameplay.

The milestone is:

> Firing the engine causes a measurable change in spacecraft motion.

---

## 12. Life Support / Habitat

Once the basic vessel can power itself, manage heat, and move, introduce
the habitable environment.

The current UI already anticipates:

- O₂
- CO₂
- pressure
- temperature
- humidity

Potential systems include:

- oxygen supply
- CO₂ scrubbing
- atmospheric circulation
- pressure management
- temperature regulation

Life support must consume resources and electrical power.

Failures should usually develop over time rather than instantly kill the
player. This gives automation time to detect and respond to
deteriorating conditions.

---

## 13. Resources and Storage

Before probes and laboratories, establish reusable physical resource
concepts.

Potential resources:

- fuel
- reaction mass
- oxygen
- water
- reagents
- replacement parts
- physical samples

Potential storage types:

- tanks
- general cargo
- sample containers
- cryogenic storage
- biological containment
- data storage

Resources should have meaningful quantities, capacities, and locations
where those distinctions create gameplay.

Do not build a generalized inventory framework until concrete
requirements justify it.

---

## 14. Player Scripting

Python-like scripting remains the preferred player-facing language.

The exact runtime remains undecided.

Do not assume CPython, Python.NET, IronPython, a custom interpreter, or
another implementation until explicitly decided.

The runtime must not simply launch unrestricted host Python.

Required concerns include:

- sandboxing
- execution limits
- CPU accounting
- memory accounting
- cancellation
- simulated sleep
- simulation time
- events
- script lifecycle management
- controlled filesystem access
- diagnostics
- deterministic behavior where practical

### Scripting should adapt to the simulation

Do not design the domain around convenient Python syntax.

First make systems work through ordinary C# commands and deterministic
engine tests.

Then expose those capabilities through the scripting layer.

For example, establish a working C# operation first, then expose:

```python
generator.start()
engine.start()
engine.set_throttle(0.5)
```

The scripting runtime is an adapter to the spacecraft simulation, not
the simulation itself.

### Programming progression

Players may begin with simple polling:

```python
while True:
    if battery.charge() < 0.4:
        generator.start()

    sleep(10)
```

Later capabilities may include:

- event-driven programming
- scheduled tasks
- multiple processes
- libraries/modules
- reusable components
- background services
- inter-process communication

Do not force advanced software architecture early.

---

## 15. Navigation and Exploration

Navigation comes after the spacecraft can actually travel.

Potential concepts:

- destinations
- celestial bodies
- sites
- distance
- trajectories
- travel time
- resource requirements

Avoid making the primary UI map-centric.

Navigation should primarily be communicated through useful
instrumentation, target information, telemetry, distances, observations,
and specialized views where needed.

Do not automatically add persistent galaxy maps, orbital maps, or
minimaps.

A specialized navigation visualization may eventually exist if gameplay
demonstrates a need.

---

## 16. Probes

Probes are a later extension of systems already established on the
spacecraft.

A probe may eventually have:

```text
Probe
├── Power
├── Fuel
├── Instruments
├── Sample Containers
└── Communication
```

The intended science chain begins here:

```text
Target location
      ↓
Probe deployment
      ↓
Travel
      ↓
Observation / collection
      ↓
Probe recovery / transfer
      ↓
Sample storage
```

Probe actions should consume time and resources.

Exploration itself should be programmable.

Conceptual future scripting:

```python
probe.deploy(site)
probe.scan()
probe.collect()
probe.return_to_ship()
```

Do not implement this until the spacecraft systems it depends upon
exist.

---

## 17. Science

Science is a major long-term purpose for the spacecraft, but it is
downstream of the ship simulation.

The intended pipeline is:

```text
LOCATION
   ↓
PROBE
   ↓
SAMPLE
   ↓
TRANSFER
   ↓
STORAGE
   ↓
LAB
   ↓
ANALYSIS
   ↓
CATALOGUE
```

Every stage should interact with established resources.

The player must eventually consider:

- electrical power
- elapsed simulation time
- sample mass
- reagent availability
- storage location
- storage capacity
- storage conditions
- instrument availability
- CPU/data storage where appropriate

### Science is script-driven

The Science workspace is primarily an observation and results interface.

Do not turn it into a point-and-click laboratory minigame full of
`RUN ANALYSIS` buttons.

The player's scripts should command laboratory equipment.

Conceptually:

```python
sample = samples.next_unprocessed()
result = spectrometer.analyze(sample, mass=2.0)
catalogue.store(result)
```

The UI shows what the scripts caused:

- incoming samples
- stored samples
- processing state
- instrument activity
- power use
- reagent state
- analysis progress
- measurements
- results
- catalogue information
- responsible script

### Physical samples

Samples are finite physical objects rather than abstract unlock tokens.

Potential properties include:

```text
Origin
Mass
Temperature
Integrity
Contamination
Container
Storage location
Storage requirements
```

Analyses may consume sample material.

Different analyses may be destructive, partially destructive, or
non-destructive.

Do not implement all of these properties at once. Add them when required
by actual gameplay.

### Laboratory instruments

Prefer exposing physical capabilities rather than high-level
answer-producing commands.

Good direction:

```python
spectrometer.scan(sample)
microscope.image(sample)
chromatograph.separate(sample)
centrifuge.process(sample)
incubator.culture(sample)
sequencer.sequence(sample)
```

Avoid APIs such as:

```python
lab.search_for_life(sample)
```

The interesting part is deciding how to investigate a sample and how to
automate that process.

### Catalogue

Keep these concepts distinct:

- **Sample** --- physical material
- **Analysis** --- measurement produced by an instrument
- **Finding** --- interpretation supported by evidence
- **Catalogue entry** --- accumulated scientific knowledge

The exact catalogue and scientific-certainty model remains undecided.

Do not implement it prematurely.

---

## 18. Missions and Progression

The real mission framework should come after the underlying spacecraft
and exploration systems are enjoyable.

During development, use small scenarios rather than a full mission
framework.

Examples:

```text
Start the generator
Maintain battery charge
Control temperature
Perform an engine burn
Travel to a target
Deploy a probe
Recover a sample
Analyze a sample
```

Later, missions orchestrate existing systems instead of inventing
special mission-only mechanics.

Example future mission:

> Survey a target body, collect geological samples from two sites,
> preserve sufficient material during return transit, and complete the
> required analyses.

Progression may eventually unlock:

- improved spacecraft hardware
- better instruments
- additional scripting capabilities
- larger storage
- improved compute
- more capable probes
- more distant destinations

---

## 19. Player Progression

Progression is not primarily a conventional XP/level system.

```text
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

The player's codebase is part of the save game and part of their
progression.

Scripts should persist between missions.

A mature environment may eventually resemble:

```text
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

The player's codebase should eventually tell the story of their
exploration career.

For example:

```python
# Added after Kepler-186 incident:
# Never start chromatography while radiator B is offline.
```

That is desirable.

---

## 20. Time

The game uses simulated time.

Time acceleration remains an intended concept, but final controls and
semantics are not yet fixed.

The current UI may visually contain pause / `1×` / `10×` / `100×`
controls. Treat those as provisional until the backend simulation
establishes the final behavior.

Simulation systems must use game time rather than directly relying on
wall-clock time.

Conceptually:

```csharp
public interface IGameClock
{
    DateTimeOffset Now { get; }
}
```

Player script operations such as:

```python
sleep(60)
```

should eventually represent 60 seconds of simulation time, not 60
seconds of real-world waiting.

High time acceleration should eventually reward confidence in robust
automation.

---

## 21. Failure Philosophy

Failures should usually create engineering problems rather than
immediate game-over screens.

Prefer cascading, understandable, recoverable consequences.

Examples:

- battery depletion causes systems to become unavailable
- excessive heat forces shutdown
- cryogenic storage warming damages samples
- poor resource planning leaves insufficient fuel
- reagent depletion interrupts analysis
- a damaged radiator reduces thermal capacity

Failures should have causes the player can understand and consequences
they can respond to.

Do not add arbitrary random failures merely to create difficulty.

A programming mistake becoming knowledge permanently encoded into the
player's software is desirable.

---

## 22. Player Computer

The spacecraft contains a virtual computing environment.

The player should feel as though they operate the vessel through its
computer rather than through a collection of abstract programming
puzzles.

A Unix-like environment remains the preferred direction.

Example:

```text
operator@deepspace:~$ ls
bin  data  scripts  telemetry

operator@deepspace:~$ cd scripts
operator@deepspace:~/scripts$ python power.py
Power automation started.
```

Potential shell commands may eventually include normal
filesystem/process commands plus ship-specific commands.

The exact command set should emerge from implementation rather than
being designed speculatively.

---

## 23. Technology Stack

### Backend / Simulation

**C# / .NET**

The C# side owns authoritative game state and simulation.

Current solution direction:

```text
DeepSpace.Domain
DeepSpace.Engine
DeepSpace.Persistence
DeepSpace.Server
DeepSpace.Engine.Tests
DeepSpace.Web
DeepSpace.Web/src-tauri
```

### Frontend

**Vue + TypeScript**

The frontend is the spacecraft workstation interface.

It displays authoritative state, hosts the editor and terminal, and
provides ship/science/mission/database workspaces.

### Desktop Runtime

**Tauri**

DeepSpace is a desktop application, not primarily a browser game.

### Communication

Preferred architecture:

```text
Tauri
  │
  └── Vue / TypeScript
          │
          │ local HTTP / WebSocket
          ▼
      .NET Server
          │
          ▼
        Engine
          │
          ▼
        SQLite
```

A local HTTP/WebSocket boundary is preferred over tightly coupling the
simulation to Tauri IPC because it preserves a clean frontend/backend
boundary and keeps the simulation independently testable.

This remains a direction rather than an immutable decision if
implementation evidence suggests otherwise.

---

## 24. Persistence

SQLite is currently the expected persistence mechanism.

Persistent data may eventually include:

- game state
- spacecraft state
- player filesystem and scripts
- discoveries
- samples
- scientific knowledge
- missions
- equipment
- upgrades
- exploration history

Do not create a large speculative database schema before the domain
model exists.

Persistence should follow proven domain requirements rather than drive
them.

---

## 25. UI Direction

The UI should be:

- minimal
- functional
- information-dense without becoming cluttered
- dark
- technical
- credible
- restrained
- primarily a spacecraft workstation

Avoid:

- decorative movie-HUD styling
- excessive glow
- glassmorphism
- huge rounded cards
- generic AI-generated sci-fi dashboard aesthetics

The interface should look like software designed to operate a
spacecraft.

### Current workspace model

Persistent shell:

```text
Top navigation

Left:
    Ship Systems
    Habitat

Center:
    active workspace

Right:
    Mission
    Telemetry
    Events

Bottom:
    status / simulation controls
```

Top-level workspaces:

```text
SHIP
SCIENCE
MISSIONS
DATABASE
```

### SHIP

The primary programming workspace.

Contains:

- multiplex/multitab editor
- terminal
- ship automation workflow

### DATABASE

The onboard knowledge base.

It is not a SQL/SQLite administration interface.

Primary categories:

- Scripts
- Reference
- API
- Guides

Science and Missions are not categories inside Database because they
have their own top-level workspaces.

### SCIENCE

Science is primarily an operational observation/results workspace.

It may eventually display:

- sample inventory
- sample storage
- laboratory activity
- instrument state
- reagent levels
- analysis progress
- measurements
- results
- catalogue records
- responsible scripts

The UI observes the laboratory pipeline. Scripts operate it.

### MISSIONS

Mission objectives, destinations, expedition progress, and mission
history belong here.

### No map-centric UI

Do not make maps a prominent part of the primary UI.

Specialized navigation visualization may eventually exist if gameplay
requires it, but maps are not the product's main visual identity.

### No cockpit/outside-window dependency

The main workspace is not a cockpit window or persistent outside-space
view.

Contextual scientific or astronomical imagery may exist where useful,
but the primary fantasy is operating the vessel through instrumentation
and software.

---

## 26. Development Rules for AI Agents

### Inspect Before Assuming

Never invent existing implementation details.

Before proposing modifications to existing code, inspect the relevant
files.

Do not assume:

- class names
- interfaces
- method signatures
- namespaces
- DTOs
- API routes
- database entities
- Vue components
- stores
- directory structures
- configuration
- dependencies

If the required source has not been provided, ask for the exact relevant
file or files.

Preferred:

> Please show me `Spacecraft.cs` and the current simulation entry point.

Avoid:

> Assuming your `SimulationManager` has a `Tick()` method...

### Request Narrow Context

When code is needed, request the smallest useful set of files.

Do not ask for the entire repository when a few files are sufficient.

### Preserve Existing Architecture

Do not casually introduce new:

- abstraction layers
- frameworks
- state-management libraries
- dependency-injection patterns
- messaging systems
- persistence technologies

Understand the existing architecture first.

### Avoid Premature Abstraction

This project can easily become overengineered.

Do not create generic abstractions for hypothetical future systems.

For example, do not immediately introduce a universal `Operation<T>`,
generic resource graph, elaborate component inheritance tree, or
event-sourcing architecture because future probes/labs might use it.

Implement concrete systems first. Refactor when repeated patterns
actually emerge.

### Work in Small Vertical Slices

Prefer:

```text
Implement
    ↓
Test
    ↓
Observe
    ↓
Commit
    ↓
Next small system
```

Do not implement several major subsystems simultaneously.

### Simulation Logic Must Be Testable

Simulation systems should execute without:

- Vue
- Tauri
- rendering
- UI state
- real-world timers

Prefer deterministic domain/engine tests.

### UI Is Not Authoritative

C# determines outcomes.

Vue displays them.

### Gameplay Before Realism

Spacecraft and scientific concepts should feel credible, but DeepSpace
is a game rather than aerospace engineering software.

When choosing between physically exact but opaque behavior and
simplified but internally consistent behavior that creates interesting
decisions, prefer the latter.

### Avoid Fake Complexity

Do not expose a resource merely because a real spacecraft has it.

Every exposed value should ideally create at least one of:

- a decision
- a tradeoff
- a programming opportunity
- a diagnostic clue
- meaningful feedback

### Manual Operation Before Automation

Where practical:

```text
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

Do not force automation before the player understands the system being
automated.

### Player Solutions Should Differ

Avoid APIs designed around one expected solution.

If electrical power is low, possible responses might include:

- increase generation
- shut down laboratory equipment
- postpone propulsion
- prioritize life support
- disable nonessential compute
- enter a low-power mode

The simulation should provide constraints; the player decides how to
respond.

### Unexpected Situations Are Valuable

Later, the game should challenge assumptions in player automation
through understandable changes such as:

- degraded generation
- damaged radiators
- unusual thermal environments
- sensor failure
- increased power consumption
- communication interruption
- sample-storage problems
- engine degradation

Do not introduce these until the basic systems they exercise are
established.

---

## 27. Coding Style and Formatting

### General

- Prefer readable, compact code.
- Avoid unnecessary vertical expansion.
- Follow the repository `.editorconfig` and formatter configuration.
- Formatting changes should not obscure functional changes.

### C

Prefer compact expressions when they remain readable and fit comfortably
on one line.

Prefer:

```csharp
var battery = new Battery(capacity: 100, charge: 75);
```

Do not split constructor calls, method calls, argument lists, or similar
expressions merely because they contain multiple arguments.

Use multiline formatting when the expression is genuinely long or
structurally clearer that way.

Preserve intentionally compact existing code when editing nearby code.

Use `dotnet format` for standard Roslyn formatting, but do not rely on
it to decide when compact expressions should be collapsed or expanded.

### TypeScript / Vue / JSON / Markdown

Follow the repository Prettier configuration.

Prefer Prettier's output rather than manually fighting its formatting.

---

## 28. Current Non-Decisions

The following remain intentionally undecided:

- exact player scripting runtime
- detailed simulation tick architecture
- final scripting API
- final persistence schema
- exact physics fidelity
- exact electrical failure/load-shedding behavior
- detailed propulsion model
- orbital mechanics depth
- procedural generation implementation
- mission framework
- science classification model
- catalogue/evidence model
- component degradation model
- final progression economy
- exact communications model
- whether fleets/multiple spacecraft will exist
- final simulation time-control semantics

Agents must not silently convert these open questions into established
decisions.

Discuss tradeoffs when implementation reaches them.

---

## 29. Near-Term Roadmap

### Milestone 0 --- Simulation Foundation

Prove:

- `Game` / spacecraft state can exist
- simulation can advance deterministically
- commands can cause state transitions
- tests can run without frontend/runtime dependencies

### Milestone 1 --- Cold Ship / Electrical Power

Implement:

- one spacecraft
- one generator
- one battery
- one electrical bus
- electrical generation
- electrical demand
- battery charge/discharge over time
- insufficient-power behavior

Validation question:

> Does the electrical simulation behave consistently and give us
> something useful to automate?

### Milestone 2 --- First Powered Component

Add one electrical consumer with operational state.

Prove that turning systems on/off affects demand and spacecraft state.

### Milestone 3 --- Thermal

Add enough thermal behavior that operating equipment creates heat and
thermal control matters.

### Milestone 4 --- Propulsion

Add:

- main engine
- fuel/propellant
- throttle
- thrust
- electrical demand
- heat generation

### Milestone 5 --- Basic Flight

Make thrust affect spacecraft motion over simulation time.

### Milestone 6 --- Habitat

Add the first meaningful life-support loop.

### Milestone 7 --- Resources / Storage

Establish physical resources and storage needed by later
exploration/science systems.

### Milestone 8 --- Player Scripting Runtime

Expose proven spacecraft capabilities through the player-facing
scripting environment.

### Milestone 9 --- Navigation

Introduce destinations and travel planning.

### Milestone 10 --- Probes

Introduce programmable remote observation and sample collection.

### Milestone 11 --- Science

Implement the physical pipeline:

```text
Probe → Sample → Transfer → Storage → Lab → Analysis → Catalogue
```

### Milestone 12 --- Missions / Progression

Use the established systems to create structured expeditions and
long-term progression.

---

## 30. Guiding Questions

When evaluating a feature, ask:

> Does this make the player feel more like they are programming and
> operating their own deep-space research vessel?

When evaluating architecture, ask:

> Do we need this abstraction for the system we are implementing now, or
> are we designing for an imagined future?

When deciding what to build next, ask:

> What is the smallest additional spacecraft behavior that creates a new
> meaningful decision or programming opportunity?

For the current stage, the answer is:

> Make the cold spacecraft electrically alive.
