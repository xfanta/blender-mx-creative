namespace Loupedeck.BlenderPlugin
{
    using System;

    // Vertex / edge / face select mode - the thing you reach for constantly while
    // editing a mesh by hand. The buttons light up to show the current mode.

    public class SelectModeCommand : BlenderCommand
    {
        public SelectModeCommand() : base("Select Mode")
        {
            this.Add(new Item
            {
                Parameter = "vert",
                DisplayName = "Vertex",
                Icon = "vertexsel.svg",
                Command = "select_mode",
                Arguments = new { types = new[] { "VERT" } },
                Key = VirtualKeyCode.Key1,
                IsActive = s => s.SelectVert && !s.SelectEdge && !s.SelectFace,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "edge",
                DisplayName = "Edge",
                Icon = "edgesel.svg",
                Command = "select_mode",
                Arguments = new { types = new[] { "EDGE" } },
                Key = VirtualKeyCode.Key2,
                IsActive = s => !s.SelectVert && s.SelectEdge && !s.SelectFace,
                IsAvailable = s => s.IsEditMode,
            });

            this.Add(new Item
            {
                Parameter = "face",
                DisplayName = "Face",
                Icon = "facesel.svg",
                Command = "select_mode",
                Arguments = new { types = new[] { "FACE" } },
                Key = VirtualKeyCode.Key3,
                IsActive = s => !s.SelectVert && !s.SelectEdge && s.SelectFace,
                IsAvailable = s => s.IsEditMode,
            });

            // No default keymap entry combines the three, so this one needs the bridge.
            this.Add(new Item
            {
                Parameter = "all",
                DisplayName = "Vert+Edge+Face",
                ShortLabel = "V+E+F",
                Icon = "mesh_data.svg",
                Command = "select_mode",
                Arguments = new { types = new[] { "VERT", "EDGE", "FACE" } },
                IsActive = s => s.SelectVert && s.SelectEdge && s.SelectFace,
                IsAvailable = s => s.IsEditMode,
            });
        }
    }
}
