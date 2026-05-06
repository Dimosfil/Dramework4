# Agent Work

This folder is the handoff area for AI-agent work in the Dramework4 repository.

Use it for:

- concrete implementation tasks;
- transfer notes from consumer projects;
- reference drafts that were validated elsewhere;
- reusable agent instructions;
- result notes after work is complete.

Do not put final runtime source here. Final Dramework4 runtime code belongs under:

- `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime`

Current task:

- `tasks/ACTIVE_TASK.md` -> `tasks/DW_PACKAGE_RELEASE_VERIFICATION_TASK.md`

Reference draft from Warehouse SIM:

- `references/warehouse-sim-dw-testing-draft/DWTestContainerDraft.cs`
- `references/warehouse-sim-dw-testing-draft/DWTestContainerDraftTests.cs`

Recommended start command for a new agent:

```powershell
git status --short
Get-Content .\AGENTS.md
Get-Content .\agent-work\README.md
Get-Content .\agent-work\tasks\ACTIVE_TASK.md
Get-Content .\agent-work\tasks\DW_PACKAGE_RELEASE_VERIFICATION_TASK.md
```
