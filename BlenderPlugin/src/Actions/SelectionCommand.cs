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
