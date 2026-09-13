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

    // Transform pivot point. Blender puts these behind a pie menu; on the keypad
    // they each get a button that shows which one is live.

    public class PivotCommand : BlenderCommand
    {
        public PivotCommand() : base("Pivot Point")
        {
            this.AddPivot("median", "Median", "pivot_median.svg", "MEDIAN_POINT");
            this.AddPivot("cursor", "3D Cursor", "pivot_cursor.svg", "CURSOR");
            this.AddPivot("individual", "Individual", "pivot_individual.svg", "INDIVIDUAL_ORIGINS");
            this.AddPivot("active", "Active", "pivot_active.svg", "ACTIVE_ELEMENT");
            this.AddPivot("bounds", "Bounding Box", "pivot_boundbox.svg", "BOUNDING_BOX_CENTER");
        }

        private void AddPivot(String parameter, String displayName, String icon, String value) =>
            this.Add(new Item
            {
                Parameter = parameter,
                DisplayName = displayName,
                Icon = icon,
                Command = "set",
                Arguments = new { what = "pivot", value },
                IsActive = s => s.Pivot == value,
            });
    }
}
