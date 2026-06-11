# Handoff Summary: UnityPackage Workflow

Date: 2026-06-11

## Context

Project: Dramework4, Unity runtime framework/package for HappyCoder projects.

Unity support rule was updated in `AGENTS.md`: treat the framework as Unity
`2022.3+`; current project version `2022.3.62f2` is supported and works.

Main active topic: prepare a repeatable `unitypackage` release workflow.

## Completed

- Ran compact project startup restore with `tools/agent-start.ps1`.
- Applied instruction kit update from `2026.06.11.1` to `2026.06.11.3`.
- Applied migrations:
  - `2026.06.11.2__add_feature_workflow_contracts`
  - `2026.06.11.3__add_feature_planning_hierarchy`
- Added feature workflow contract rules to:
  - `AGENTS.md`
  - `tools/AGENT_WORKING_AGREEMENTS.md`
- Updated `tools/project-memory/README.md` to document feature contracts.
- Added `tools/project-memory/feature-workflow-contract.template.md`.
- Replaced the placeholder task in `tools/project-memory/pending-tasks.md` with
  `Prepare UnityPackage release workflow`.
- Added `tools/project-memory/unitypackage-workflow-contract.md`.

## Current Git State

Branch: `main`, tracking `origin/main`.

Uncommitted changes:

- `AGENTS.md`
- `tools/AGENT_WORKING_AGREEMENTS.md`
- `tools/project-memory/README.md`
- `tools/project-memory/instruction-kit.json`
- `tools/project-memory/pending-tasks.md`
- `tools/project-memory/feature-workflow-contract.template.md`
- `tools/project-memory/unitypackage-workflow-contract.md`

The automatic commit/push after `gi обновить` was not done because `AGENTS.md`
already had a separate Unity support rule change before the instruction-kit
update, and both changes touched the same file.

## Verification

- `tools/check-instruction-kit-updates.ps1` reports installed and available
  versions both as `2026.06.11.3`, with no pending migrations.
- `git diff --check` passed for the scoped project-memory task files.
- Full `git diff --check` also passed, with only Git line-ending warnings about
  future `LF` to `CRLF` replacement.

## Next Steps

For the `unitypackage` task:

1. Read `Assets/IG/package.json`, package layout, README, and relevant Unity
   metadata.
2. Decide artifact format: `.unitypackage`, UPM folder/archive, or both.
3. Define include and exclude lists.
4. Define manual Unity export, Unity batchmode export, or helper script.
5. Verify package contents, Unity `2022.3+` compatibility, current
   `2022.3.62f2`, compilation, and EditMode tests.

Do not publish, upload, push, or create a release without a separate explicit
user command.
