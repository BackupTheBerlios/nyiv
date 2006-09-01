/* [ UiMenuManager.cs ] Image Viewer (Menu & Toolbar UIManager)
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

using Gtk;
using System;

using Niry;
using Niry.GUI.Gtk2;

namespace NyIV.GUI {
	public delegate void RightMenuHandler (object sender, PopupMenu menu);

	public sealed class UiMenuManager : Gtk.UIManager {
		// ============================================
		// PUBLIC Events
		// ============================================
		public event EventHandler Activated = null;

		// ============================================
		// PRIVATE Members
		// ============================================
		private ActionGroup actionGroup;

		private const string uiInfo = 
			"<ui>" +
			"  <menubar name='MenuBar'>" +
			"    <menu action='FileMenu'>" +
			"      <menuitem action='OpenImage'/>" +
			"      <menuitem action='OpenWebImage'/>" +
			"      <separator />" + 
			"      <menuitem action='ProxySettings'/>" +
			"      <separator />" + 
			"      <menuitem action='Quit'/>" +
			"    </menu>" +
			"    <menu action='ViewMenu'>" +
			"      <menuitem action='Fullscreen'/>" +
			"    </menu>" +
			"    <menu action='ImageMenu'>" +
			"      <menuitem action='ZoomIn'/>" +
			"      <menuitem action='ZoomOut'/>" +
			"      <menuitem action='Zoom100'/>" +
			"      <menuitem action='ZoomFit'/>" +
			"    </menu>" +
			"    <menu action='GoMenu'>" +
			"      <menuitem action='GoBack'/>" +
			"      <menuitem action='GoForward'/>" +
			"    </menu>" +
			"    <menu action='HelpMenu'>" +
			"      <menuitem action='About'/>" +
			"    </menu>" +
			"  </menubar>" +
			"  <toolbar name='ToolBar'>" +
			"    <toolitem action='GoBack' />" +
			"    <toolitem action='GoForward' />" +
			"    <separator />" + 
			"    <toolitem action='ZoomIn' />" +
			"    <toolitem action='ZoomOut' />" +
			"    <toolitem action='Zoom100' />" +
			"    <toolitem action='ZoomFit' />" +
			"    <separator />" + 
			"    <toolitem action='Fullscreen' />" +
			"  </toolbar>" +
			"</ui>";

		// ============================================
		// PUBLIC Constructors
		// ============================================
		public UiMenuManager() {
			ActionEntry[] entries = new ActionEntry[] {
				new ActionEntry("GoMenu", null, "_Go", null, null, null),
				new ActionEntry("FileMenu", null, "_File", null, null, null),
				new ActionEntry("ViewMenu", null, "_View", null, null, null),
				new ActionEntry("HelpMenu", null, "_Help", null, null, null),
				new ActionEntry("ImageMenu", null, "_Image", null, null, null),

				// File
				new ActionEntry("OpenImage", "NyIVImage", "Open Image", null, 
								"Open Image", new EventHandler(ActionActivated)),

				new ActionEntry("OpenWebImage", "NyIVImage", "Open Web Image", null, 
								"Open Web Image", new EventHandler(ActionActivated)),

				new ActionEntry("ProxySettings", "NyIVProxy", "Proxy Settings", null, 
								"Proxy Settings", new EventHandler(ActionActivated)),

				new ActionEntry("Quit", Gtk.Stock.Quit, "Quit", null, 
								"Quit Application", new EventHandler(ActionActivated)),

				// Go
				new ActionEntry("GoBack", Gtk.Stock.GoBack, "Back", null, 
								"Go Back", new EventHandler(ActionActivated)),

				new ActionEntry("GoForward", Gtk.Stock.GoForward, "Forward", null, 
								"Go Forward", new EventHandler(ActionActivated)),

				// View
				new ActionEntry("Fullscreen", Gtk.Stock.Fullscreen, "Fullscreen", null, 
								"Fullscreen", new EventHandler(ActionActivated)),

				// Image
				new ActionEntry("ZoomIn", Gtk.Stock.ZoomIn, "In", null, 
								"Zoom In", new EventHandler(ActionActivated)),

				new ActionEntry("ZoomOut", Gtk.Stock.ZoomOut, "Out", null, 
								"Zoom Out", new EventHandler(ActionActivated)),

				new ActionEntry("Zoom100", Gtk.Stock.Zoom100, "Normal", null, 
								"Zoom Normal", new EventHandler(ActionActivated)),

				new ActionEntry("ZoomFit", Gtk.Stock.ZoomFit, "Fit", null, 
								"Zoom Fit", new EventHandler(ActionActivated)),

				// Help Menu
				new ActionEntry("About", Gtk.Stock.About, "About", null, 
								"About Shared Folder", new EventHandler(ActionActivated)),
			};

			// Toggle items
			ToggleActionEntry[] toggleEntries = new ToggleActionEntry[] {
				// View
				new ToggleActionEntry("ViewNetwork", null, "Network", null,
										null, new EventHandler(ActionActivated), true),
				new ToggleActionEntry("ViewLogo", null, "NyFolder Logo", null,
										null, new EventHandler(ActionActivated), true),
				// Network
				new ToggleActionEntry("NetOnline", null, "Online", null,
										null, new EventHandler(ActionActivated), false)
			};

			actionGroup = new ActionGroup("group");
			actionGroup.Add(entries);
			actionGroup.Add(toggleEntries);
			InsertActionGroup(actionGroup, 0);
			AddUiFromString(uiInfo);
		}

		// ============================================
		// PUBLIC Methods
		// ============================================
		public void AddMenus (string ui, ActionEntry[] entries) {
			AddUiFromString(ui);
			actionGroup.Add(entries);
			EnsureUpdate();
		}

		public void SetSensitive (string path, bool sensitive) {
			Widget widget = GetWidget(path);
			if (widget != null) widget.Sensitive = sensitive;
		}

		// ============================================
		// PRIVATE STATIC (Methods) Event Handler
		// ============================================
		private void ActionActivated (object sender, EventArgs args) {
			if (Activated != null) Activated(sender, args);				
		}

		// ============================================
		// PUBLIC Properties
		// ============================================
		public ActionGroup GroupAction {
			get { return(this.actionGroup); }
		}
	}
}
