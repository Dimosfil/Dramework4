# UnityPackage Release Workflow Contract

## Feature

Name: UnityPackage release workflow

Idea: make Dramework4 exportable as a predictable Unity release artifact without
mixing runtime package contents with local caches, private data, or agent-only
generated noise.

Functional description: define how to choose the artifact format, version,
contents, exclusions, export process, verification steps, and final release
checklist for a Dramework4 Unity package release.

Out of scope: publishing to GitHub releases, asset stores, package registries,
or FTP unless the user explicitly asks for that release target.

## Workflow Contract

Entry points:

- User asks to prepare, build, export, or release a Unity package.
- User asks specifically for `.unitypackage`, UPM package archive, or release
  checklist work.

Required order:

1. Restore compact project context.
2. Read package metadata and current package layout.
3. Decide or confirm artifact format, version, package contents, and exclusions.
4. Define or implement export steps.
5. Verify contents and package metadata before producing or reporting an artifact.
6. Report checks, artifact path if created, and any manual Unity checks still required.

Branches and states:

- Loading: inspect only targeted package files, instructions, and release notes.
- Empty: if no export target is configured, create a release checklist before
  generating files.
- Success: artifact or documented release workflow is available with contents
  and verification status.
- Error: stop on ambiguous versioning, missing package metadata, Unity export
  failure, or possible inclusion of secrets/private/generated data.
- Cancelled: leave partial release files clearly reported and do not publish.
- Retry: fix the scoped cause, then re-run only the affected verification/export
  step.

Blocking work:

- Choosing release artifact format when the user has not already specified it.
- Confirming package version source and release version.
- Preventing accidental inclusion of generated caches, logs, local databases,
  secrets, private paths, or unrelated agent outputs.

Background work:

- None by default. Do not start long Unity exports or publish steps in the
  background unless explicitly requested.

Data freshness:

- Treat `Assets/IG/package.json` as the primary package metadata source.
- Treat `ProjectSettings/ProjectVersion.txt` as evidence for the current Unity
  editor version.
- Re-read package contents before each release attempt.

Observability:

- Report changed files, generated artifact path, package version, selected
  contents, exclusions, checks run, and checks not run.

User-visible guarantees:

- Do not delete project instruction, memory, tool, or agent infrastructure files
  during cleanup without explicit confirmation.
- Do not commit secrets, local databases, logs, generated caches, or build
  outputs.
- Do not publish, upload, push, or create a GitHub release unless explicitly
  requested.
- Keep `.unitypackage` work scoped to the current Dramework4 project root.

## Implementation Plan

Affected areas:

- `Assets/IG/package.json`
- `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime`
- `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Tests/EditMode`
- `tools/project-memory/`
- Optional future release/export scripts under `tools/`

Planned changes:

- Define package format and artifact naming.
- Define package contents and exclusions.
- Decide whether export is manual, Unity batchmode, or script-assisted.
- Add release checklist and optional export helper if needed.
- Verify artifact contents and Unity compatibility.

Dependencies:

- Unity `2022.3+`; current project version `2022.3.62f2` is supported.
- Unity Test Runner for EditMode tests.

Risks:

- Unity export may need Editor-only automation.
- Shell-only checks cannot prove Unity import or compile success.
- Version drift between package metadata and artifact name can confuse releases.

## Sprints

### Sprint 1

Goal: define the release workflow and package boundaries.

Scope: inspect package metadata, choose artifact strategy, list included and
excluded paths, and document release checklist.

Dependencies: package metadata and project instructions.

Exit criteria: release workflow is documented and ready for implementation or
manual execution.

Verification: `git diff --check`, metadata review, contents/exclusion review.

### Sprint 2

Goal: implement or execute package export.

Scope: create any needed export helper, run/manual-run Unity export, inspect
artifact, and record verification results.

Dependencies: chosen artifact format and Unity Editor availability.

Exit criteria: package artifact exists or manual export steps are documented
with blockers.

Verification: artifact contents inspection, Unity import/compile, EditMode tests
when available.

### Sprint 3

Goal: automate release artifact creation and post-export inspection where Unity
Editor access allows it.

Scope: add a Unity Editor export entry point or batchmode helper, choose a
project-local release output folder, generate the `.unitypackage`, inspect the
artifact contents, and record Unity import/compile/EditMode results.

Dependencies: Unity `2022.3+`, current `2022.3.62f2` project compatibility, and
the documented `.unitypackage` workflow from Sprint 1.

Exit criteria: `Dramework4-<package-version>-unity<unity-version>.unitypackage`
exists in the selected release output folder or the exact Unity/export blocker
is recorded with retry steps.

Verification: candidate validator, artifact contents inspection, clean import,
editor compilation, and EditMode Test Runner when Unity Editor is available.

## Tasks

- [x] Inspect package metadata and layout.
  - Trace to workflow or plan: Required order 2.
  - Definition of done: version source, package root, runtime paths, tests, and
    metadata are known.
  - Verification: targeted reads only; no broad repository dump.

