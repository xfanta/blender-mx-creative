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

    // The modelling tools themselves.
    //
    // Most of these are modal operators: invoking them starts the tool and then
    // the mouse drives it, exactly as pressing the shortcut would. That is why
    // they are sent with exec = "INVOKE" rather than executed outright.

    public class MeshToolCommand : BlenderCommand
    {
        public MeshToolCommand() : base("Mesh Tools")
        {
            this.Add(new Item
            {
                Parameter = "extrude",
                DisplayName = "Extrude",
                Icon = "extrude.svg",
                Command = "op",
                Arguments = new { name = "mesh.extrude_region_move", exec = "INVOKE" },
                Key = VirtualKeyCode.KeyE,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "inset",
                DisplayName = "Inset",
                Icon = "inset.svg",
                Command = "op",
                Arguments = new { name = "mesh.inset", exec = "INVOKE" },
                Key = VirtualKeyCode.KeyI,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "bevel",
                DisplayName = "Bevel",
                Icon = "mod_bevel.svg",
                Command = "op",
                Arguments = new { name = "mesh.bevel", exec = "INVOKE" },
                Key = VirtualKeyCode.KeyB,
                Modifiers = ModifierKey.Control,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "loopcut",
                DisplayName = "Loop Cut",
                Icon = "loopcut.svg",
                Command = "op",
                Arguments = new { name = "mesh.loopcut_slide", exec = "INVOKE" },
                Key = VirtualKeyCode.KeyR,
                Modifiers = ModifierKey.Control,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "knife",
                DisplayName = "Knife",
                Icon = "knife.svg",
                Command = "op",
                Arguments = new { name = "mesh.knife_tool", exec = "INVOKE" },
                Key = VirtualKeyCode.KeyK,
                IsAvailable = s => s.IsEditMode,
            });

            // Subdivide has no default shortcut - it lives in the right-click menu -
            // so this button only works with the bridge running.
            this.Add(new Item
            {
                Parameter = "subdivide",
                DisplayName = "Subdivide",
                Icon = "subdivide.svg",
                Command = "op",
                Arguments = new { name = "mesh.subdivide", exec = "EXEC" },
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "merge",
                DisplayName = "Merge",
                Icon = "merge.svg",
                Command = "op",
                Arguments = new { name = "mesh.merge", exec = "INVOKE" },
                Key = VirtualKeyCode.KeyM,
                IsAvailable = s => s.IsEditMode,
            });

            // F in Blender's keymap: closes the selected vertices or edges into
            // an edge or a face.
            this.Add(new Item
            {
                Parameter = "make_face",
                DisplayName = "Make Face",
                Icon = "make_face.svg",
                Command = "op",
                Arguments = new { name = "mesh.edge_face_add", exec = "EXEC" },
                Key = VirtualKeyCode.KeyF,
                IsAvailable = s => s.IsEditMode,
            });

            // The S Z 0 reflex - scale to nothing along one global axis, which
            // flattens the selection onto a plane. There is no single shortcut to
            // fall back to, so these need the add-on.
            this.AddFlatten("flatten_x", "Flatten X", "flatten_x.svg", new[] { 0.0, 1.0, 1.0 });
            this.AddFlatten("flatten_y", "Flatten Y", "flatten_y.svg", new[] { 1.0, 0.0, 1.0 });
            this.AddFlatten("flatten_z", "Flatten Z", "flatten_z.svg", new[] { 1.0, 1.0, 0.0 });

            this.Add(new Item
            {
                Parameter = "delete",
                DisplayName = "Delete",
                Icon = "trash.svg",
                Command = "op",
                Arguments = new { name = "mesh.delete", exec = "INVOKE" },
                Key = VirtualKeyCode.KeyX,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "duplicate",
                DisplayName = "Duplicate",
                Icon = "duplicate.svg",
                Command = "op",
                Arguments = new { name = "mesh.duplicate_move", exec = "INVOKE" },
                Key = VirtualKeyCode.KeyD,
                Modifiers = ModifierKey.Shift,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "normals",
                DisplayName = "Recalc Normals",
                ShortLabel = "Normals",
                Icon = "normals_face.svg",
                Command = "op",
                Arguments = new { name = "mesh.normals_make_consistent", exec = "EXEC" },
                Key = VirtualKeyCode.KeyN,
                Modifiers = ModifierKey.Shift,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "shade_smooth",
                DisplayName = "Shade Smooth",
                ShortLabel = "Smooth",
                Icon = "mod_smooth.svg",
                Command = "op",
                Arguments = new { name = "object.shade_smooth", exec = "EXEC" },
                IsAvailable = s => !s.IsEditMode,
            });
        }

        private void AddFlatten(String parameter, String displayName, String icon, Double[] value) =>
            this.Add(new Item
            {
                Parameter = parameter,
                DisplayName = displayName,
                Icon = icon,
                Command = "op",
                Arguments = new
                {
                    name = "transform.resize",
                    exec = "EXEC",
                    props = new { value },
                },
            });
    }
}
