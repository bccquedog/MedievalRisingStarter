# Medieval Rising Starter

Unity 6.3 LTS starter repository for the first playable foundation.

This scaffold contains:

- Pure C# domain primitives and deterministic simulation
- Explicit simulation phases and cadences
- Character needs and a minimal game session
- Versioned local save DTOs with last-known-good backup recovery
- Unity bootstrap, debug HUD, placeholder scene builder, and keyboard/controller movement
- EditMode tests
- Agent-routing rules, ownership locks, task templates, and parallel-lane validation
- Static validation workflow

## Open it

1. Install Unity 6.3 LTS. The project is pinned to `6000.3.20f1`.
2. Open this folder from Unity Hub.
3. Allow Unity Package Manager to restore packages.
4. In Unity, choose **Tools → Medieval Rising → Create Starter Scene**.
5. Open `Assets/Game/Scenes/StarterVillage.unity`.
6. Press Play.

Controls:

- WASD, arrow keys, or left gamepad stick: move
- `F5`: save
- `F9`: load

The HUD is deliberately development-only. It displays game time, hunger, energy, save status, and the active workflow reminder.

## Validate before assigning work

```bash
python3 tools/validate_repo.py
python3 tools/validate_agent_task.py tasks/examples/MR-001-time-engine.json
```

## Agent workflow

Read these in order:

1. `AGENTS.md`
2. `agent-routing.json`
3. The assigned task JSON or Markdown ticket
4. Relevant ADRs and architecture sections

An agent must not edit production code unless the task is in `READY_FOR_IMPLEMENTATION` or `IMPLEMENTING`, names that agent as implementation owner, and assigns nonconflicting paths.

## Important boundary

This is starter architecture, not the completed game. It proves stable identity, time, needs, persistence, movement, tests, and agent operations. The master production plan defines the full destination.
