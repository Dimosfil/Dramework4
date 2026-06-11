# Dramework4

## Русский

Dramework4 - runtime-фреймворк/пакет для Unity проектов HappyCoder и Indie
Games Project. Он рассчитан на Unity `2022.3+`, включая актуальные версии
Unity 6. Фреймворк добавляет поверх Unity-сцен прикладной слой:
dependency injection, управляемый жизненный цикл, messaging, storage/config
helpers, editor-инструменты и поддержку EditMode-тестов.

### Пакет

- Manifest пакета: `Assets/IG/package.json`
- Package name: `ru.indiega.happycoder.dramework4`
- Version: `0.1.2`
- Unity version: `2022.3+`; пакет можно разворачивать на более новых версиях,
  включая Unity 6
- Runtime assembly: `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime/dramework4.asmdef`
- Runtime source: `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime`
- EditMode tests: `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Tests/EditMode`

### Основной функционал

#### Dependency Injection

Фреймворк использует атрибуты и scene containers для регистрации и внедрения
объектов. В runtime этим управляет `Dispatcher`, а для EditMode-тестов есть
публичный `DWTestContainer`.

Поддерживаются:

- `[Bind]` на классах, которые нужно зарегистрировать в контейнере.
- `[Create(sceneName, order)]` на non-MonoBehaviour классах, которые нужно
  создать для сцены.
- `[Inject]` на конструкторах, полях, свойствах и методах.
- `[ID("name")]` для выбора зависимости по идентификатору.
- `[InjectInside]` для внедрения зависимостей во вложенные объекты.
- `IIdentifiable.ID` для идентификаторов объектов и конфигов.
- Внедрение списков и массивов в `DWTestContainer`.

Пример:

```csharp
public interface IWalletService
{
}

[Bind]
public sealed class WalletService : IWalletService
{
}

public sealed class WalletPresenter
{
    [Inject] private IWalletService _walletService;
}
```

#### Сцены и жизненный цикл

`Dispatcher` создается автоматически до загрузки сцены. Он регистрирует
`SceneContainer`, создает объекты по атрибутам, внедряет зависимости и запускает
lifecycle-интерфейсы в заданном порядке.

Lifecycle-интерфейсы:

- `IPreInitializable.OnPreInitialize()` - синхронная подготовка перед async
  initialization.
- `IInitializable.OnInitialize(CancellationToken)` - async initialization,
  например загрузка данных, создание runtime state или подготовка сервисов.
- `IStartable.OnStart(CancellationToken)` - async старт после initialization,
  когда зависимости уже созданы и проинициализированы.
- `IEarlyUpdatable.OnEarlyUpdate()` - ранний per-frame update до основного
  update flow.
- `IPreUpdatable.OnPreUpdate()` - подготовительный update перед `OnUpdate`.
- `IUpdatable.OnUpdate()` - основной per-frame update.
- `IFixedUpdatable.OnFixedUpdate()` - fixed-step update для physics-like
  логики.
- `IPreLateUpdatable.OnPreLateUpdate()` - подготовка перед late-update stage.
- `IPostLateUpdatable.OnPostLateUpdate()` - завершающий late-update stage.
- `IPausable.OnPause()` и `IPausable.OnResume()` - реакция объекта на pause и
  resume.

Атрибуты порядка:

- `[InitializeOrder]` - задает порядок вызова `OnInitialize`.
- `[StartOrder]` - задает порядок вызова `OnStart`.
- `[EarlyUpdateOrder]` - задает порядок вызова `OnEarlyUpdate`.
- `[PreUpdateOrder]` - задает порядок вызова `OnPreUpdate`.
- `[UpdateOrder]` - задает порядок вызова `OnUpdate`.
- `[FixedUpdateOrder]` - задает порядок вызова `OnFixedUpdate`.
- `[PreLateUpdateOrder]` - задает порядок вызова `OnPreLateUpdate`.
- `[PostLateUpdateOrder]` - задает порядок вызова `OnPostLateUpdate`.
- `[PauseOrder]` - задает порядок вызова `OnPause` и `OnResume`.

У всех order-атрибутов есть числовое значение `Order`: чем меньше число, тем
раньше объект попадает в соответствующий flow. Если атрибут не указан,
используется порядок `0`.

`SceneContainer` может помечать сцену как `DontDestroyOnLoad`, хранит найденные
`DBehaviour` объекты и список компонентов, которые нужно привязать к
контейнеру сцены.

#### Messaging

В Dramework4 есть три статических messaging API:

