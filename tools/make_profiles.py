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

"""Generate the default .lp5 profiles shipped with the plugin.

A .lp5 is a zip holding ApplicationInfo.json and ProfileInfo.json. Action names
follow "$<plugin>___<action class>___<parameter>"; run

    logiplugintool xliff Blender <dir>

(or open loupedeck://plugin/Blender/xliff) against a loaded plugin to list them.

Writes BlenderPlugin/src/package/profiles/DefaultProfile70.lp5 (MX Creative
Keypad, 9 keys per page) and DefaultProfile71.lp5 (Dialpad, 4 buttons + 2 dials).
"""

import json
import pathlib
import uuid
import zipfile
from datetime import datetime, timezone

PLUGIN = "Blender"
APPLICATION = "@_blender"
NS = "Loupedeck.Service"

ROOT = pathlib.Path(__file__).resolve().parent.parent
PACKAGE = ROOT / "BlenderPlugin" / "src" / "package"
OUT = PACKAGE / "profiles"
# Options+ copies this into the application's folder the first time the profile is
# created, and never refreshes it afterwards - so it has to be right up front.
APPLICATION_ICON = PACKAGE / "metadata" / "Icon256x256.png"


def guid():
    return uuid.uuid4().hex.upper()


def action(cls, parameter=None):
    name = f"${PLUGIN}___Loupedeck.BlenderPlugin.{cls}"
    return f"{name}___{parameter}" if parameter else name


# Keypad pages, nine keys each, read left to right and top to bottom.
KEYPAD_PAGES = [
    ("Select", [
        ("SelectModeCommand", "vert"),
        ("SelectModeCommand", "edge"),
        ("SelectModeCommand", "face"),
        ("ModeCommand", "toggle_edit"),
        ("OverlayToggleCommand", "xray"),
        ("SelectModeCommand", "all"),
        ("SelectionCommand", "all"),
        ("SelectionCommand", "none"),
        ("SelectionCommand", "invert"),
    ]),
    ("Tools", [
        ("MeshToolCommand", "extrude"),
        ("MeshToolCommand", "inset"),
        ("MeshToolCommand", "bevel"),
        ("MeshToolCommand", "loopcut"),
        ("MeshToolCommand", "knife"),
        ("MeshToolCommand", "subdivide"),
        ("MeshToolCommand", "merge"),
        ("MeshToolCommand", "duplicate"),
        ("MeshToolCommand", "delete"),
    ]),
    ("Transform", [
        ("OverlayToggleCommand", "proportional"),
        ("OverlayToggleCommand", "snap"),
        ("OverlayToggleCommand", "automerge"),
        ("PivotCommand", "median"),
        ("PivotCommand", "cursor"),
        ("PivotCommand", "individual"),
        ("OrientationCommand", "global"),
        ("OrientationCommand", "normal"),
        ("OrientationCommand", "local"),
    ]),
    ("Finish", [
        ("SelectionCommand", "more"),
        ("SelectionCommand", "less"),
        ("SelectionCommand", "linked"),
        ("MeshToolCommand", "normals"),
        ("MeshToolCommand", "shade_smooth"),
        ("ModeCommand", "object"),
        ("ModeCommand", "sculpt"),
        ("HistoryCommand", "undo"),
        ("HistoryCommand", "redo"),
    ]),
]

DIALPAD_BUTTONS = [
    ("HistoryCommand", "undo"),
    ("HistoryCommand", "redo"),
    ("ModeCommand", "toggle_edit"),
    ("OverlayToggleCommand", "xray"),
]

DIALPAD_DIALS = [
    ("ProportionalSizeAdjustment", None),
    ("SubsurfLevelAdjustment", None),
]


def control(index, press=None, rotate=None):
    return {
        "$type": f"{NS}.Devices.Loupedeck7Devices.ProfileLayoutControl7, LoupedeckService",
        "controlId": index,
        "pressAction": press,
        "rotateAction": rotate,
    }


def page(display_name, controls):
    return {
        "$type": f"{NS}.Devices.Loupedeck7Devices.ProfileLayoutPage7, LoupedeckService",
        "name": guid(),
        "displayName": display_name,
        "description": None,
        "controls": controls,
    }


