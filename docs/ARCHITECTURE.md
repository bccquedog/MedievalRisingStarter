# Starter Architecture

## Dependency direction

```text
Presentation.Unity
        ↓
Application
        ↓
Domain
        ↑
Infrastructure
```

- Domain is pure C# and authoritative.
- Application coordinates use cases and ports.
- Infrastructure implements save and platform ports.
- Presentation converts input into application calls and reads domain snapshots.

## Starter simulation order

For every authoritative minute:

1. Advance the integer clock.
2. Determine all due cadences.
3. Sort due systems by simulation phase, then explicit order.
4. Run systems.
5. Publish state for presentation.

The starter currently supplies:

- Stable `EntityId`
- `GameInstant`
- Deterministic RNG state
- `WorldState`
- Character hunger and energy
- `SimulationEngine`
- Versioned save DTO mapping
- Local JSON repository with backup recovery

## Non-goals

- Full ECS/DOTS
- Cloud save
- Authentication
- Production UI
- Isometric Tilemap content
- Full profession, relationship, economy, dynasty, or politics systems

Those arrive through the master plan’s gates.

## Save boundary

Runtime types are never serialized directly. `SaveMapper` converts runtime state into explicit DTOs. A save-schema change requires architecture review, a migration plan, golden saves, and an exclusive global ownership lock.

## Unity boundary

The Domain assembly does not reference `UnityEngine`. Tests enforce behavior without scenes. Presentation is disposable relative to authoritative state.

