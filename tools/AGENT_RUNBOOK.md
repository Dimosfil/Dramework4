# Agent Runbook

Every command should be copy-pasteable from the project root.

## Install

```powershell
# Open the repository root as a Unity project.
# Unity restores Packages/ from Packages/manifest.json.
```

## Run

```powershell
# Open the project in Unity 2022.3.x and enter Play Mode from the editor.
```

## Test

```powershell
# Run EditMode tests from Unity Test Runner:
# Window > General > Test Runner > EditMode > Run All
```

## Build

```powershell
# This repository is a Unity package/framework. Prefer package validation and
# EditMode tests unless a consuming project defines a concrete player build.
```

## Smoke Check

```powershell
# Confirm Unity imports the project without compile errors and the package
# manifest at Assets/IG/package.json is visible to the Package Manager.
```

Expected result:

```text
Unity imports cleanly, editor compilation succeeds, and EditMode tests can run.
```

## Logs

```powershell
# Unity editor logs are outside the repository and should be read only when the
# user gives an explicit log path or asks for log inspection.
```

## Environment Notes

- Unity version: 2022.3.0f1 or compatible 2022.3 LTS.
- Package name: ru.indiega.happycoder.dramework4.