def layout(device, press_pages, rotate_pages):
    return {
        "$type": f"{NS}.Devices.Loupedeck7Devices.ProfileLayout7, LoupedeckService",
        "deviceType": device,
        "profileFlags": "None",
        "layoutModes": [{
            "$type": f"{NS}.Devices.Loupedeck7Devices.ProfileLayoutMode7, LoupedeckService",
            "deviceType": device,
            "modeName": "Main",
            "parentModeName": None,
            "actions": None,
            "dynamicButtonPages": None,
            "dynamicEncoderPages": None,
            "workspaces": [{
                "$type": f"{NS}.Devices.Loupedeck7Devices.ProfileLayoutWorkspace7, LoupedeckService",
                "name": guid(),
                "displayName": "Workspace 1",
                "description": None,
                "pressPages": press_pages,
                "rotatePages": rotate_pages,
            }],
            "folderPages": [],
        }],
        "folderPages": [],
    }


def profile(device, display_name, press_pages, rotate_pages):
    name = guid()
    info = {
        "$type": f"{NS}.ApplicationProfile, LoupedeckService",
        "name": name,
        "profileFlags": "None",
        "displayName": display_name,
        "description": "",
        "deviceType": device,
        "applicationName": APPLICATION,
        "nativePluginName": PLUGIN,
        "hasNativePlugin": True,
        "additionalNativePluginNames": ["DefaultMac"],
        "lastModifiedTimeUtc": datetime.now(timezone.utc).isoformat().replace("+00:00", "Z"),
        "profileSettings": {
            "$type": "Loupedeck.DictionaryNoCase`1[[System.String, System.Private.CoreLib]], PluginApi"
        },
        "actionImages90": None,
        "actionImages60": None,
        "wheelImages": None,
        "actionColors": None,
        "layout": layout(device, press_pages, rotate_pages),
        "macroCommands": [],
        "macroAdjustments": [],
        "profileCommands": [],
        "profileAdjustments": [],
        "conversionHistory": None,
        "profileActions": [],
    }
    application = {
        "$type": f"{NS}.SupportedApplicationInfo, LoupedeckService",
        "name": APPLICATION,
        "displayName": "Blender",
        "description": None,
        "deviceType": device,
        "nativePluginName": PLUGIN,
        "hasNativePlugin": True,
        "processOrBundleName": "org.blenderfoundation.blender",
        "modes": [{
            "$type": f"{NS}.ApplicationMode, LoupedeckService",
            "name": "Main",
            "parentModeName": None,
            "displayName": "Main",
        }],
        "defaultProfileName": name,
        "isEnabled": True,
    }
    return info, application


def write(path, info, application):
    path.parent.mkdir(parents=True, exist_ok=True)
    with zipfile.ZipFile(path, "w", zipfile.ZIP_DEFLATED) as zf:
        zf.writestr("ApplicationInfo.json", json.dumps(application, indent=2))
        zf.writestr("ProfileInfo.json", json.dumps(info, indent=2))
        zf.write(APPLICATION_ICON, "ApplicationIcon.png")
    print(f"wrote {path.name}")


def main():
    keypad_pages = [
        page(f"Page ({i + 1}) {title}",
             [control(slot, press=action(cls, param)) for slot, (cls, param) in enumerate(items)])
        for i, (title, items) in enumerate(KEYPAD_PAGES)
    ]
    write(OUT / "DefaultProfile70.lp5",
          *profile("Loupedeck70", "Blender", keypad_pages, []))

    buttons = [page("Page (1)", [control(slot, press=action(cls, param))
                                 for slot, (cls, param) in enumerate(DIALPAD_BUTTONS)])]
    dials = [page("Dial Page", [control(slot, rotate=action(cls, param))
                                for slot, (cls, param) in enumerate(DIALPAD_DIALS)])]
    write(OUT / "DefaultProfile71.lp5",
          *profile("Loupedeck71", "Blender", buttons, dials))


if __name__ == "__main__":
    main()
