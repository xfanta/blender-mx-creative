namespace Loupedeck.BlenderPlugin
{
    using System;
    using System.Collections.Generic;

    // Shared base for every button this plugin offers.
    //
    // Each button is described by an Item: what to ask the bridge add-on to do,
    // which keystroke to send if the add-on is not running, which icon to draw,
    // and how to tell from Blender's state whether the button is "on".

    public abstract class BlenderCommand : PluginDynamicCommand
    {
        // State shows in the icon's brightness alone. Painting a background is not
        // an option: the image covers only the key's icon area, so a fill lands as
        // a floating rectangle rather than a highlighted key.
        private static readonly BitmapColor ActiveIcon = BitmapColor.White;
        private static readonly BitmapColor IdleIcon = new BitmapColor(0x80, 0x80, 0x80);
        private static readonly BitmapColor DisabledIcon = new BitmapColor(0x4a, 0x4a, 0x4a);

        protected sealed class Item
        {
            // The action parameter; must be unique within the command.
            public String Parameter { get; set; }

            public String DisplayName { get; set; }

            // File name of an embedded SVG, e.g. "vertexsel.svg".
            public String Icon { get; set; }

            // Bridge command and arguments. Null means this button is keystroke-only.
            public String Command { get; set; }

            public Object Arguments { get; set; }

            // Sent when the bridge is unavailable, or for anything Blender refuses
            // to do over the Python API (undo and redo).
            public VirtualKeyCode? Key { get; set; }

            public ModifierKey Modifiers { get; set; } = ModifierKey.None;

            // Whether the button should be drawn highlighted.
            public Func<BlenderState, Boolean> IsActive { get; set; }

            // Greys the icon out when the action cannot do anything right now.
            public Func<BlenderState, Boolean> IsAvailable { get; set; }

            // Optional label that reflects live state instead of the fixed name.
            public Func<BlenderState, String> Caption { get; set; }

            // Drawn on the key when DisplayName is too wide for it. The full name
            // is still what the action picker shows.
            public String ShortLabel { get; set; }
        }

        private readonly Dictionary<String, Item> _items = new Dictionary<String, Item>();
        private readonly String _groupName;

        protected BlenderCommand(String groupName) => this._groupName = groupName;

        protected BlenderPlugin BlenderPlugin => this.Plugin as BlenderPlugin;

        protected BlenderState State => this.BlenderPlugin?.Bridge.State ?? new BlenderState();

        // Without the bridge there is no state to draw from, and guessing would be
        // worse than saying nothing: the keystroke fallback still works, so buttons
        // must not be greyed out or wrongly highlighted just because it is missing.
        protected Boolean IsStateKnown => this.BlenderPlugin?.Bridge.IsConnected == true;

        protected void Add(Item item)
        {
            this._items[item.Parameter] = item;
            this.AddParameter(item.Parameter, item.DisplayName, this._groupName);
        }

        protected override Boolean OnLoad()
        {
            var bridge = this.BlenderPlugin?.Bridge;
            if (bridge != null)
            {
                bridge.Changed += this.OnBridgeChanged;
            }

            return true;
        }

        protected override Boolean OnUnload()
        {
            var bridge = this.BlenderPlugin?.Bridge;
            if (bridge != null)
            {
                bridge.Changed -= this.OnBridgeChanged;
            }

            return true;
        }

        private void OnBridgeChanged(Object sender, EventArgs e) => this.ActionImageChanged(null);

        protected override void RunCommand(String actionParameter)
        {
            if (actionParameter == null || !this._items.TryGetValue(actionParameter, out var item))
            {
                return;
            }

            var bridge = this.BlenderPlugin?.Bridge;
            var handled = item.Command != null && bridge != null && bridge.TryInvoke(item.Command, item.Arguments);

            if (!handled && item.Key.HasValue)
            {
                this.Plugin.ClientApplication.SendKeyboardShortcut(item.Key.Value, item.Modifiers);

                // The keystroke changes Blender behind our back, so re-read the state.
                bridge?.Refresh();
            }

            this.ActionImageChanged(null);
        }

        // Logi Options+ draws this under the icon itself, so the images stay text-free.
        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) =>
            actionParameter != null && this._items.TryGetValue(actionParameter, out var item)
                ? (this.IsStateKnown ? item.Caption?.Invoke(this.State) : null)
                  ?? item.ShortLabel
                  ?? item.DisplayName
                : null;

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter == null || !this._items.TryGetValue(actionParameter, out var item))
            {
                return null;
            }

            var state = this.State;
            var known = this.IsStateKnown;
            var active = known && (item.IsActive?.Invoke(state) ?? false);
            var available = !known || (item.IsAvailable?.Invoke(state) ?? true);

            try
            {
                using (var builder = new BitmapBuilder(imageSize))
                {
                    // Transparent: the key background and the label belong to Options+.
                    builder.Clear(BitmapColor.Transparent);

                    // This image *is* the key's icon element, and where that element
                    // sits comes from DefaultIconTemplate.ict - so the pictogram is
                    // simply centred here. Insetting it towards the top would push it
                    // up inside an element that is already above the label.
                    var side = (Int32)(Math.Min(builder.Width, builder.Height) * 0.74);
                    var iconColor = !available ? DisabledIcon : active ? ActiveIcon : IdleIcon;

                    // An SVG-backed BitmapImage reports its size as -1x-1, and the
                    // three-argument DrawImage throws on one. The width and height
                    // here are what actually rasterises the vector.
                    builder.DrawImage(
                        IconRenderer.Render(item.Icon, iconColor, side),
                        (builder.Width - side) / 2,
                        (builder.Height - side) / 2,
                        side,
                        side);

                    return builder.ToImage();
                }
            }
            catch (Exception ex)
            {
                // The service swallows anything thrown here and silently falls back
                // to drawing the action name, which is a miserable thing to debug.
                PluginLog.Error(ex, $"Could not draw '{item.Icon}' for {actionParameter}");
                return null;
            }
        }
    }
}
