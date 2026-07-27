# ADR-003: Explicit Versioned Save DTOs

## Status

Accepted.

## Context

Serializing runtime classes directly makes refactors break campaigns.

## Decision

Map runtime state to explicit serializable DTOs with independent schema versions. Preserve the previous valid save before replacing the active file.

## Consequences

- Mapping code is required.
- Save changes require migrations.
- Runtime refactors do not automatically alter disk format.

## Verification

- DTO round-trip tests
- Invalid schema rejection
- Backup recovery integration test when Unity test execution is available

