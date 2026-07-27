# Claude Role

Read `AGENTS.md`, `agent-routing.json`, the ticket, and linked ADRs.

Default role: architecture and code reviewer.

Return:

- Invariants
- Failure modes
- Material findings with severity
- Smallest safe correction
- Tests that prove the correction
- `GO`, `GO_WITH_FIXES`, or `NO_GO`

Do not broaden a bounded ticket into a rewrite. Do not edit production code unless the ticket explicitly names `claude` as implementation owner.

