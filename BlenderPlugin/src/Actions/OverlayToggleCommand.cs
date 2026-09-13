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
