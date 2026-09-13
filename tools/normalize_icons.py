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

"""Give every icon a square viewBox with its artwork centred and equally sized.

Blender's icons come with assorted viewBoxes - 1100x1100, 1500x1600, 1600x1600 -
and the artwork fills a different fraction of each. Rendered into a square key
that makes some pictograms smaller than others and shifts them off centre.

This measures each pictogram's real bounding box, then rewrites the viewBox to a
square centred on it, sized so the artwork always occupies ART_FRACTION of the
key. The drawing itself is untouched, so re-running is idempotent.
"""

import pathlib
import re
import subprocess
import tempfile

from PIL import Image

ROOT = pathlib.Path(__file__).resolve().parent.parent
ICONS = ROOT / "BlenderPlugin" / "src" / "Icons"

# How much of the square the longest side of the pictogram takes up.
ART_FRACTION = 0.86
# Measuring rasterises at PROBE pixels, so a bounding box is only accurate to
# about a pixel of viewBox units. Rewriting a file over less than that starts a
# feedback loop - the new viewBox shifts the render, which shifts the next
# measurement - and the artwork walks a unit per run. Leave it alone instead.
TOLERANCE = 4
# Resolution used to measure the artwork; higher is slower, not more correct.
PROBE = 600

VIEWBOX = re.compile(r'viewBox\s*=\s*"([-\d.\s]+)"')
DIMENSION = re.compile(r'\b(width|height)\s*=\s*"[^"]*"')
SVG_TAG = re.compile(r"<svg\b[^>]*>")


def measure(path, x0, y0, w, h):
    """Return the artwork's bounding box in user units."""
    px = PROBE
    py = max(1, round(px * h / w))
    with tempfile.NamedTemporaryFile(suffix=".png") as tmp:
        subprocess.run(["rsvg-convert", "-w", str(px), "-h", str(py),
                        str(path), "-o", tmp.name], check=True)
        box = Image.open(tmp.name).convert("RGBA").getbbox()
    if box is None:
        raise SystemExit(f"{path.name} renders empty")
    sx, sy = w / px, h / py
    return (x0 + box[0] * sx, y0 + box[1] * sy,
            x0 + box[2] * sx, y0 + box[3] * sy)


def main():
    changed = 0
    for path in sorted(ICONS.rglob("*.svg")):
        svg = path.read_text()
        match = VIEWBOX.search(svg)
        if not match:
            print(f"  skipped {path.name}: no viewBox")
            continue

        x0, y0, w, h = (float(v) for v in match.group(1).split())
        bx0, by0, bx1, by1 = measure(path, x0, y0, w, h)

        # Rounded to whole units: measuring rasterises at PROBE pixels, so the
        # last fraction jitters between runs and would rewrite every file for
        # nothing. A unit out of ~1500 is far below anything visible.
        side = round(max(bx1 - bx0, by1 - by0) / ART_FRACTION)
        cx, cy = round((bx0 + bx1) / 2), round((by0 + by1) / 2)
        box = f"{cx - side // 2} {cy - side // 2} {side} {side}"

        updated = VIEWBOX.sub(f'viewBox="{box}"', svg, count=1)
        # The width and height attributes have to follow the viewBox. Leaving them
        # at the original non-square values makes rsvg letterbox the document when
        # measuring, so the next run reads a wrong bounding box and the artwork
        # creeps a little smaller every time this is run.
        def rewrite_tag(match):
            tag = DIMENSION.sub("", match.group(0))
            # Removing attributes leaves the gaps behind; collapse them or every
            # run adds two more spaces and the file never settles.
            tag = re.sub(r"\s+", " ", tag).replace("< svg", "<svg")
            return tag.replace("<svg", f'<svg width="{side}" height="{side}"', 1)

        updated = SVG_TAG.sub(rewrite_tag, updated, count=1)

        current = [float(v) for v in match.group(1).split()]
        wanted = [cx - side // 2, cy - side // 2, side, side]
        settled = (len(current) == 4
                   and all(abs(a - b) <= TOLERANCE for a, b in zip(current, wanted)))

        if updated != svg and not settled:
            path.write_text(updated)
            changed += 1

    print(f"normalised {changed} icons ({ART_FRACTION:.0%} of a square viewBox)")


if __name__ == "__main__":
    main()
