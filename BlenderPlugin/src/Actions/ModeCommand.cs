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

    // Switching between object, edit and sculpt mode.

    public class ModeCommand : BlenderCommand
    {
        public ModeCommand() : base("Mode")
        {
            this.Add(new Item
            {
                Parameter = "toggle_edit",
                DisplayName = "Edit Mode",
                Icon = "editmode_hlt.svg",
                Command = "op",
                Arguments = new { name = "object.editmode_toggle", exec = "EXEC" },
                Key = VirtualKeyCode.Tab,
                IsActive = s => s.IsEditMode,
                Caption = s => s.IsEditMode ? "Edit" : "Object",
            });

            this.Add(new Item
            {
                Parameter = "object",
                DisplayName = "Object Mode",
                ShortLabel = "Object",
                Icon = "object_datamode.svg",
                Command = "op",
                Arguments = new { name = "object.mode_set", exec = "EXEC", props = new { mode = "OBJECT" } },
                IsActive = s => s.Mode == "OBJECT",
            });

            this.Add(new Item
            {
                Parameter = "sculpt",
                DisplayName = "Sculpt",
                Icon = "sculptmode_hlt.svg",
                Command = "op",
                Arguments = new { name = "object.mode_set", exec = "EXEC", props = new { mode = "SCULPT" } },
                IsActive = s => s.Mode == "SCULPT",
            });
        }
    }
}
