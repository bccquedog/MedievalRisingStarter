#!/usr/bin/env python3
"""Validate one task ticket and optionally a parallel-lane declaration."""

from __future__ import annotations

import argparse
import json
import pathlib
import sys


ROOT = pathlib.Path(__file__).resolve().parents[1]
ROUTING = json.loads((ROOT / "agent-routing.json").read_text(encoding="utf-8"))


def normalized(path: str) -> str:
    return path.strip().replace("\\", "/").rstrip("/") + "/"


def paths_overlap(left: str, right: str) -> bool:
    a, b = normalized(left), normalized(right)
    return a.startswith(b) or b.startswith(a)


def validate_task(path: pathlib.Path) -> list[str]:
    errors: list[str] = []
    task = json.loads(path.read_text(encoding="utf-8"))
    required = [
        "ticket",
        "workflow_state",
        "implementation_owner",
        "base_commit",
        "branch",
        "allowed_paths",
        "acceptance_criteria",
        "required_tests",
    ]
    for field in required:
        if field not in task:
            errors.append(f"task missing field: {field}")

    if task.get("workflow_state") not in ROUTING["states"]:
        errors.append(f"unknown workflow state: {task.get('workflow_state')}")
    if task.get("implementation_owner") not in ROUTING["agents"]:
        errors.append(f"unknown implementation owner: {task.get('implementation_owner')}")

    allowed = task.get("allowed_paths", [])
    for path_value in allowed:
        if pathlib.PurePosixPath(path_value).is_absolute() or ".." in pathlib.PurePosixPath(path_value).parts:
            errors.append(f"unsafe allowed path: {path_value}")

    locks = task.get("exclusive_locks", [])
    for global_path in ROUTING["exclusive_global_paths"]:
        if any(paths_overlap(path_value, global_path) for path_value in allowed):
            if not any(paths_overlap(lock, global_path) for lock in locks):
                errors.append(f"global path requires explicit lock: {global_path}")

    owner = task.get("implementation_owner")
    state = task.get("workflow_state")
    if state in ROUTING["implementation_states"]:
        if owner == "glm" and not all(
            normalized(path_value).startswith("Assets/Game/Content/Staging/")
            or normalized(path_value).startswith("docs/")
            for path_value in allowed
        ):
            errors.append("GLM production writes are forbidden; use staging or docs")
        if owner in {"claude", "kimi"} and not task.get("specialist_write_exception"):
            errors.append(f"{owner} is review-only without specialist_write_exception")

    return errors


def validate_lanes(path: pathlib.Path) -> list[str]:
    errors: list[str] = []
    data = json.loads(path.read_text(encoding="utf-8"))
    lanes = data.get("lanes", [])
    for index, left in enumerate(lanes):
        for right in lanes[index + 1 :]:
            for left_path in left.get("exclusive_paths", []):
                for right_path in right.get("exclusive_paths", []):
                    if paths_overlap(left_path, right_path):
                        errors.append(
                            f"parallel path collision: {left['ticket']}:{left_path} "
                            f"overlaps {right['ticket']}:{right_path}"
                        )
    merge_order = data.get("merge_order", [])
    tickets = [lane.get("ticket") for lane in lanes]
    if sorted(merge_order) != sorted(tickets):
        errors.append("merge_order must contain every lane ticket exactly once")
    return errors


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("task", type=pathlib.Path, nargs="?")
    parser.add_argument("--lanes", type=pathlib.Path)
    args = parser.parse_args()

    if args.task is None and args.lanes is None:
        parser.error("provide a task file, --lanes, or both")

    errors: list[str] = []
    if args.task is not None:
        errors.extend(validate_task(args.task))
    if args.lanes is not None:
        errors.extend(validate_lanes(args.lanes))

    if errors:
        print("AGENT WORK VALIDATION: FAIL")
        for error in errors:
            print(f"- {error}")
        return 1

    print("AGENT WORK VALIDATION: PASS")
    return 0


if __name__ == "__main__":
    sys.exit(main())
