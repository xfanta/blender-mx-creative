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
