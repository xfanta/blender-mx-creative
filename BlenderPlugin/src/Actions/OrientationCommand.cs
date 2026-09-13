namespace Loupedeck.BlenderPlugin
{
    using System;

    // Transform orientation. Normal orientation in particular is worth a dedicated
    // button when you are pushing geometry around by hand.

    public class OrientationCommand : BlenderCommand
    {
        public OrientationCommand() : base("Orientation")
        {
            this.AddOrientation("global", "Global", "orientation_global.svg", "GLOBAL");
            this.AddOrientation("local", "Local", "orientation_local.svg", "LOCAL");
            this.AddOrientation("normal", "Normal", "orientation_normal.svg", "NORMAL");
            this.AddOrientation("view", "View", "orientation_view.svg", "VIEW");
            this.AddOrientation("gimbal", "Gimbal", "orientation_gimbal.svg", "GIMBAL");
            this.AddOrientation("cursor", "3D Cursor", "orientation_cursor.svg", "CURSOR");
        }

        private void AddOrientation(String parameter, String displayName, String icon, String value) =>
            this.Add(new Item
            {
                Parameter = parameter,
                DisplayName = displayName,
                Icon = icon,
                Command = "set",
                Arguments = new { what = "orientation", value },
                IsActive = s => s.Orientation == value,
            });
    }
}
