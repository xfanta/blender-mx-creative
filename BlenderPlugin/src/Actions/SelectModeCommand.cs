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

    // Vertex / edge / face select mode - the thing you reach for constantly while
    // editing a mesh by hand. The buttons light up to show the current mode.

    public class SelectModeCommand : BlenderCommand
    {
        public SelectModeCommand() : base("Select Mode")
        {
            this.Add(new Item
            {
                Parameter = "vert",
                DisplayName = "Vertex",
                Icon = "vertexsel.svg",
                Command = "select_mode",
                Arguments = new { types = new[] { "VERT" } },
                Key = VirtualKeyCode.Key1,
                IsActive = s => s.SelectVert && !s.SelectEdge && !s.SelectFace,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "edge",
                DisplayName = "Edge",
                Icon = "edgesel.svg",
                Command = "select_mode",
                Arguments = new { types = new[] { "EDGE" } },
                Key = VirtualKeyCode.Key2,
                IsActive = s => !s.SelectVert && s.SelectEdge && !s.SelectFace,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "face",
                DisplayName = "Face",
                Icon = "facesel.svg",
                Command = "select_mode",
                Arguments = new { types = new[] { "FACE" } },
                Key = VirtualKeyCode.Key3,
                IsActive = s => !s.SelectVert && !s.SelectEdge && s.SelectFace,
                IsAvailable = s => s.IsEditMode,
            });

            // No default keymap entry combines the three, so this one needs the bridge.
            this.Add(new Item
            {
                Parameter = "all",
                DisplayName = "Vert+Edge+Face",
                ShortLabel = "V+E+F",
                Icon = "mesh_data.svg",
                Command = "select_mode",
                Arguments = new { types = new[] { "VERT", "EDGE", "FACE" } },
                IsActive = s => s.SelectVert && s.SelectEdge && s.SelectFace,
                IsAvailable = s => s.IsEditMode,
            });
        }
    }
}
