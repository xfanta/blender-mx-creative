#!/usr/bin/env python3
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
# Resolution used to measure the artwork; higher is slower, not more correct.
PROBE = 600

VIEWBOX = re.compile(r'viewBox\s*=\s*"([-\d.\s]+)"')


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

        side = max(bx1 - bx0, by1 - by0) / ART_FRACTION
        cx, cy = (bx0 + bx1) / 2, (by0 + by1) / 2
        box = f"{cx - side / 2:.1f} {cy - side / 2:.1f} {side:.1f} {side:.1f}"

        updated = VIEWBOX.sub(f'viewBox="{box}"', svg, count=1)
        if updated != svg:
            path.write_text(updated)
            changed += 1

    print(f"normalised {changed} icons ({ART_FRACTION:.0%} of a square viewBox)")


if __name__ == "__main__":
    main()
