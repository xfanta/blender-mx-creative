namespace Loupedeck.BlenderPlugin
{
    using System;
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;

    // Renders the plugin's monochrome SVG icons at a given size and colour.
    //
    // Blender ships its UI icons as single-colour SVGs painted in #fff, and the
    // hand-drawn tool icons follow the same convention, so recolouring is a plain
    // token substitution. Results are cached because the service redraws buttons
    // often - on every state change, and on every page flip.

    internal static class IconRenderer
    {
        private static readonly Regex SvgTag = new Regex(@"<svg\b[^>]*>", RegexOptions.Compiled);
        private static readonly Regex Dimension = new Regex(@"\b(width|height)\s*=\s*""[^""]*""", RegexOptions.Compiled);

        private static readonly ConcurrentDictionary<String, BitmapImage> Cache =
            new ConcurrentDictionary<String, BitmapImage>();

        public static BitmapImage Render(String fileName, BitmapColor color, Int32 size)
        {
            var key = $"{fileName}|{color.ARGB:x8}|{size}";
            return Cache.GetOrAdd(key, _ => Build(fileName, color, size));
        }

        private static BitmapImage Build(String fileName, BitmapColor color, Int32 size)
        {
            var svg = PluginResources.ReadTextFile(fileName);
            var hex = $"#{color.R:x2}{color.G:x2}{color.B:x2}";

            svg = svg.Replace("#ffffff", hex).Replace("#FFFFFF", hex).Replace("#fff", hex).Replace("#FFF", hex);
            svg = Resize(svg, size);

            return BitmapImage.FromSvg(svg);
        }

        // The icons keep Blender's 1600x1500 authoring grid; rendering at the
        // button's pixel size avoids scaling a huge bitmap down afterwards.
        private static String Resize(String svg, Int32 size) =>
            SvgTag.Replace(
                svg,
                match => Dimension.Replace(match.Value, String.Empty)
                             .Replace("<svg", $@"<svg width=""{size}"" height=""{size}"""),
                1);
    }
}
