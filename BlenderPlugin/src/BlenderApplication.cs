namespace Loupedeck.BlenderPlugin
{
    using System;
    using System.IO;

    // Links the plugin to Blender so that the console switches to the Blender
    // profile when Blender comes to the foreground.

    public class BlenderApplication : ClientApplication
    {
        private const String MacBundleName = "org.blenderfoundation.blender";
        private const String MacApplicationPath = "/Applications/Blender.app";

        protected override String GetProcessName() => "blender";

        protected override String GetBundleName() => MacBundleName;

        public override ClientApplicationStatus GetApplicationStatus() =>
            Directory.Exists(MacApplicationPath)
                ? ClientApplicationStatus.Installed
                : ClientApplicationStatus.Unknown;
    }
}
