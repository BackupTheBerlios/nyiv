/* [ Main.cs ] NyIV (Main)
 * Author: Matteo Bertozzi
 * ============================================================================
 * This program (NyIV) is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */

using System;

using NyIV;
using NyIV.Utils;
using NyIV.PluginLib;

namespace NyIV {
	public class NyIVApp : INyIV {
		// ============================================
		// PROTECTED Members
		// ============================================
		protected GUI.Window window;

		// ============================================
		// PUBLIC Constructors
		// ============================================
		public NyIVApp() {
		}

		// ============================================
		// PUBLIC Methods
		// ============================================
		public void Initialize() {
			Paths.Initialize();
			Environment.CurrentDirectory = Paths.HomeDirectory;

			Proxy.Initialize();
			GUI.StockIcons.Initialize();
		}

		public void Run() {
			window = new GUI.Window();
			window.ShowAll();
		}

		// ============================================
		// PUBLIC Properties
		// ============================================
		public GUI.Window Window {
			get { return(this.window); }
		}

		// ============================================
		//               APPLICATION MAIN
		// ============================================
		public static int Main (string[] args) {
			try {
				Gtk.Application.Init();

				// Initialize NyImageViewer (NyIV) Main Application
				NyIVApp nyImageViewer = new NyIVApp();
				nyImageViewer.Initialize();
				nyImageViewer.Run();

				// Load Plugins
				new PluginManager(nyImageViewer);

				Gtk.Application.Run();
			} catch (Exception e) {
				Console.WriteLine("{0} {1} Error", Info.Name, Info.Version);
				Console.WriteLine("Source:  {0}", e.Source);
				Console.WriteLine("Message: {0}", e.Message);
				Console.WriteLine("Stack Trace:\n{0}", e.StackTrace);
				return(1);
			}
			return(0);
		}
	}
}
