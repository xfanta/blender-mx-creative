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
