# Dramework4 Refactor Plan

Created: 2026-06-11

## Goal

Improve maintainability of the Dramework4 runtime package without changing the
public behavior expected by existing Unity projects.

## Scope

Primary runtime source:

- `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime`

Primary tests:

- `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Tests/EditMode`

Out of scope unless explicitly approved:

- Broad public API renames.
- Package version bump or release artifact generation.
- Unity scene, asset, or prefab migrations.
- Deleting project memory, tools, release workflow files, or agent
  infrastructure.

## Refactor Principles

- Preserve source and `.meta` file pairings.
- Keep public API compatibility unless a breaking change is explicitly planned.
- Prefer small vertical refactors with focused EditMode coverage.
- Separate runtime code from editor-only code where assembly boundaries allow it.
- Do not introduce new framework dependencies unless the benefit is clear and
  local patterns support it.

## Phase 0 - Baseline

- [x] Record current git status and diff stat.
- [x] Confirm Unity version target remains `2022.3+`, including `2022.3.62f2`.
- [x] Run lightweight text checks: public API inventory, TODO/error search,
  editor/runtime boundary search.
- [x] Run `git diff --check` before and after code changes.
- [ ] Run Unity EditMode tests from Test Runner when Unity is available.

Definition of done:

- [x] Baseline risks and unavailable checks are documented.
- [x] No code changes are made before a target slice is selected.

## Baseline Findings

- Runtime has about 99 `.cs` files and only one existing EditMode test file
  before the refactor pass.
- Largest maintenance hotspots are `Messaging/Signals`, `Messaging/Requests`,
  `Tools/DW4Helpers.cs`, `Core/Dispatcher*.cs`, storage providers, and editor
  generators.
- `dramework4.asmdef` references `Unity.Addressables.Editor` while the assembly
  is not editor-only. This should be reviewed as a dedicated assembly-boundary
  refactor before player-build validation.
- `Assets/IG/package.json` lists Unity packages, but runtime code and asmdef
  also depend on `UniTask`, `MemoryPack`, and `Sirenix/Odin`. Confirm whether
  these are intentionally external project prerequisites or should be declared
  in package documentation/dependencies.
- Storage providers use static readonly `CancellationTokenSource` instances
  that are disposed through `DW4.Dispose()`. Reinitialization behavior after
  disposal needs a focused lifecycle review before changing it.
- Messaging and request classes contain large repeated generic arity blocks.
  Refactor only with representative behavior tests or generator-backed output.

## Completed First Slice

- [x] Made storage facade routing explicit with `switch` statements.
- [x] Removed `HasFlag` usage from non-flags `StorageType` routing.
- [x] Made unsupported storage types fail consistently for sync and async load
  and save paths.
- [x] Added EditMode tests for unsupported storage type behavior without using
  real `PlayerPrefs`, files, or remote requests.
- [x] Guarded `UnityEditor` import in `Dispatcher.cs` with `#if UNITY_EDITOR`.
- [x] Fixed pause/resume lifecycle loop so logging disabled no longer stops
  after the first pausable object.
- [x] Removed duplicate `ExCSS` assembly filter entry.

## Phase 1 - Safety Net

- [ ] Expand EditMode coverage around `DWTestContainer` and basic DI behavior.
- [ ] Add focused tests for lifecycle order attributes:
  initialization, start, update, pause.
- [ ] Add focused tests for messaging subscribe, fire, unsubscribe, and duplicate
  subscription handling.
- [ ] Add storage serialization tests that avoid user `PlayerPrefs` and local
  private data.

Definition of done:

- [ ] Refactor target has tests that fail on plausible regressions.
- [ ] Tests use temporary or in-memory data only.

## Phase 2 - Dependency Injection And Lifecycle

Candidate areas:

- `Core/Dispatcher*.cs`
- `SceneContainer.cs`
- `Testing/DWTestContainer.cs`
- `Attributes/Injecting`
- `Attributes/Updating`
- `Interfaces`

Tasks:

