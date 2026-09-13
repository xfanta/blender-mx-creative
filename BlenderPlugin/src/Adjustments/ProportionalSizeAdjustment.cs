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
