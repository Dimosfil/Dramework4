# Dramework4 Agent Entry Point

This repository contains the Dramework4 Unity source project.

When starting a new agent chat here:

1. Read `AGENTS.md`.
2. Read `agent-work/README.md`.
3. Read `agent-work/tasks/ACTIVE_TASK.md`, then read the task it points to.
4. Inspect `Assets/IG/package.json` and `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime` before editing source.
5. Check `git status --short` before and after work.

Current priority task:

- `agent-work/tasks/ACTIVE_TASK.md` -> `agent-work/tasks/DW_PACKAGE_RELEASE_VERIFICATION_TASK.md`

Rules:

- Preserve user changes. Do not reset, restore, or delete unrelated files.
- Keep framework changes scoped and package-friendly.
- Add/port tests with the framework change.
- If Unity or dotnet checks cannot run because of local environment paths, document the exact blocker.
- After implementation, update package version only if the task explicitly reaches the validated package-update step.
