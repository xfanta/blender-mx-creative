# Third-party assets

The code in this repository is MIT licensed (see `LICENSE`). The artwork is not
all mine, and the distinction matters if you plan to redistribute it.

## Blender UI icons

`BlenderPlugin/src/Icons/Blender/` holds 39 SVGs taken from Blender's source tree
(`release/datafiles/icons_svg/`). Only their `viewBox` has been rewritten, by
`tools/normalize_icons.py`; the drawings are unchanged.

Blender is **GNU GPL v3 or later**, and these files are part of that source tree,
so that is the licence they carry here too.

## Blender application icon

`BlenderPlugin/src/package/metadata/Icon256x256.png` is Blender's own application
icon, extracted from `Blender.app` and cropped to fill its canvas. The Blender logo
is a trademark of the Blender Foundation, used here to identify which application
this plugin drives.

## Hand-drawn tool icons

`BlenderPlugin/src/Icons/Tools/` — extrude, inset, subdivide, loop cut, knife and
merge — are original, drawn to match Blender's icon style because Blender ships
those tool icons only in its internal `VCO` binary format. These are MIT like the
rest of the repository.

## What this means for redistribution

The Logitech Marketplace requires open-source components to be MIT or Apache 2.0
and **explicitly excludes GPL**. Shipping the Blender-derived icons and the Blender
logo inside a Marketplace package is therefore not something you can do without
clearing it first. See the *Marketplace* section of the README.

## The Blender add-on

`blender-addon/blender_mx_bridge/` is **GPL-3.0-or-later**, not MIT. Blender's own
licensing page is explicit that the Python API is an integral part of the software
and that published scripts must be shared under a GPL compliant licence. The add-on
is distributed from this repository only; it is not part of the `.lplug4`.
