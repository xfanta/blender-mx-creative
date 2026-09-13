#!/usr/bin/env python3
# Blender MX Creative - drive Blender from a Logitech MX Creative Console
# Copyright (C) 2026 Michal Fanta
#
# This program is free software: you can redistribute it and/or modify it
# under the terms of the GNU General Public License as published by the Free
# Software Foundation, either version 3 of the License, or (at your option)
# any later version.
#
# This program is distributed in the hope that it will be useful, but WITHOUT
# ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
# FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License along with
# this program. If not, see <https://www.gnu.org/licenses/>.

"""Generate package/actionsymbols from the icons the actions already declare.

Action symbols are the small glyphs beside action names in the Options+ picker.
The service finds them by file name: "<action class full name>___<parameter>.svg".
They must be monochrome black on transparent, so the source SVGs - which are
painted white for the device - are recoloured here.

The mapping is read straight out of the C# so it cannot drift from what the
buttons actually draw.
"""

import pathlib
import re

ROOT = pathlib.Path(__file__).resolve().parent.parent
SRC = ROOT / "BlenderPlugin" / "src"
ICONS = SRC / "Icons"
OUT = SRC / "package" / "actionsymbols"
NAMESPACE = "Loupedeck.BlenderPlugin"

# this.Add(new Item { ... Parameter = "vert", ... Icon = "vertexsel.svg", ... });
ITEM = re.compile(r'Parameter\s*=\s*"([^"]+)".*?Icon\s*=\s*"([^"]+)"', re.S)
# this.AddPivot("median", "Median", "pivot_median.svg", "MEDIAN_POINT");
HELPER = re.compile(r'this\.Add\w+\(\s*"([^"]+)"\s*,\s*"[^"]*"\s*,\s*"([^"]+\.svg)"')
# base("Proportional Size", "...", "Toggles", "prop_on.svg")
ADJUSTMENT = re.compile(r':\s*base\([^)]*"([^"]+\.svg)"\s*\)', re.S)


def find_icon(name):
    matches = list(ICONS.rglob(name))
    if not matches:
        raise SystemExit(f"icon not found: {name}")
    return matches[0]


def blacken(path):
    svg = path.read_text()
    for token in ("#ffffff", "#FFFFFF", "#fff", "#FFF"):
        svg = svg.replace(token, "#000000")
    return svg


def emit(action, parameter, icon):
    stem = f"{NAMESPACE}.{action}" + (f"___{parameter}" if parameter else "")
    (OUT / f"{stem}.svg").write_text(blacken(find_icon(icon)))
    return stem


def main():
    OUT.mkdir(parents=True, exist_ok=True)
    for stale in OUT.glob("*.svg"):
        stale.unlink()

    count = 0
    for path in sorted((SRC / "Actions").glob("*Command.cs")):
        action = path.stem
        if action == "BlenderCommand":
            continue
        text = path.read_text()
        pairs = [(p, i) for p, i in ITEM.findall(text)] + \
                [(p, i) for p, i in HELPER.findall(text)]
        for parameter, icon in pairs:
            emit(action, parameter, icon)
            count += 1
        # One symbol for the action itself, used wherever a parameter has none.
        if pairs:
            emit(action, None, pairs[0][1])
            count += 1

    for path in sorted((SRC / "Adjustments").glob("*Adjustment.cs")):
        if path.stem == "BlenderAdjustment":
            continue
        match = ADJUSTMENT.search(path.read_text())
        if match:
            emit(path.stem, None, match.group(1))
            count += 1

    print(f"wrote {count} action symbols to {OUT.relative_to(ROOT)}")


if __name__ == "__main__":
    main()
