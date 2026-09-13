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
    }
}
