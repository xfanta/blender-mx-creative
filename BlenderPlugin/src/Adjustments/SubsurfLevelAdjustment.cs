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

    // Viewport subdivision level of the active object. Adds a Subdivision modifier
    // if the object does not have one yet.

    public class SubsurfLevelAdjustment : BlenderAdjustment
    {
        public SubsurfLevelAdjustment()
            : base("Subdivision Level", "Viewport levels of the active object's Subdivision modifier", "Modifiers", "mod_subsurf.svg")
        {
        }

        protected override String Adjustment => "subsurf";

        protected override Object ResetValue => 0;

        protected override String GetAdjustmentValue(String actionParameter)
        {
            if (!this.IsAvailable)
            {
                return "--";
            }

            var level = this.State.Subsurf;
            return level.HasValue ? level.Value.ToString() : "off";
        }
    }
}
