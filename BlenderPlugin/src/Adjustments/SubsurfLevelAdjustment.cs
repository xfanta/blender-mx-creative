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
