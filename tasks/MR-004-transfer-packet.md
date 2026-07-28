# TRANSFER — MR-004 — Codex to Claude

## Identity

- Base commit: `45fe602`
- Current commit: pending before PR
- Branch/worktree: `feature/MR-004-npc-relationship`
- Current workflow state: `CODE_REVIEW`
- Requested workflow state: `VISUAL_QA`

## Authority

- Canonical ticket: `tasks/MR-004-npc-relationship.json`
- Accepted ADR/schema: `docs/adr/ADR-004-relationships-and-schema-v2.md`
- Allowed paths: Domain/Application/Infrastructure Persistence/Presentation/Editor/Scenes/Tests/tasks/docs ADR
- Locked/shared resources: `Assets/Game/Scenes/StarterVillage.unity`, `SaveGameDto.cs`

## Completed outcome

- Player-visible: Mira follows a daily schedule and can be talked to with `E`.
- Technical: Relationship state lives on `WorldState` and persists through save schema v2.

## Evidence

- Tests: EditMode suite including `NpcScheduleTests`, `SocialTalkTests`, `RelationshipSaveTests`
- Build: Unity batchmode EditMode run
- Screenshots/video/logs: pending Kimi visual QA

## Open items

- Blockers: none
- Accepted limitations: schedule waypoints are presentation config, not save state
- Decisions: schema v2 additive relationships; v1 remains loadable
- Findings: admin direct pushes previously bypassed branch protection; this ticket uses a PR

## Compatibility

- Save: schema version 2
- Content schema: none
- Packages: unchanged
- Project settings: unchanged

## Requested next action

Review architecture and save-boundary correctness, then hand to Kimi for visual QA.
