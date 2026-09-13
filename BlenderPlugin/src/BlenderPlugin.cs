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

    // Drives Blender from the MX Creative Console.
    //
    // Actions prefer the "MX Console Bridge" add-on, which runs the real Blender
    // operators and reports state back so buttons can show what is active. When
    // the add-on is not running, actions fall back to sending keyboard shortcuts.

    public class BlenderPlugin : Plugin
    {
        // The plugin sends keyboard shortcuts, so it is not API-only.
        public override Boolean UsesApplicationApiOnly => false;

        // This is an application plugin: its actions target Blender.
        public override Boolean HasNoApplication => false;

        public BlenderBridge Bridge { get; } = new BlenderBridge();

        public BlenderPlugin()
        {
            PluginLog.Init(this.Log);
            PluginResources.Init(this.Assembly);
        }

        public override void Load()
        {
            var application = this.ClientApplication;
            if (application != null)
            {
                application.ApplicationStarted += this.OnApplicationStarted;
                application.ApplicationStopped += this.OnApplicationStopped;

                if (!application.IsRunning())
                {
                    return;
                }
            }

            this.Bridge.StartPolling();
        }

        public override void Unload()
        {
            var application = this.ClientApplication;
            if (application != null)
            {
                application.ApplicationStarted -= this.OnApplicationStarted;
                application.ApplicationStopped -= this.OnApplicationStopped;
            }

            this.Bridge.Dispose();
        }

        private void OnApplicationStarted(Object sender, ClientApplicationChangedEventArgs e) =>
            this.Bridge.StartPolling();

        private void OnApplicationStopped(Object sender, ClientApplicationChangedEventArgs e) =>
            this.Bridge.StopPolling();
    }
}
