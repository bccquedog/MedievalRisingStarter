#!/usr/bin/env python3
"""Fast static checks that do not require a Unity installation."""

from __future__ import annotations

import json
import pathlib
import sys


ROOT = pathlib.Path(__file__).resolve().parents[1]
GENERATED_DIRS = {"Library", "Temp", "Obj", "Build", "Builds", "Logs", "UserSettings"}
REQUIRED = [
    "AGENTS.md",
    "agent-routing.json",
    "Packages/manifest.json",
    "ProjectSettings/ProjectVersion.txt",
    "Assets/Game/Scripts/Domain/MedievalRising.Domain.asmdef",
    "Assets/Game/Scripts/Application/MedievalRising.Application.asmdef",
    "Assets/Game/Scripts/Infrastructure/MedievalRising.Infrastructure.asmdef",
    "Assets/Game/Scripts/Presentation/MedievalRising.Presentation.asmdef",
    "Assets/Game/Tests/EditMode/MedievalRising.EditModeTests.asmdef",
]


def fail(message: str, failures: list[str]) -> None:
    failures.append(message)


def source_files(pattern: str):
    for path in ROOT.rglob(pattern):
        relative_parts = path.relative_to(ROOT).parts
        if any(part in GENERATED_DIRS for part in relative_parts):
            continue
        yield path


def main() -> int:
    failures: list[str] = []

    for relative in REQUIRED:
        if not (ROOT / relative).is_file():
            fail(f"missing required file: {relative}", failures)

    for path in source_files("*.json"):
        try:
            json.loads(path.read_text(encoding="utf-8"))
        except (OSError, json.JSONDecodeError) as error:
            fail(f"invalid JSON: {path.relative_to(ROOT)}: {error}", failures)

    domain = ROOT / "Assets/Game/Scripts/Domain"
    for path in domain.rglob("*.cs"):
        text = path.read_text(encoding="utf-8")
        if "UnityEngine" in text or "UnityEditor" in text:
            fail(f"Unity reference in pure domain: {path.relative_to(ROOT)}", failures)

    expected_references = {
        "MedievalRising.Domain.asmdef": set(),
        "MedievalRising.Application.asmdef": {"MedievalRising.Domain"},
        "MedievalRising.Infrastructure.asmdef": {
            "MedievalRising.Domain",
            "MedievalRising.Application",
        },
        "MedievalRising.Presentation.asmdef": {
            "MedievalRising.Domain",
            "MedievalRising.Application",
            "MedievalRising.Infrastructure",
            "Unity.InputSystem",
        },
    }
    for path in (ROOT / "Assets/Game/Scripts").rglob("*.asmdef"):
        if path.name not in expected_references:
            continue
        data = json.loads(path.read_text(encoding="utf-8"))
        actual = set(data.get("references", []))
        if actual != expected_references[path.name]:
            fail(
                f"unexpected references in {path.relative_to(ROOT)}: "
                f"{sorted(actual)}",
                failures,
            )

    version = (ROOT / "ProjectSettings/ProjectVersion.txt").read_text(encoding="utf-8")
    if "6000.3.20f1" not in version:
        fail("project is not pinned to Unity 6000.3.20f1", failures)

    if failures:
        print("STATIC VALIDATION: FAIL")
        for item in failures:
            print(f"- {item}")
        return 1

    json_count = sum(1 for _ in source_files("*.json"))
    cs_count = sum(1 for _ in source_files("*.cs"))
    print(f"STATIC VALIDATION: PASS ({json_count} JSON, {cs_count} C# files)")
    print("Unity compilation and Unity Test Framework execution are separate required gates.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
