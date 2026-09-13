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