- `DWSignal` - синхронные publish/subscribe события.
- `DWSignalAsync` - отложенные async-сигналы, обрабатываемые через UniTask.
- `DWRequest` - request/response вызовы, где подписчик возвращает значение.

Signals и requests поддерживают именованные каналы и типизированные overloads.
`DWSignal` поддерживает до 10 payload-аргументов; `DWRequest` - до 10 входных
аргументов и один return type.

Пример:

```csharp
DWSignal.Subscribe<int>("coins_changed", OnCoinsChanged, order: 10);
DWSignal.Fire("coins_changed", 25);
DWSignal.Unsubscribe<int>("coins_changed", OnCoinsChanged);
```

#### Storage и config data

Storage настраивается через `StorageDataConfig` и доступен через `DW4.Load`,
`DW4.LoadAsync`, `DW4.Save` и `DW4.SaveAsync`.

Поддерживаемые storage targets:

- `PlayerPrefs`
- локальные файлы
- remote-файлы, загружаемые через `UnityWebRequest` в async-режиме

Сериализация:

- JSON через Newtonsoft.Json
- binary через MemoryPack

Encryption options:

- none
- XOR
- AES
- MD5 provider

`StorageDataConfigAsset`, `DW4ConfigAsset<T>`, `DW4Configurator<TConfig,
TConfigAsset>` и `StorageDataConfigurator` дают ScriptableObject-based
конфигурацию и editor import/export flow.

Пример:

```csharp
var config = new StorageDataConfig(
    id: "player_profile",
    filePath: "Assets/Data/player_profile.json",
    storageType: StorageType.File,
    serializationType: SerializationType.JSON);

config.Save(profile);
var response = config.Load<PlayerProfile>();

if (response.Success)
{
    profile = response.Data;
}
```

#### Helper tools

Статический фасад `DW4` группирует утилиты:

- async loading helpers для Addressables;
- collection helpers для random items, shuffle и insertion sort;
- IO helpers для очистки директорий, file attributes и relative paths;
- math helpers для distance/rotation comparisons;
- object helpers для поиска ближайших компонентов и physics overlap searches;
- string helpers для cleaning, casing, random strings, URL encoding и
  очистки clone-name postfix;
- Unity PlayerLoop helpers для добавления, вставки, удаления и логирования
  систем;
- logging wrappers поверх Unity logging API;
- encryption helpers для AES, MD5 и XOR.

Extension methods дублируют часть helpers для `Component`, `Transform`,
`Vector3`, `Quaternion`, strings, arrays и lists.

#### Базовые Unity-типы и binders

- `DBehaviour` - базовый framework `MonoBehaviour`.
- `DScriptableObject` - базовый framework `ScriptableObject` с editor save
  support и branded inspector header.
- `CameraBinder` и `TransformBinder` - готовые identifiable binders для ссылок
  на camera/transform в сцене.
- Component lookup attributes: `[GetComponent]`, `[GetComponentInChildren]`,
  `[GetComponentInParent]`, `[GetComponentOnScene]`.

#### Editor utilities

В runtime-папке также есть editor-only helpers:

- initializer и generator utilities;
- tooling для scene container;
- prefab tools;
- addressables/editor asset lookup helpers;
- helper component `AnimatorTester`.

Editor-зависимые части защищены editor-only API или `#if UNITY_EDITOR`.

#### Testing

`DWTestContainer` - lightweight DI container для EditMode-тестов. Он умеет
bind instance/implementation, resolve/create объекты, inject private fields и
properties, inject nested objects, inject arrays/lists, запускать initialize/start
lifecycle flow и dispose tracked `IDisposable` instances.

Тесты лежат здесь:

```text
Assets/IG/HappyCoder/Plugins/Dramework4/Code/Tests/EditMode
```

Запуск через Unity Test Runner:

```text
Window > General > Test Runner > EditMode > Run All
```

### Зависимости

`Assets/IG/package.json` объявляет:

- `com.unity.nuget.newtonsoft-json`
- `com.unity.addressables`
- `com.unity.mathematics`
- `com.unity.burst`

Runtime assembly в текущем проекте также ссылается на:

- `UniTask`
- `MemoryPack.Unity`
- `Unity.Addressables.Editor`
- `Unity.ResourceManager`
- `Unity.IL2CPP`
- Odin Inspector namespaces (`Sirenix.OdinInspector`, `Sirenix.Utilities`)

Перед импортом или компиляцией Dramework4 убедитесь, что эти зависимости
доступны в consuming Unity project.

### Development

Откройте root репозитория как Unity project. Unity восстановит `Packages/` из
`Packages/manifest.json`.

