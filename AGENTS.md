# Medieval Rising Agent Instructions

## Required reading

Before work:

1. Read `agent-routing.json`.
2. Read the assigned ticket completely.
3. Read only the linked ADR and architecture sections.
4. Confirm the base commit, implementation owner, state, and allowed paths.
5. Run `python3 tools/validate_agent_task.py <task-file>`.

## Editing authority

- Only the named `implementation_owner` edits production code.
- Claude is review-only unless a ticket explicitly names it as implementation owner.
- Kimi produces visual QA reports and does not edit the integration branch.
- GLM writes only to assigned staging paths.
- Codex is the default implementation and integration owner.
- Stop when a task requires paths or decisions outside its authority.

## Architecture

- `MedievalRising.Domain` references no Unity assemblies.
- `Application` depends on `Domain`.
- `Infrastructure` implements persistence and platform adapters.
- `Presentation` owns Unity scenes, views, input, animation, and debug UI.
- UI and MonoBehaviours do not own authoritative simulation state.
- Persistent entities use stable IDs.
- Time is stored as integer game minutes.
- Simulation ordering is explicit.
- Save DTOs are separate from runtime types.

## Required completion evidence

Report:

- Final commit or working-tree state
- Files changed
- Tests: `PASS`, `FAIL`, `NOT RUN`, or `BLOCKED`
- Manual reproduction path
- Save/schema/package/project-setting impact
- Known limitations

Never say “tests should pass.”

## Stop conditions

Stop and prepare a transfer packet when:

- Requirements conflict or are missing.
- Stable IDs, time semantics, tick order, saves, or schemas must change.
- A new package or service is needed.
- A locked scene, prefab, Project Settings file, or package manifest is required.
- Two attempts fail to solve the same root problem.
- The ticket requires unassigned paths.

## Commands

Static checks:

```bash
python3 tools/validate_repo.py
python3 tools/validate_agent_task.py tasks/examples/MR-001-time-engine.json
```

Unity tests must be run from the pinned Unity editor or CI once Unity is available.

