# Pending Tasks

Use this file for active project-wide plans and multi-step work.

Keep entries concise and task-relevant. Do not store full diffs, large logs,
generated outputs, secrets, credentials, or private production data.

## Status Markers

- `[ ]` not started
- `[~]` in progress
- `[x]` done
- `[!]` blocked or needs attention

## Tasks

### Refactor Dramework4 runtime package

Goal: improve maintainability of the runtime package while preserving public
behavior for existing Unity projects.

Plan:

- [x] Create scoped refactor plan in `tools/project-memory/refactor-plan.md`.
- [x] Choose the first implementation slice.
- [x] Add or update safety-net EditMode tests for the selected slice.
- [x] Apply small behavior-preserving refactors.
- [~] Verify with lightweight checks and Unity EditMode tests when available.

Risks or dependencies:

- [!] Unity EditMode tests require Unity Test Runner.
- [x] Broad public API renames are out of scope unless explicitly approved.
- [!] Runtime/editor assembly boundaries must be preserved.

### Make README bilingual

Goal: mirror the framework capability documentation in Russian and English.

Planned changes:

- [x] Restructure README into RU and EN sections with equivalent content.
- [x] Verify Markdown headings and scoped diff.

Verification:

- [x] Read README headings.
- [x] Run `git diff --check`.

### Document Dramework4 capabilities in README

Goal: expand README with a factual overview of framework functionality based on
the current runtime package code.

Planned changes:

- [x] Inspect package manifest, runtime modules, public APIs, and tests.
- [x] Rewrite README overview and capability sections.
- [x] Verify Markdown structure and git diff scope.

Execution order:

- [x] Restore compact project context.
- [x] Gather feature map from source files.
- [x] Patch README.
- [x] Review resulting README and diff stat.

Risks or dependencies:

- [x] Existing README already has local changes; preserve intent and keep edits scoped.
- [x] Preserve file encoding while editing Russian text.

Verification:

- [x] Read README after patch.
- [x] Run targeted git diff/stat.

### Prepare UnityPackage release workflow

Goal: define and prepare a repeatable `.unitypackage` release workflow for
Dramework4 on Unity `2022.3+`, including the currently working `2022.3.62f2`
project version.

Planned changes:

- [x] Decide package artifact format and naming convention.
- [x] Confirm package version source and release version update rules.
- [x] Define included paths and excluded local/generated/private paths.
- [x] Create or document the package export/build steps.
- [x] Add release verification steps for Unity import, compilation, and EditMode tests.
- [x] Document the final release checklist.

Execution order:

- [x] Read `Assets/IG/package.json`, package layout, README, and relevant Unity metadata.
- [x] Decide whether the primary artifact is `.unitypackage`, UPM folder/archive, or both.
- [x] Draft the package contents manifest and exclusion list.
- [x] Define the export command/manual Unity flow and output location.
- [x] Verify that release files do not include caches, logs, local databases, secrets, or agent-only generated noise.
- [x] Run the available lightweight checks, then record any Unity Editor checks that must be run manually.

Risks or dependencies:

- [x] Unity `.unitypackage` export may require Unity Editor automation or a manual editor step.
- [x] Package versioning must stay consistent with `Assets/IG/package.json`.
- [x] Existing project instruction and memory files are agent infrastructure; do not delete or package them unless explicitly selected.
- [x] EditMode tests require Unity Test Runner rather than a plain shell command.

Verification:

- [x] `git diff --check`.
- [x] Validate selected package contents against the exclusion list.
- [x] Confirm configured Unity version is `2022.3+` and current project version is `2022.3.62f2`.
- [x] Document EditMode Test Runner as a required manual release check.
- [x] Define final artifact name, version, and contents before release.

Sprint 3 backlog:

- [x] Add a Unity Editor or batchmode export helper for the documented `.unitypackage` workflow.
- [x] Choose and document the project-local release output folder.
- [x] Produce `Dramework4-<package-version>-unity<unity-version>.unitypackage` with the no-Unity fallback packer.
- [x] Add an artifact inspection script for the include/exclude list.
- [x] Inspect exported artifact contents against the include/exclude list.
- [!] Record clean import, editor compilation, and EditMode Test Runner results.

Sprint 3 blocker:

- [!] `.\tools\release\export-unitypackage.ps1` validates the candidate but cannot run Unity Editor export until Unity executable path is available. Pass `-UnityPath` or set `UNITY_EDITOR` / `UNITY_EDITOR_PATH`.
- [!] The fallback artifact exists at `Builds/UnityPackage/Dramework4-0.1.2-unity2022.3.unitypackage`, but clean Unity import, editor compilation, and EditMode Test Runner are still required before final release.
