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
