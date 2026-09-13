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

    // Changing what is selected, rather than how it is selected.

    public class SelectionCommand : BlenderCommand
    {
        public SelectionCommand() : base("Selection")
        {
            this.Add(new Item
            {
                Parameter = "all",
                DisplayName = "Select All",
                Icon = "select_set.svg",
                Command = "op",
                Arguments = new { name = "mesh.select_all", exec = "EXEC", props = new { action = "SELECT" } },
                Key = VirtualKeyCode.KeyA,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "none",
                DisplayName = "Deselect",
                Icon = "select_subtract.svg",
                Command = "op",
                Arguments = new { name = "mesh.select_all", exec = "EXEC", props = new { action = "DESELECT" } },
                Key = VirtualKeyCode.KeyA,
                Modifiers = ModifierKey.AltOrOption,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "invert",
                DisplayName = "Invert",
                Icon = "select_difference.svg",
                Command = "op",
                Arguments = new { name = "mesh.select_all", exec = "EXEC", props = new { action = "INVERT" } },
                Key = VirtualKeyCode.KeyI,
                Modifiers = ModifierKey.Control,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "linked",
                DisplayName = "Linked",
                Icon = "group_vertex.svg",
                Command = "op",
                Arguments = new { name = "mesh.select_linked", exec = "EXEC" },
                Key = VirtualKeyCode.KeyL,
                Modifiers = ModifierKey.Control,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "more",
                DisplayName = "Grow",
                Icon = "add.svg",
                Command = "op",
                Arguments = new { name = "mesh.select_more", exec = "EXEC" },
                Key = VirtualKeyCode.Add,
                Modifiers = ModifierKey.Control,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "less",
                DisplayName = "Shrink",
                Icon = "remove.svg",
                Command = "op",
                Arguments = new { name = "mesh.select_less", exec = "EXEC" },
                Key = VirtualKeyCode.Subtract,
                Modifiers = ModifierKey.Control,
                IsAvailable = s => s.IsEditMode,
            });
        }
    }
}
