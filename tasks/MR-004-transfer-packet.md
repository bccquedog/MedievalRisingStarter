# TRANSFER — MR-004 — Codex to Kimi

## Identity

- Base commit: `45fe602`
- Current commit: pending (post-review fixes)
- Branch/worktree: `main`
- Current workflow state: `VISUAL_QA`
- Requested workflow state: `VISUAL_QA`

## Authority

- Canonical ticket: `tasks/MR-004-npc-relationship.json`
- Accepted ADR/schema: `docs/adr/ADR-004-relationships-and-schema-v2.md`
- Allowed paths: Domain/Application/Infrastructure Persistence/Presentation/Editor/Scenes/Tests/tasks/docs ADR
- Locked/shared resources: `Assets/Game/Scenes/StarterVillage.unity`, `SaveGameDto.cs`

## Completed outcome

- Player-visible: Mira follows a daily schedule and can be talked to with `E`; relationship values (affection/trust/respect) update and display on DebugHud.
- Technical: Relationship state lives on `WorldState` and persists through save schema v2; backward-compatible with v1 saves.

## Evidence

- Tests: EditMode suite 27/27 passed — `tasks/MR-004-editmode-results.xml`
- Review: Claude architecture/code review findings (F1, F2, F5) resolved — `tasks/MR-004-claude-review.md`
- Build: Unity batchmode EditMode run
- Screenshots/video/logs: pending Kimi visual QA

## Open items

- Blockers: none
- Accepted limitations: schedule waypoints are presentation config, not save state
- Decisions: schema v2 additive relationships; v1 remains loadable
- Findings: admin direct pushes previously bypassed branch protection; MR-004 merged via PR #1

## Compatibility

- Save: schema version 2
- Content schema: none
- Packages: unchanged
- Project settings: unchanged

## Requested next action

Perform visual QA on Mira NPC schedule movement, talk interaction feedback, and relationship HUD display.
