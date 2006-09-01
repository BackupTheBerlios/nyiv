/* [ PluginLib/PluginManager.cs  ] NyIV Plugin Manager
 * Author: Matteo Bertozzi
 * ============================================================================
 * This file is part of NyIV.
 *
 * NyIV is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * NyIV is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with NyIV; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */

using System;
using System.IO;
using System.Reflection;

using NyIV;
using NyIV.Utils;

namespace NyIV.PluginLib {	
	public class PluginManager {
		// ============================================
		// PRIVATE Members
		// ============================================
		private INyIV nyImageViewer;

		// ============================================
		// PUBLIC Constructors
		// ============================================
		public PluginManager (INyIV nyImageViewer) {
			this.nyImageViewer = nyImageViewer;

			FindAssemblies(Paths.SystemPluginDirectory);
			FindAssemblies(Paths.UserPluginDirectory);
		}

		// ============================================
		// PRIVATE Methods
		// ============================================
		private void ScanAssemblyForPlugins (Assembly asm) {
			foreach (Type t in asm.GetTypes()) {				
				if (t.IsSubclassOf(typeof(Plugin)) == true) {
					Plugin plugin = (Plugin) Activator.CreateInstance(t);				
					plugin.Initialize(nyImageViewer);
				}
			}
		}

		private void FindAssemblies (string dir) {
			if (dir == null || dir == "") return;

			DirectoryInfo info = new DirectoryInfo(dir);
			if (!info.Exists) return;

			foreach (FileInfo file in info.GetFiles()) {
				if (file.Extension != ".dll") continue;

				Assembly asm = Assembly.LoadFrom(file.FullName);
				ScanAssemblyForPlugins(asm);
			}
		}
	}
}
