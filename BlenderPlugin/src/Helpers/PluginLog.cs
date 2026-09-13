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

    // A helper class that enables logging from the plugin code.

    internal static class PluginLog
    {
        private static PluginLogFile _pluginLogFile;

        public static void Init(PluginLogFile pluginLogFile)
        {
            pluginLogFile.CheckNullArgument(nameof(pluginLogFile));
            PluginLog._pluginLogFile = pluginLogFile;
        }

        public static void Verbose(String text) => PluginLog._pluginLogFile?.Verbose(text);

        public static void Verbose(Exception ex, String text) => PluginLog._pluginLogFile?.Verbose(ex, text);

        public static void Info(String text) => PluginLog._pluginLogFile?.Info(text);

        public static void Info(Exception ex, String text) => PluginLog._pluginLogFile?.Info(ex, text);

        public static void Warning(String text) => PluginLog._pluginLogFile?.Warning(text);

        public static void Warning(Exception ex, String text) => PluginLog._pluginLogFile?.Warning(ex, text);

        public static void Error(String text) => PluginLog._pluginLogFile?.Error(text);

        public static void Error(Exception ex, String text) => PluginLog._pluginLogFile?.Error(ex, text);
    }
}
