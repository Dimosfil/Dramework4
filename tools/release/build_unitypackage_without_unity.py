import argparse
import io
import json
import re
import tarfile
from pathlib import Path


FORBIDDEN_SUFFIXES = {".sqlite", ".sqlite3", ".db", ".log", ".unitypackage"}
FORBIDDEN_PARTS = {
    ".git",
    ".idea",
    ".vs",
    "agent-work",
    "Build",
    "Builds",
    "Library",
    "Logs",
    "Obj",
    "Packages",
    "ProjectSettings",
    "Temp",
    "tools",
    "UserSettings",
}


def main() -> None:
    args = parse_args()
    repo_root = Path(args.repo_root).resolve()
    package_root = (repo_root / args.package_root).resolve()
    output_directory = (repo_root / args.output_directory).resolve()

    package_json = json.loads((package_root / "package.json").read_text(encoding="utf-8"))
    artifact_name = f"Dramework4-{package_json['version']}-unity{package_json['unity']}.unitypackage"
    artifact_path = output_directory / artifact_name

    output_directory.mkdir(parents=True, exist_ok=True)

    meta_files = [package_root.with_suffix(package_root.suffix + ".meta")]
    meta_files.extend(sorted(package_root.rglob("*.meta")))

    exported = 0
    with tarfile.open(artifact_path, "w:gz") as archive:
        for meta_path in meta_files:
            asset_path = Path(str(meta_path)[:-5])
            if not asset_path.exists():
                continue

            relative_asset_path = asset_path.relative_to(repo_root).as_posix()
            ensure_allowed(relative_asset_path)

            guid = read_guid(meta_path)
            if not guid:
                raise RuntimeError(f"Unity meta file has no guid: {meta_path}")

            add_bytes(archive, f"{guid}/asset.meta", meta_path.read_bytes())
            add_bytes(archive, f"{guid}/pathname", relative_asset_path.encode("utf-8"))

            if asset_path.is_file():
                add_file(archive, f"{guid}/asset", asset_path)

            exported += 1

    print(f"ArtifactPath: {artifact_path}")
    print(f"AssetRecordCount: {exported}")


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser()
    parser.add_argument("--repo-root", required=True)
    parser.add_argument("--package-root", required=True)
    parser.add_argument("--output-directory", required=True)
    return parser.parse_args()


def ensure_allowed(relative_asset_path: str) -> None:
    parts = set(relative_asset_path.replace("\\", "/").split("/"))
    if parts & FORBIDDEN_PARTS:
        raise RuntimeError(f"Forbidden path included in package candidate: {relative_asset_path}")

    suffix = Path(relative_asset_path).suffix.lower()
    if suffix in FORBIDDEN_SUFFIXES:
        raise RuntimeError(f"Forbidden file type included in package candidate: {relative_asset_path}")


def read_guid(meta_path: Path) -> str:
    match = re.search(r"^guid:\s*([0-9a-fA-F]{32})\s*$", meta_path.read_text(encoding="utf-8"), re.MULTILINE)
    return match.group(1) if match else ""


def add_file(archive: tarfile.TarFile, name: str, source: Path) -> None:
    info = archive.gettarinfo(str(source), arcname=name)
    info.mtime = 0
    with source.open("rb") as source_file:
        archive.addfile(info, source_file)


def add_bytes(archive: tarfile.TarFile, name: str, data: bytes) -> None:
    info = tarfile.TarInfo(name)
    info.size = len(data)
    info.mtime = 0
    archive.addfile(info, io.BytesIO(data))


if __name__ == "__main__":
    main()
