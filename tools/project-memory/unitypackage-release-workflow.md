# Dramework4 UnityPackage Release Workflow

## Release Decision

Primary artifact: `.unitypackage`.

Source root: `Assets/IG`.

Version source: `Assets/IG/package.json`.

Current package metadata:

- Package id: `ru.indiega.happycoder.dramework4`
- Display name: `Dramework 4`
- Version: `0.1.2`
- Unity baseline: `2022.3`
- Current verified project editor: `2022.3.62f2`

Artifact naming:

```text
Dramework4-<package-version>-unity<unity-version>.unitypackage
```

For the current metadata, use:

```text
Dramework4-0.1.2-unity2022.3.unitypackage
```

Optional secondary artifact: an archive of the UPM package folder can be created
later from the same `Assets/IG` source root, but it is not the primary release
artifact for this workflow.

## Included Paths

Export the package root:

```text
Assets/IG
Assets/IG.meta
```

This includes:

- `Assets/IG/package.json`
- `Assets/IG/HappyCoder/Plugins/Dramework4`
- bundled package-local plugin folders under `Assets/IG/HappyCoder/Plugins`
- Unity `.meta` files for the exported assets

Keep the whole `Assets/IG` root together so GUIDs, package metadata, bundled
plugin references, runtime code, editor code, and EditMode tests stay aligned.

## External Dependencies

Declared by `Assets/IG/package.json`:

- `com.unity.nuget.newtonsoft-json`: `3.2.1`
- `com.unity.addressables`: `1.22.3`
- `com.unity.mathematics`: `1.2.6`
- `com.unity.burst`: `1.8.18`

The package also contains bundled plugin folders currently used by Dramework4
assembly references, including `UniTask` and `MemoryPack`.

## Exclusions

Do not include project-level generated, private, or agent-only paths:

- `Library/`
- `Temp/`
- `Obj/`
- `Build/`
- `Builds/`
- `Logs/`
- `UserSettings/`
- `.git/`
- `.vs/`
- `.idea/`
- `tools/`
- `ProjectSettings/`
- `Packages/`
- `agent-work/`
- `*.sqlite`, `*.sqlite3`, `*.db`
- `*.log`
- already-created `*.unitypackage` artifacts

Do not publish, upload, push, or create a hosted release as part of this
workflow unless the user gives a separate explicit command.

## Export Flow

1. Confirm `Assets/IG/package.json` has the intended release version.
2. Run the lightweight candidate validation:

   ```powershell
   .\tools\release\validate-unitypackage-candidate.ps1
   ```

3. Export with Unity batchmode when the Unity executable can be resolved:

   ```powershell
   .\tools\release\export-unitypackage.ps1
   ```

   If Unity is not installed in the standard Unity Hub path for the current
   project version, pass it explicitly:

   ```powershell
   .\tools\release\export-unitypackage.ps1 -UnityPath "C:\Path\To\Unity.exe"
   ```

   The default output folder is:

   ```text
   Builds/UnityPackage
   ```

4. Inspect an already exported artifact when needed:

   ```powershell
   .\tools\release\inspect-unitypackage.ps1 -ArtifactPath .\Builds\UnityPackage\Dramework4-0.1.2-unity2022.3.unitypackage
   ```

5. If Unity executable access is unavailable but a local package artifact is
   still needed, build the `.unitypackage` archive from Unity `.meta` GUIDs:

   ```powershell
   .\tools\release\build-unitypackage-without-unity.ps1
   ```

   This is a fallback packer; a clean Unity import, compilation, and EditMode
   tests are still required before treating the artifact as a final release.

6. If batchmode export is unavailable, open the project in Unity `2022.3+`; this project currently uses
   `2022.3.62f2`.
7. In the Project window, select `Assets/IG`.
8. Use `Assets > Export Package...` or `Tools > Dramework4 > Release > Export UnityPackage`.
9. Keep the selected package root and its children; avoid adding project-level
   generated folders or unrelated assets.
10. Export to a local release-output folder that is not inside `Assets/IG`, using
   the artifact name from this workflow.
11. Import the `.unitypackage` into a clean Unity `2022.3+` project and confirm
   dependencies restore from package metadata or are otherwise installed.

## Verification Checklist

- [ ] `Assets/IG/package.json` version matches the artifact name.
- [ ] `ProjectSettings/ProjectVersion.txt` confirms Unity `2022.3+`.
- [ ] Candidate validation script passes.
- [ ] Exported artifact name matches the release naming convention.
- [ ] Artifact contents contain `Assets/IG` and required `.meta` files.
- [ ] Artifact contents do not contain excluded generated/private/local paths.
- [ ] If the artifact was built without Unity Editor, validate it in a clean
  Unity import before publishing or tagging.
- [ ] Clean Unity import succeeds.
- [ ] Editor compilation succeeds.
- [ ] EditMode Test Runner passes or failures are recorded.
- [ ] No publish/upload/push/release-hosting step was performed without an
  explicit user command.
