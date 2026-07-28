# ADR-004: Relationships and Save Schema Version 2

## Status

Accepted.

## Context

The daily-life proof needs one persistent NPC and one relationship interaction. Relationship values are authoritative world state and must survive restart. Serializing them requires an explicit DTO change.

## Decision

1. Store undirected pairwise relationships as domain state on `WorldState`.
2. Persist them in save schema version 2 as additive relationship DTOs.
3. Keep schema version 1 loadable: missing relationships initialize empty.
4. Keep NPC schedule content as presentation/application configuration keyed by hour of day; schedule waypoints are not save state in this proof.

## Alternatives

- Runtime-only relationships: rejected because the day loop must prove save continuity.
- Schema version 1 mutation in place: rejected because it breaks golden-save discipline.

## Consequences

- SaveMapper and SaveGameDto own an exclusive lock for this change.
- Directed feelings require a future save-schema version.
- Future relationship systems must migrate through schema versions.

## Verification

- Relationship round-trip tests
- Schema v1 load still succeeds
- Talk action mutates only the intended pair
