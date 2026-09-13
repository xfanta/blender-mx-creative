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

    // The proportional editing falloff radius - the value you normally scrub with
    // the scroll wheel mid-transform, here available before you start one.

    public class ProportionalSizeAdjustment : BlenderAdjustment
    {
        public ProportionalSizeAdjustment()
            : base("Proportional Size", "Falloff radius for proportional editing", "Toggles", "prop_on.svg")
        {
        }

        protected override String Adjustment => "proportional_size";

        protected override Object ResetValue => 1.0;

        protected override String GetAdjustmentValue(String actionParameter) =>
            this.IsAvailable ? this.State.ProportionalSize.ToString("0.###") : "--";
    }
}
