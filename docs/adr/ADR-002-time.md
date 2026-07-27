# ADR-002: Integer Time and Explicit Tick Ordering

## Status

Accepted.

## Context

Daily life, deadlines, aging, wages, travel, and succession require reproducible timing.

## Decision

Store authoritative time as integer game minutes. On each minute boundary, run due systems ordered by `SimulationPhase` and explicit `Order`.

## Consequences

- Equal seed, input, and system set produce reproducible timing.
- Accelerated time can be tested independently from render FPS.
- A future catch-up optimizer must preserve the same externally observable ordering.

## Verification

- Clock rollover tests
- Pause/advance tests in Application
- System-order tests
- Seven-day deterministic digest test in a future ticket

