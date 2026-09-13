// Blender MX Creative - drive Blender from a Logitech MX Creative Console
// Copyright (C) 2026 Michal Fanta
//
// This program is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option)
// any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// this program. If not, see <https://www.gnu.org/licenses/>.

namespace Loupedeck.BlenderPlugin
{
    using System;

    // Modelling aids that are either on or off. Each button shows its own state,
    // which is the main thing the keyboard cannot tell you.

    public class OverlayToggleCommand : BlenderCommand
    {
        public OverlayToggleCommand() : base("Toggles")
        {
            this.Add(new Item
            {
                Parameter = "xray",
                DisplayName = "X-Ray",
                Icon = "xray.svg",
                Command = "toggle",
                Arguments = new { what = "xray" },
                Key = VirtualKeyCode.KeyZ,
                Modifiers = ModifierKey.AltOrOption,
                IsActive = s => s.XRay,
            });

            this.Add(new Item
            {
                Parameter = "proportional",
                DisplayName = "Proportional",
                ShortLabel = "Prop",
                Icon = "prop_on.svg",
                Command = "toggle",
                Arguments = new { what = "proportional" },
                Key = VirtualKeyCode.KeyO,
                IsActive = s => s.Proportional,
                Caption = s => s.Proportional ? $"Prop {s.ProportionalSize:0.##}" : "Proportional",
            });

            this.Add(new Item
            {
                Parameter = "snap",
                DisplayName = "Snap",
                Icon = "snap_on.svg",
                Command = "toggle",
                Arguments = new { what = "snap" },
                Key = VirtualKeyCode.Tab,
                Modifiers = ModifierKey.Shift,
                IsActive = s => s.Snap,
            });

            this.Add(new Item
            {
                Parameter = "automerge",
                DisplayName = "Auto Merge",
                ShortLabel = "Automerge",
                Icon = "automerge_on.svg",
                Command = "toggle",
                Arguments = new { what = "automerge" },
                IsActive = s => s.AutoMerge,
            });
        }
    }
}
