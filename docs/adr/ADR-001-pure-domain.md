# ADR-001: Pure C# Authoritative Domain

## Status

Accepted.

## Context

The game must simulate distant characters and long campaigns without requiring a Unity GameObject for every entity.

## Decision

Authoritative world state and rules live in a pure C# Domain assembly. Unity objects present and control active scene representations but do not own persistent state.

## Consequences

- Domain tests can run without scenes.
- Save DTOs map explicitly from runtime state.
- Presentation cannot directly mutate state fields.
- Unity-specific conveniences must be adapted at boundaries.

## Verification

- Domain assembly definition has no Unity references.
- Unit tests create and advance a world without loading a scene.