Unity/IDE generated files вроде `Library/`, `Temp/`, `UserSettings/`,
`*.csproj` и `*.sln` игнорируются.

Для проверки обычно достаточно editor compilation и EditMode tests. Это
package/framework, поэтому standalone player build обычно определяется
проектом-потребителем, а не самим Dramework4.

## English

Dramework4 is a Unity runtime framework/package for HappyCoder and Indie Games
Project Unity projects. It targets Unity `2022.3+`, including current Unity 6
versions, and provides a small application layer around Unity scenes: dependency
injection, ordered lifecycle orchestration, messaging, storage/config helpers,
editor tooling, and EditMode test utilities.

### Package

- Package manifest: `Assets/IG/package.json`
- Package name: `ru.indiega.happycoder.dramework4`
- Version: `0.1.2`
- Unity version: `2022.3+`; the package can be deployed on newer versions,
  including Unity 6
- Runtime assembly: `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime/dramework4.asmdef`
- Runtime source: `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime`
- EditMode tests: `Assets/IG/HappyCoder/Plugins/Dramework4/Code/Tests/EditMode`

### Main Capabilities

#### Dependency Injection

The framework uses attributes and scene containers to bind and inject objects.
Runtime injection is coordinated by `Dispatcher`; tests can use the public
`DWTestContainer`.

Supported patterns include:

- `[Bind]` on classes that should be registered in the container.
- `[Create(sceneName, order)]` on non-MonoBehaviour classes that should be
  created for a scene.
- `[Inject]` on constructors, fields, properties, and methods.
- `[ID("name")]` for selecting an identified dependency.
- `[InjectInside]` for injecting dependencies into nested objects.
- `IIdentifiable.ID` for object/config identifiers.
- List and array injection in `DWTestContainer`.

Example:

```csharp
public interface IWalletService
{
}

[Bind]
public sealed class WalletService : IWalletService
{
}

public sealed class WalletPresenter
{
    [Inject] private IWalletService _walletService;
}
```

#### Scene And Lifecycle Orchestration

`Dispatcher` is created automatically before scene load. It registers
`SceneContainer` instances, creates attributed objects, injects dependencies,
and runs lifecycle interfaces in an ordered flow.

Lifecycle interfaces:

- `IPreInitializable.OnPreInitialize()` - synchronous preparation before async
  initialization.
- `IInitializable.OnInitialize(CancellationToken)` - async initialization, such
  as loading data, creating runtime state, or preparing services.
- `IStartable.OnStart(CancellationToken)` - async start after initialization,
  when dependencies have already been created and initialized.
- `IEarlyUpdatable.OnEarlyUpdate()` - early per-frame update before the main
  update flow.
- `IPreUpdatable.OnPreUpdate()` - preparatory update before `OnUpdate`.
- `IUpdatable.OnUpdate()` - main per-frame update.
- `IFixedUpdatable.OnFixedUpdate()` - fixed-step update for physics-like logic.
- `IPreLateUpdatable.OnPreLateUpdate()` - preparation before the late-update
  stage.
- `IPostLateUpdatable.OnPostLateUpdate()` - final late-update stage.
- `IPausable.OnPause()` and `IPausable.OnResume()` - object reaction to pause
  and resume.

Ordering attributes:

- `[InitializeOrder]` - sets the call order for `OnInitialize`.
- `[StartOrder]` - sets the call order for `OnStart`.
- `[EarlyUpdateOrder]` - sets the call order for `OnEarlyUpdate`.
- `[PreUpdateOrder]` - sets the call order for `OnPreUpdate`.
- `[UpdateOrder]` - sets the call order for `OnUpdate`.
- `[FixedUpdateOrder]` - sets the call order for `OnFixedUpdate`.
- `[PreLateUpdateOrder]` - sets the call order for `OnPreLateUpdate`.
- `[PostLateUpdateOrder]` - sets the call order for `OnPostLateUpdate`.
- `[PauseOrder]` - sets the call order for `OnPause` and `OnResume`.

All order attributes have a numeric `Order` value: lower values run earlier in
the corresponding flow. If the attribute is missing, the default order is `0`.

`SceneContainer` can mark a scene as `DontDestroyOnLoad`, exposes discovered
`DBehaviour` objects, and lists components that should be bound into the scene
container.

#### Messaging

Dramework4 has three static messaging APIs:

- `DWSignal` for synchronous publish/subscribe events.
- `DWSignalAsync` for queued async signals processed through UniTask.
- `DWRequest` for request/response calls where one subscriber returns a value.

