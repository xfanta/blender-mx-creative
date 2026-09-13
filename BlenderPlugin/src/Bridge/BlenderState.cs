namespace Loupedeck.BlenderPlugin
{
    using System;
    using System.Text.Json;

    // A snapshot of the bits of Blender's state that the console draws on its buttons.
    // Built from the "state" object the bridge add-on returns with every response.

    public sealed class BlenderState
    {
        public String Mode { get; private set; } = "OBJECT";

        public String ObjectName { get; private set; }

        public Boolean SelectVert { get; private set; } = true;

        public Boolean SelectEdge { get; private set; }

        public Boolean SelectFace { get; private set; }

        public Boolean XRay { get; private set; }

        public Boolean Proportional { get; private set; }

        public Double ProportionalSize { get; private set; } = 1.0;

        public Boolean Snap { get; private set; }

        public Boolean AutoMerge { get; private set; }

        public String Pivot { get; private set; }

        public String Orientation { get; private set; }

        public Int32? Subsurf { get; private set; }

        public Boolean IsEditMode => this.Mode == "EDIT";

        public static BlenderState Parse(JsonElement element)
        {
            var state = new BlenderState
            {
                Mode = GetString(element, "mode") ?? "OBJECT",
                ObjectName = GetString(element, "object"),
                SelectVert = GetBoolean(element, "select_vert"),
                SelectEdge = GetBoolean(element, "select_edge"),
                SelectFace = GetBoolean(element, "select_face"),
                XRay = GetBoolean(element, "xray"),
                Proportional = GetBoolean(element, "proportional"),
                ProportionalSize = GetDouble(element, "proportional_size") ?? 1.0,
                Snap = GetBoolean(element, "snap"),
                AutoMerge = GetBoolean(element, "automerge"),
                Pivot = GetString(element, "pivot"),
                Orientation = GetString(element, "orientation"),
            };

            if (element.TryGetProperty("subsurf", out var subsurf) && subsurf.ValueKind == JsonValueKind.Number)
            {
                state.Subsurf = subsurf.GetInt32();
            }

            return state;
        }

        // Used to decide whether redrawing the device is worth it.
        public Boolean SameAs(BlenderState other) =>
            other != null
            && this.Mode == other.Mode
            && this.ObjectName == other.ObjectName
            && this.SelectVert == other.SelectVert
            && this.SelectEdge == other.SelectEdge
            && this.SelectFace == other.SelectFace
            && this.XRay == other.XRay
            && this.Proportional == other.Proportional
            && Math.Abs(this.ProportionalSize - other.ProportionalSize) < 0.0001
            && this.Snap == other.Snap
            && this.AutoMerge == other.AutoMerge
            && this.Pivot == other.Pivot
            && this.Orientation == other.Orientation
            && this.Subsurf == other.Subsurf;

        private static String GetString(JsonElement element, String name) =>
            element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String
                ? value.GetString()
                : null;

        private static Boolean GetBoolean(JsonElement element, String name) =>
            element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.True;

        private static Double? GetDouble(JsonElement element, String name) =>
            element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.Number
                ? value.GetDouble()
                : (Double?)null;
    }
}