- [x] Decide artifact format and naming.
  - Trace to workflow or plan: Required order 3.
  - Definition of done: `.unitypackage`, UPM archive, or combined strategy is
    selected, with naming based on package name and version.
  - Verification: user-visible summary or release checklist records the choice.

- [x] Define contents and exclusions.
  - Trace to workflow or plan: User-visible guarantees.
  - Definition of done: included paths and excluded generated/private/local
    paths are listed.
  - Verification: compare planned contents to package root and `.gitignore`.

- [x] Define export flow.
  - Trace to workflow or plan: Required order 4.
  - Definition of done: manual Unity flow, batchmode command, or helper script
    is documented.
  - Verification: command/checklist is reviewable and scoped to this project.

- [~] Verify release candidate.
  - Trace to workflow or plan: Required order 5.
  - Definition of done: metadata, contents, Unity version, and tests are checked
    or explicitly reported as manual/not run.
  - Verification: `git diff --check`, artifact contents review, Unity import,
    compile, and EditMode tests when available.

- [x] Add or execute automated export support.
  - Trace to workflow or plan: Sprint 3.
  - Definition of done: an Editor/batchmode export path exists or the artifact
    is produced manually with recorded commands/steps.
  - Verification: generated artifact path is reported, or blocker and retry
    path are recorded.

- [x] Add exported `.unitypackage` inspection support.
  - Trace to workflow or plan: Sprint 3.
  - Definition of done: artifact inspection can verify package paths and
    exclusions after export.
  - Verification: `tools/release/inspect-unitypackage.ps1` is parse-checked and
    documented in `tools/project-memory/unitypackage-release-workflow.md`.

- [x] Build fallback `.unitypackage` artifact without Unity Editor.
  - Trace to workflow or plan: Sprint 3.
  - Definition of done: package archive exists in the configured output folder
    when Unity executable access is unavailable.
  - Verification: `tools/release/build-unitypackage-without-unity.ps1` creates
    `Builds/UnityPackage/Dramework4-0.1.2-unity2022.3.unitypackage`, then
    `tools/release/inspect-unitypackage.ps1` validates its package paths.

## Verification

Manual checks:

- Open project in Unity `2022.3+`.
- Confirm current `2022.3.62f2` project version works.
- Export selected package artifact from Unity when automation is not available.

Automated checks:

- `git diff --check`
- Targeted metadata and contents queries.

Unity checks:

- Unity import succeeds.
- Editor compilation succeeds.
- EditMode Test Runner passes or failures are recorded.

Release/package checks:

- Artifact name includes package name and version.
- Artifact contents match the approved include/exclude list.
- No secrets, local databases, logs, caches, or generated project noise are
  included.

## Change Log

- 2026-06-11:
  - Change: built fallback artifact
    `Builds/UnityPackage/Dramework4-0.1.2-unity2022.3.unitypackage` with
    `tools/release/build-unitypackage-without-unity.ps1`.
  - Workflow impact: package assembly can proceed without a local Unity
    executable, but the artifact must still pass clean Unity import,
    compilation, and EditMode tests before final release.

- 2026-06-11:
  - Change: attempted `tools/release/export-unitypackage.ps1`; candidate
    validation passed, then export stopped because no Unity executable was
    found through `UNITY_EDITOR`, `UNITY_EDITOR_PATH`, or the standard Unity Hub
    path for `2022.3.62f2`.
  - Workflow impact: Sprint 3 is ready to retry with `-UnityPath` or a Unity
    editor environment variable.

- 2026-06-11:
  - Change: added Unity Editor export support, a batchmode PowerShell wrapper,
    and a `.unitypackage` inspection script for Sprint 3.
  - Workflow impact: export can now be run through
    `tools/release/export-unitypackage.ps1`; exported artifacts can be checked
    with `tools/release/inspect-unitypackage.ps1`.
  - Remaining verification: actual artifact creation, clean import,
    compilation, and EditMode tests require Unity Editor execution.

- 2026-06-11:
  - Change: added Sprint 3 for automated release artifact creation and
    post-export inspection.
  - Workflow impact: future work can move from documented release workflow to
    actual `.unitypackage` creation while preserving version, contents,
    exclusion, and Unity verification gates.

- 2026-06-11:
  - Change: documented the `.unitypackage` release workflow in
    `tools/project-memory/unitypackage-release-workflow.md` and added
    `tools/release/validate-unitypackage-candidate.ps1`.
  - Workflow impact: the release workflow now has a concrete primary artifact,
    version/naming source, contents boundary, exclusion list, manual Unity export
    steps, and lightweight candidate validation.
  - Remaining verification: Unity import, editor compilation, and EditMode tests
    still require the Unity Editor.

- 2026-06-11:
  - Change: created initial workflow contract and task breakdown.
  - Workflow impact: future `unitypackage` work must preserve release scope,
    contents verification, and no-publish-without-explicit-request guarantees.