Signals and requests support named channels and typed overloads. `DWSignal`
supports up to 10 payload arguments; `DWRequest` supports up to 10 input
arguments and one return type.

Example:

```csharp
DWSignal.Subscribe<int>("coins_changed", OnCoinsChanged, order: 10);
DWSignal.Fire("coins_changed", 25);
DWSignal.Unsubscribe<int>("coins_changed", OnCoinsChanged);
```

#### Storage And Config Data

Storage is configured with `StorageDataConfig` and exposed through `DW4.Load`,
`DW4.LoadAsync`, `DW4.Save`, and `DW4.SaveAsync`.

Storage targets:

- `PlayerPrefs`
- local files
- remote files loaded with `UnityWebRequest` for async reads

Serialization:

- JSON through Newtonsoft.Json
- binary through MemoryPack

Encryption options:

- none
- XOR
- AES
- MD5 provider

`StorageDataConfigAsset`, `DW4ConfigAsset<T>`, `DW4Configurator<TConfig,
TConfigAsset>`, and `StorageDataConfigurator` provide ScriptableObject-based
configuration and editor import/export flows.

Example:

```csharp
var config = new StorageDataConfig(
    id: "player_profile",
    filePath: "Assets/Data/player_profile.json",
    storageType: StorageType.File,
    serializationType: SerializationType.JSON);

config.Save(profile);
var response = config.Load<PlayerProfile>();

if (response.Success)
{
    profile = response.Data;
}
```

#### Helper Tools

The `DW4` static facade groups common helpers:

- Addressables async loading helpers.
- Collection helpers for random items, shuffling, and insertion sort.
- IO helpers for directory cleanup, file attributes, and relative paths.
- Math helpers for distance and rotation comparisons.
- Object helpers for nearest-component searches and physics overlap searches.
- String helpers for cleaning, casing, random strings, URL encoding, and
  clone-name cleanup.
- Unity PlayerLoop helpers for adding, inserting, removing, and logging systems.
- Logging wrappers around Unity logging APIs.
- Encryption helpers for AES, MD5, and XOR.

Extension methods mirror selected helpers for `Component`, `Transform`,
`Vector3`, `Quaternion`, strings, arrays, and lists.

#### Base Unity Types And Binders

- `DBehaviour` is the framework base `MonoBehaviour`.
- `DScriptableObject` is the framework base `ScriptableObject` with editor save
  support and a branded inspector header.
- `CameraBinder` and `TransformBinder` are ready-made identifiable binders for
  scene camera/transform references.
- Component lookup attributes include `[GetComponent]`,
  `[GetComponentInChildren]`, `[GetComponentInParent]`, and
  `[GetComponentOnScene]`.

#### Editor Utilities

The runtime folder also includes editor-only helpers:

- initializer and generator utilities;
- scene container editor tooling;
- prefab tools;
- addressables/editor asset lookup helpers;
- an `AnimatorTester` helper component.

These files are guarded by editor-only APIs or `#if UNITY_EDITOR` where needed.

#### Testing

`DWTestContainer` provides a lightweight DI container for EditMode tests. It can
bind instances or implementations, resolve/create objects, inject private fields
and properties, inject nested objects, inject arrays/lists, run initialize/start
lifecycle flows, and dispose tracked `IDisposable` instances.

Existing tests live in:

```text
Assets/IG/HappyCoder/Plugins/Dramework4/Code/Tests/EditMode
```

Run them from Unity Test Runner:

```text
Window > General > Test Runner > EditMode > Run All
```

### Dependencies

The package manifest declares:

- `com.unity.nuget.newtonsoft-json`
- `com.unity.addressables`
- `com.unity.mathematics`
- `com.unity.burst`

The runtime assembly also references these assemblies/plugins in the current
project:

- `UniTask`
- `MemoryPack.Unity`
- `Unity.Addressables.Editor`
- `Unity.ResourceManager`
- `Unity.IL2CPP`
- Odin Inspector namespaces (`Sirenix.OdinInspector`, `Sirenix.Utilities`)

Make sure these dependencies are available in the consuming Unity project before
importing or compiling Dramework4.

### Development

Open the repository root as a Unity project. Unity restores `Packages/` from
`Packages/manifest.json`.

Generated Unity and IDE files such as `Library/`, `Temp/`, `UserSettings/`,
`*.csproj`, and `*.sln` are ignored.

For validation, prefer editor compilation and EditMode tests. This repository is
a package/framework, so a standalone player build is usually defined by a
consuming project rather than by Dramework4 itself.
