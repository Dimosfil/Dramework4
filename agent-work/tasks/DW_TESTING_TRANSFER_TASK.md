# Dramework4 Transfer Task: Headless Test Container

## Goal

Add a Dramework4-supported way to write unit tests without Zenject.

Warehouse SIM currently uses Zenject tests through `ZenjectUnitTestFixture`, `DiContainer.Bind`, and `DiContainer.Resolve`.
Before removing Zenject, Dramework4 needs an equivalent test harness for pure C# object graphs.

## Prototype Location

Warehouse SIM draft:

- `Assets/TestsProject/DrameworkTesting/DWTestContainerDraft.cs`
- `Assets/TestsProject/DrameworkTesting/DWTestContainerDraftTests.cs`

This draft is intentionally local to Warehouse SIM. Do not treat it as the final package location.

## Proposed Dramework4 Package Location

Move/adapt the draft to the Dramework4 source project:

- Source project: `H:\work\Dramework4`
- Runtime package source: `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime`
- Suggested final namespace: `IG.HappyCoder.Plugins.Dramework4.Runtime.Testing`
- Suggested final file: `Testing/DWTestContainer.cs`

## Required API

The first supported API should include:

- `Bind<T>(T instance, string id = "")`
- `Bind(Type contractType, object instance, string id = "")`
- `Bind<TContract, TImplementation>(string id = "", bool asSingle = true)`
- `Bind<TImplementation>(string id = "", bool asSingle = true)`
- `Resolve<T>(string id = "")`
- `Resolve(Type type, string id = "")`
- `Create<T>()`
- `Create(Type type)`
- `Inject(object target)`
- `InitializeAsync(CancellationToken cancellationToken = default)`
- `StartAsync(CancellationToken cancellationToken = default)`
- `InitializeAndStartAsync(CancellationToken cancellationToken = default)`

## Behavior To Preserve

- Constructor injection through `[Inject]`.
- Constructor parameter IDs through `[ID]`.
- Field and property injection through `[Inject]`.
- Nested injection through `[InjectInside]`.
- Array and `List<T>` injection.
- ID fallback from `IIdentifiable.ID` for bound instances.
- Lifecycle execution:
  - `IPreInitializable`
  - `IInitializable` ordered by `[InitializeOrder]`
  - `IStartable` ordered by `[StartOrder]`
- `IDisposable` cleanup in reverse tracked-object order.

## Explicit Non-Goals For First Version

- Do not boot Unity scenes.
- Do not depend on `SceneContainer`.
- Do not depend on the runtime hidden `Dispatcher`.
- Do not replace PlayMode integration tests.
- Do not implement scene hierarchy bindings in the headless test container.

Scene-bound `DBehaviour` / `SceneContainer` behavior should be tested separately with PlayMode tests.

## Validation

Port the draft tests or equivalent tests into the Dramework4 source project and verify:

- bind/resolve returns the bound instance.
- constructor injection with `[ID]` works.
- private field/property injection works.
- list injection works.
- lifecycle methods run in deterministic order.

Warehouse SIM prototype validation:

- Command: Unity EditMode test run with assembly `TestsProject` and filter `TestsProject.DrameworkTesting`.
- Results file: `TestResults/dwtestcontainer-results.xml`
- Result: `Passed`, total `5`, passed `5`, failed `0`.
- Verified tests:
  - `Resolve_ReturnsBoundInstance`
  - `Create_UsesInjectConstructorAndID`
  - `Inject_FillsPrivateFieldsAndProperties`
  - `Inject_FillsListDependencies`
  - `InitializeAndStartAsync_RunsLifecycleInOrder`

After successful Dramework4 validation:

1. Build/package Dramework4.
2. Bump Dramework4 package version.
3. Update Warehouse SIM to the new Dramework4 version/package copy.
4. Replace the Warehouse SIM draft with the package API.
5. Migrate at least one existing `ZenjectUnitTestFixture` test to the new Dramework4 testing API.