- [ ] Extract shared reflection helpers used by runtime and test container.
- [ ] Centralize binding key behavior for type plus optional id.
- [ ] Reduce duplicated lifecycle ordering code across dispatcher helpers.
- [ ] Make error messages consistent and actionable.
- [ ] Keep existing attribute entry points intact.

Definition of done:

- [ ] Existing runtime API remains source-compatible.
- [ ] Test container and runtime dispatcher agree on constructor, field, and
  property injection rules.

## Phase 3 - Messaging

Candidate areas:

- `Messaging/Signals`
- `Messaging/Requests`

Tasks:

- [ ] Map generated or repetitive generic arity classes before editing.
- [ ] Extract common subscription storage and validation behavior.
- [ ] Normalize unsubscribe semantics and missing-handler behavior.
- [ ] Consider code generation only if it matches existing generator patterns.
- [ ] Keep public `DWSignal*` and `DWRequest*` entry points stable.

Definition of done:

- [ ] Common logic is not copy-pasted across every arity.
- [ ] Subscribe/fire/unsubscribe behavior is covered for representative arities.

## Phase 4 - Storage And Config

Candidate areas:

- `Tools/DW4Storage.cs`
- `Providers/Storage`
- `Providers/Encryption`
- `Configs/StorageDataConfig*.cs`
- `Constants/StorageType.cs`
- `Constants/SerializationType.cs`
- `Constants/EncryptionType.cs`

Tasks:

- [ ] Replace repeated storage type branching with a small provider routing
  helper.
- [ ] Normalize sync and async error handling.
- [ ] Review cancellation token ownership and disposal behavior.
- [ ] Keep file paths and remote URLs supplied by config, not hard-coded.
- [ ] Add temporary-path tests for file storage where Unity permits it.

Definition of done:

- [ ] Storage behavior is easier to reason about without changing config shape.
- [ ] Async operations have clear cancellation and exception behavior.

## Phase 5 - Editor And Generation Boundaries

Candidate areas:

- `Editor`
- `*Editor.cs`
- `Tools/DW4HelpersEditor.cs`
- `Behaviours/DBehaviourEditor.cs`
- `ScriptableObjects/DScriptableObjectEditor.cs`

Tasks:

- [ ] Confirm editor-only files are protected by assembly layout or `#if UNITY_EDITOR`.
- [ ] Extract repeated editor UI/logo loading behavior.
- [ ] Reduce repeated file write/delete patterns in identifier generators.
- [ ] Preserve generated file paths and naming conventions.

Definition of done:

- [ ] Runtime builds do not depend on editor-only APIs.
- [ ] Generator changes are covered by deterministic output checks where
  practical.

## Phase 6 - Helpers And API Polish

Candidate areas:

- `Tools/DW4Helpers.cs`
- `Tools/DW4Extensions.cs`
- `Tools/DW4Logger.cs`
- `Tools/DW4.cs`

Tasks:

- [ ] Split large helper groups only where it reduces real maintenance cost.
- [ ] Keep compatibility wrappers for moved helper methods.
- [ ] Review logging wrapper naming and pass-through consistency.
- [ ] Remove dead code only after references are checked.

Definition of done:

- [ ] Helpers are easier to navigate.
- [ ] Existing call sites continue to compile.

## Suggested Execution Order

1. Assembly/dependency boundary audit for `dramework4.asmdef` and
   `Assets/IG/package.json`.
2. DI/lifecycle safety-net tests for dispatcher/test-container parity.
3. Storage provider lifecycle review, especially cancellation and disposal.
4. Messaging/request behavior tests before reducing generated duplication.
5. Editor/generation cleanup once runtime boundaries are stable.
6. Helper/API polish.

## Verification Checklist

- [ ] `git diff --check`
- [ ] Unity editor compilation succeeds.
- [ ] EditMode tests pass.
- [ ] Package manifest remains valid JSON.
- [ ] No secrets, logs, caches, local databases, or generated build artifacts are
  committed.
