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

    // Shared base for the Dialpad dials.
    //
    // Dials need the bridge - there is no keystroke that nudges a value by one
    // detent - so when the add-on is not running they simply report "--".

    public abstract class BlenderAdjustment : PluginDynamicAdjustment
    {
        private static readonly BitmapColor Foreground = new BitmapColor(0xb4, 0xb4, 0xb4);
        private static readonly BitmapColor Disabled = new BitmapColor(0x5a, 0x5a, 0x5a);

        private readonly String _icon;

        protected BlenderAdjustment(String displayName, String description, String groupName, String icon)
            : base(displayName, description, groupName, hasReset: true) => this._icon = icon;

        protected BlenderPlugin BlenderPlugin => this.Plugin as BlenderPlugin;

        protected BlenderBridge Bridge => this.BlenderPlugin?.Bridge;

        protected BlenderState State => this.Bridge?.State ?? new BlenderState();

        protected Boolean IsAvailable => this.Bridge?.IsConnected == true;

        // What the dial nudges, as understood by the add-on's "adjust" command.
        protected abstract String Adjustment { get; }

        // The value to write back when the dial is pressed.
        protected abstract Object ResetValue { get; }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            this.Bridge?.TryInvoke("adjust", new { what = this.Adjustment, delta = diff });
            this.AdjustmentValueChanged();
        }

        protected override void RunCommand(String actionParameter)
        {
            this.Bridge?.TryInvoke("set", new { what = this.Adjustment, value = this.ResetValue });
            this.AdjustmentValueChanged();
        }

        protected override BitmapImage GetAdjustmentImage(String actionParameter, PluginImageSize imageSize)
        {
            try
            {
                using (var builder = new BitmapBuilder(imageSize))
                {
                    // Options+ draws the dial's value and background itself.
                    builder.Clear(BitmapColor.Transparent);

                    var side = (Int32)(Math.Min(builder.Width, builder.Height) * 0.74);
                    var color = this.IsAvailable ? Foreground : Disabled;

                    // Centred and filling the icon element; see BlenderCommand.
                    builder.DrawImage(
                        IconRenderer.Render(this._icon, color, side),
                        (builder.Width - side) / 2,
                        (builder.Height - side) / 2,
                        side,
                        side);

                    return builder.ToImage();
                }
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, $"Could not draw '{this._icon}'");
                return null;
            }
        }

        protected override Boolean OnLoad()
        {
            var bridge = this.Bridge;
            if (bridge != null)
            {
                bridge.Changed += this.OnBridgeChanged;
            }

            return true;
        }

        protected override Boolean OnUnload()
        {
            var bridge = this.Bridge;
            if (bridge != null)
            {
                bridge.Changed -= this.OnBridgeChanged;
            }

            return true;
        }

        private void OnBridgeChanged(Object sender, EventArgs e) => this.AdjustmentValueChanged();
    }
}
