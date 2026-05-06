# General Development Playbook

This is a reusable must-have plan for starting and maintaining a software project with an AI coding agent. Keep it short, explicit, and local to the repository.

## 1. Create The Agent Entry Point

Add `AGENTS.md` in the repository root.

It should contain:

- What the project is.
- How to restore context.
- Where durable memory lives.
- How to run, test, build, and inspect logs.
- What files/folders are safe working areas.
- Rules about git, commits, generated files, secrets, and destructive commands.

For other tools, add tiny redirect files when useful:

- `.github/copilot-instructions.md`
- `CLAUDE.md`
- `GEMINI.md`
- root `README.md` section: `For AI agents: read AGENTS.md first.`

Do not duplicate the whole instruction set everywhere. Keep `AGENTS.md` the source of truth.

## 2. Define Durable Memory

Create a local memory folder, for example:

```text
tools/project-memory/
```

Minimum contents:

- `README.md`: what memory exists and how to use it.
- `STUDY_PLAN.md`: roadmap for understanding the project.
- Local database or index, ignored by git when large/generated.
- A command to rebuild the index from source files.
- A command to save durable investigation notes.

Rule: important findings must be written locally, not only said in chat.

## 3. Add A Startup Script

Create one command that restores project context.

Example:

```powershell
.\tools\codex-start.ps1
```

It should print:

- `AGENTS.md`
- working agreements
- latest summary
- `git status --short`
- useful diff/stat information
- next recommended commands

The goal is that a new chat can become useful in minutes.

## 4. Write Working Agreements

Create:

```text
tools/CODEX_WORKING_AGREEMENTS.md
```

Include:

- Do not revert user changes unless explicitly requested.
- Treat dirty worktrees as normal.
- Whether the agent may commit or only edit.
- Preferred edit method.
- Main project folders.
- Where logs and generated files live.
- When to ask before expanding scope.

## 5. Write A Runbook

Create:

```text
tools/CODEX_RUNBOOK.md
```

It should answer:

- How to install dependencies.
- How to run the app.
- How to run tests.
- How to build.
- How to smoke-check the most important workflow.
- How to inspect logs.
- Known environmental caveats.

Every command should be copy-pasteable.

## 6. Keep Handoff Summaries

Create:

```text
tools/summary/
```

After meaningful work, write a concise summary named:

```text
YYYY-MM-DD_HH-mm-ss_CODEX_WORK_SUMMARY.md
```

Include:

- Current state.
- What changed.
- Commands run and results.
- Known failures or caveats.
- Next best steps.
- Current git status snapshot.

Summaries are for handoff. Durable knowledge belongs in project memory notes.

## 7. Map The Architecture Early

Before large changes, build a first map:

- Entry points.
- Modules/packages/assemblies.
- Dependency graph.
- Data flow.
- Runtime lifecycle.
- Config/secrets boundaries.
- Storage/database/API boundaries.
- Assets/templates/UI/routes as applicable.
- Tests and automation.

Do not try to understand everything in one pass. Build a searchable index and record small verified findings.

## 8. Establish Quality Gates

Every project should define minimum checks:

- Fast syntax/type check.
- Unit tests.
- Integration or smoke test for the main workflow.
- Build/package command.
- Lint/format command when available.
- Log inspection command for runtime systems.

If a check cannot run locally, document why.

## 9. Decide Git Rules

Write the policy clearly:

- Who commits.
- Branch naming.
- Whether generated files are committed.
- What must never be committed.
- How to handle unrelated dirty files.

Default safe rule: the agent edits and verifies; the user reviews and commits.

## 10. Protect Secrets And Generated Noise

Before real work starts:

- Review `.gitignore`.
- Ignore local databases, logs, cache folders, build outputs, temp folders.
- Never store credentials in summaries or notes.
- Document where secret/config examples live.

## 11. Make The First Useful Vertical Slice

For a new product, avoid spending the first phase only on structure.

Build one thin but real workflow:

- UI/API entry.
- Domain action.
- Persistence or external integration if needed.
- Test or smoke check.
- Logging/observability.

Then expand around a working spine.

## 12. End Every Session Cleanly

Before stopping:

- Run the relevant checks.
- Record failures honestly.
- Update summary.
- Save durable findings as notes.
- Leave `git status --short` understandable.

The next chat should never have to reconstruct the previous session from vibes.

## Minimal New Project Checklist

- [ ] Root `AGENTS.md`
- [ ] Root `README.md` with human setup and AI redirect
- [ ] `.gitignore`
- [ ] `tools/CODEX_WORKING_AGREEMENTS.md`
- [ ] `tools/CODEX_RUNBOOK.md`
- [ ] `tools/summary/`
- [ ] `tools/project-memory/README.md`
- [ ] `tools/project-memory/STUDY_PLAN.md`
- [ ] Startup script
- [ ] Test/build/run commands
- [ ] Secret/config policy
- [ ] First handoff summary

