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

    // Undo and redo.
    //
    // These are keystroke-only on purpose: Blender refuses to run ed.undo and
    // ed.redo from outside its own event loop, so the bridge cannot help here.

    public class HistoryCommand : BlenderCommand
    {
        public HistoryCommand() : base("History")
        {
            this.Add(new Item
            {
                Parameter = "undo",
                DisplayName = "Undo",
                Icon = "loop_back.svg",
                Key = VirtualKeyCode.KeyZ,
                Modifiers = ModifierKey.ControlOrCommand,
            });

            this.Add(new Item
            {
                Parameter = "redo",
                DisplayName = "Redo",
                Icon = "loop_forwards.svg",
                Key = VirtualKeyCode.KeyZ,
                Modifiers = ModifierKey.ControlOrCommand | ModifierKey.Shift,
            });
        }
    }
}
