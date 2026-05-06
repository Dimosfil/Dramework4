# Dramework4 Task: Verify Testing API Release And Version Metadata

## Goal

Finalize the Dramework4 testing API work as a real plugin/package update, not just a copied consumer-package change.

Warehouse SIM has verified that the updated embedded package contains a working `DWTestContainer` and package EditMode tests. The remaining risk is release hygiene: the package metadata still reports version `0.1.1` in Warehouse SIM, so another agent working in the Dramework4 source repository must verify the source project, run/port tests there, bump version metadata if appropriate, and produce a clean handoff for updating consumers.

## Source Project

Work in:

```text
H:\work\Dramework4
```

Important source locations:

```text
Assets/IG/package.json
Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime
Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime/Testing
Assets/IG/HappyCoder/Plugins/Dramework4/Code/Tests/EditMode
```

## Consumer Verification Already Done

In Warehouse SIM, the embedded package was checked at:

```text
H:\work\Logistix\Warehouse SIM\warehousesimulation\Packages\ru.indiega.happycoder.dramework4
```

Found files:

```text
HappyCoder/Plugins/Dramework4/Code/Runtime/Testing/DWTestContainer.cs
HappyCoder/Plugins/Dramework4/Code/Tests/EditMode/DWTestContainerTests.cs
HappyCoder/Plugins/Dramework4/Code/Tests/EditMode/dramework4.tests.asmdef
```

Unity EditMode package test result in Warehouse SIM:

```text
Assembly: dramework4.tests
Result: Passed
Total: 7
Passed: 7
Failed: 0
```

Covered scenarios:

- `Bind/Resolve`
- constructor injection with `[ID]`
- private field/property injection
- `List<T>` injection
- array injection
- `[InjectInside]`
- lifecycle order: `IPreInitializable`, `IInitializable`, `IStartable`

Known caveat from Warehouse SIM:

```text
Packages/ru.indiega.happycoder.dramework4/package.json
version: 0.1.1
```

If the Dramework4 source package also remains `0.1.1`, decide and apply the correct next version for this feature, for example `0.1.2`, unless the maintainer has a different versioning rule.

## Required Work

1. Inspect current Dramework4 git state.

```powershell
git status --short
```

Do not reset or remove unrelated user files.

2. Verify the testing API exists in the Dramework4 source project.

Expected final runtime API location:

```text
Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime/Testing/DWTestContainer.cs
```

Expected namespace:

```csharp
IG.HappyCoder.Plugins.Dramework4.Runtime.Testing
```

Expected public API:

```csharp
Bind<T>(T instance, string id = "")
Bind(Type contractType, object instance, string id = "")
Bind<TContract, TImplementation>(string id = "", bool asSingle = true)
Bind<TImplementation>(string id = "", bool asSingle = true)
Resolve<T>(string id = "")
Resolve(Type type, string id = "")
Create<T>()
Create(Type type)
Inject(object target)
InitializeAsync(CancellationToken cancellationToken = default)
StartAsync(CancellationToken cancellationToken = default)
InitializeAndStartAsync(CancellationToken cancellationToken = default)
Dispose()
```

3. Verify or port tests into the Dramework4 source project.

Expected test location:

```text
Assets/IG/HappyCoder/Plugins/Dramework4/Code/Tests/EditMode
```

Tests should cover at least:

- bound instance resolution;
- constructor injection and `[ID]`;
- private field/property injection;
- `List<T>` injection;
- array injection;
- `[InjectInside]` nested injection;
- lifecycle ordering for `IPreInitializable`, `IInitializable`, `IStartable`.

4. Run the best available validation.

Preferred Unity EditMode command, adjusted for the local Unity path if needed:

```powershell
& '<Unity.exe>' -batchmode -projectPath 'H:\work\Dramework4' -runTests -testPlatform EditMode -assemblyNames dramework4.tests -testResults 'TestResults\dramework4-tests-results.xml' -logFile 'Logs\codex-unity-dramework4-tests.log'
```

If this cannot run because generated `.csproj` paths or Unity installation paths are stale, document the exact blocker and still verify source layout/asmdef references.

5. Version/package metadata.

Check:

```text
Assets/IG/package.json
```

If the feature is complete and tests pass, bump package version according to maintainer policy. If no policy exists, use the next patch version from `0.1.1` to `0.1.2` for this additive testing API.

6. Handoff for Warehouse SIM.

Write a short final note in this repository:

```text
agent-work/results/DW_TESTING_RELEASE_RESULT.md
```

Include:

- files changed;
- package version before/after;
- tests run and result;
- exact consumer update instructions for Warehouse SIM;
- any unresolved caveats.

## Non-Goals

- Do not migrate Warehouse SIM tests in this Dramework4 task.
- Do not remove Zenject from Warehouse SIM.
- Do not change Dramework4 runtime `Dispatcher` behavior unless required for this test container.
- Do not hide test failures by deleting tests.

## Acceptance Criteria

The task is complete when:

- `DWTestContainer` exists in the Dramework4 source package runtime code.
- Dramework4 package tests exist and compile.
- EditMode tests pass, or a precise environment blocker is documented.
- `Assets/IG/package.json` version is correct for the release.
- A result note exists under `agent-work/results/` with consumer update instructions.
