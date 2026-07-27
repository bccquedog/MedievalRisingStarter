# AI Handoffs and Parallel Work

## Quick routing

| Situation | Next agent |
| --- | --- |
| Missing or conflicting player rule | Human/product clarification |
| Cross-system, save, time, ID, schema, NPC, economy, dynasty, or politics design | Claude review |
| Accepted bounded implementation | Codex |
| Approved repetitive schema-bound data | GLM to staging |
| Runnable UI, animation, camera, scene, or sorting change | Kimi visual QA |
| Accepted specialist findings | Codex integration |
| Final player-outcome judgment | Human acceptance |

## Transfer sequence

```text
Ticket
→ architecture if triggered
→ implementation
→ code review
→ playable build
→ visual QA if triggered
→ implementation owner fixes
→ human acceptance
→ merge
```

## Safe concurrency

Safe only with separate worktrees and nonoverlapping exclusive paths:

- Domain code and unrelated art
- Accepted importer and staged GLM content
- UI code and audio asset production after event IDs are stable
- Separate settlement scenes after shared world rules are accepted
- Read-only review of one branch while another branch implements an unrelated ticket

## Unsafe concurrency

- Two agents on the same ticket
- Two agents editing one scene, prefab, animator, schema, input actions, or Project Settings
- Save schema while a persistent system changes
- Time semantics while Needs depends on them
- Economy algorithm while balance data is being finalized
- Package upgrades beside ordinary feature work

## Receiving-agent checklist

Reject the transfer unless it includes:

- Base and current commit
- Branch
- Current and requested states
- Canonical ticket
- Allowed paths
- Locks
- Test evidence
- One bounded requested action

