/* [ Window.cs  ] NyIV Main Window
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
using System.IO;

using NyIV;
using NyIV.Utils;

namespace NyIV.GUI {
	public class Window : Gtk.Window {
		// ========================================
		// PROTECTED Members
		// ========================================
		protected UiMenuManager uiMenuManager;
		protected Gtk.Statusbar statusBar;
		protected Viewer viewer;
		protected Gtk.VBox vbox;

		// ========================================
		// PRIVATE Members
		// ========================================
		private bool isFullscreen = false;

		// ========================================
		// PUBLIC Constructors
		// ========================================
		public Window() : base(Info.Name + " " + Info.Version) {
			// Initialize Window Properties
			this.SetDefaultSize(500, 300);
			DefaultIcon = StockIcons.GetPixbuf("NyIVIcon");
			this.DeleteEvent += new DeleteEventHandler(OnWindowDelete);

			// Initialize VBox
			this.vbox = new Gtk.VBox(false, 0);
			this.Add(this.vbox);

			// Initialize Menu & Toolbar
			this.uiMenuManager = new UiMenuManager();
			this.uiMenuManager.Activated += new EventHandler(OnMenuActivated);
			this.AddAccelGroup(this.uiMenuManager.AccelGroup);

			// Initialize MenuBar
			Gtk.MenuBar menubar = this.MenuBar;
			this.vbox.PackStart(menubar, false, false, 0);

			// Initialize ToolBar
			Gtk.Toolbar toolbar = this.ToolBar;
			toolbar.ToolbarStyle = ToolbarStyle.Both;
			toolbar.IconSize = IconSize.LargeToolbar;
			this.vbox.PackStart(toolbar, false, false, 0);

			// Initialize Viewer
			this.viewer = new Viewer();
			this.vbox.PackStart(this.viewer, true, true, 2);			

			// Initialize StatusBar
			this.statusBar = new Gtk.Statusbar();
			this.vbox.PackEnd(this.statusBar, false, false, 0);

			// Set Sensitive Menu
			SetSensitiveImageMenu(false);
		}

		// ========================================
		// PROTECTED Methods
		// ========================================
		protected void MessageErrorDialog (string title, string message) {
			MessageDialog dialog;

			dialog = new MessageDialog (null, DialogFlags.Modal, MessageType.Error,
										ButtonsType.Close, true, 
										"<span size='x-large'><b>{0}</b></span>\n\n{1}",
										title, message);
			dialog.Run();
			dialog.Destroy();
		}

		// ========================================
		// PRIVATE Methods
		// ========================================
		private string OpenImage() {
			FileChooserDialog dialog = new FileChooserDialog("Open Image", null,
															 FileChooserAction.Open);
			dialog.SelectMultiple = false;
			
			FileFilter filter = new FileFilter();
			filter.AddPixbufFormats();
			dialog.AddFilter(filter);

			dialog.AddButton(Gtk.Stock.Cancel, ResponseType.Cancel);
			dialog.AddButton(Gtk.Stock.Open, ResponseType.Ok);

			string filename = null;
			if ((ResponseType) dialog.Run() == ResponseType.Ok)
				filename = dialog.Filename;
			dialog.Destroy();
			return(filename);
		}

		private void SetSensitiveImageMenu (bool sensitive) {
			// MenuBar
			this.uiMenuManager.SetSensitive("/MenuBar/ImageMenu/ZoomIn", sensitive);
			this.uiMenuManager.SetSensitive("/MenuBar/ImageMenu/ZoomOut", sensitive);
			this.uiMenuManager.SetSensitive("/MenuBar/ImageMenu/Zoom100", sensitive);
			this.uiMenuManager.SetSensitive("/MenuBar/ImageMenu/ZoomFit", sensitive);

			// ToolBar
			this.uiMenuManager.SetSensitive("/ToolBar/ZoomIn", sensitive);
			this.uiMenuManager.SetSensitive("/ToolBar/ZoomOut", sensitive);
			this.uiMenuManager.SetSensitive("/ToolBar/Zoom100", sensitive);
			this.uiMenuManager.SetSensitive("/ToolBar/ZoomFit", sensitive);
		}

		private void SetStatusBarImageInfo() {
			this.statusBar.Pop(0);
			this.statusBar.Push(0, this.viewer.GetImageInfo());
		}

		private void LoadImage (string filename, bool changeDir) {
			this.viewer.Open(filename, changeDir);
			SetSensitiveImageMenu(true);
			SetStatusBarImageInfo();
		}

		private void LoadWebImage() {
			Dialogs.LoadWebImage dialog = new Dialogs.LoadWebImage();
			if (dialog.Run() == ResponseType.Ok && dialog.Url != null) {
				WebFetch webFetch = new WebFetch(dialog.Url);
				string fileName = null;
				try {
					if (dialog.Save == true) {
						string name = dialog.Url.Substring(dialog.Url.LastIndexOf('/') + 1);
						fileName = System.IO.Path.Combine(Paths.WebImagesDirectory, name);
					} else {
						fileName = System.IO.Path.GetTempFileName();
					}
					webFetch.Save(fileName);
				} catch (Exception e) {
					MessageErrorDialog("Save Web Image", e.Message);
				}

				try {
					LoadImage(fileName, dialog.Save);
				} catch (Exception e) {
					MessageErrorDialog("Load Web Image", e.Message);
					// Delete Saved File
					if (dialog.Save == true) File.Delete(fileName);
				}

				// Delete Temp File
				if (dialog.Save == false) File.Delete(fileName);
			}
			dialog.Destroy();
		}

		private void SetProxy() {
			Dialogs.ProxySettings dialog = new Dialogs.ProxySettings();
			if (dialog.Run() == ResponseType.Ok) {
				Proxy.UseProxyAuth = dialog.UseProxyAuth;
				Proxy.EnableProxy = dialog.EnableProxy;
				Proxy.Username = dialog.Username;
				Proxy.Password = dialog.Password;
				Proxy.Host = dialog.Host;
				Proxy.Port = dialog.Port;

				// Save Proxy Configuration
				Proxy.Save();
			}
			dialog.Destroy();
		}

		// ========================================
		// PRIVATE (Methods) Event Handlers
		// ========================================
		private void OnWindowDelete (object sender, DeleteEventArgs args) {
			Application.Quit();
			args.RetVal = true;
		}

		private void OnMenuActivated (object sender, EventArgs args) {
			Action action = sender as Action;
			string filename = null;
			ImageList imageList;

			Gtk.Application.Invoke(delegate {
			switch (action.Name) {
				// File
				case "OpenImage":
					filename = OpenImage();
					if (filename != null) LoadImage(filename, true);
					break;
				case "OpenWebImage":
					LoadWebImage();
					break;
				case "ProxySettings":
					SetProxy();
					break;
				case "Quit":
					Gtk.Application.Quit();
					break;
				// View
				case "Fullscreen":
					if (isFullscreen == true) {
						Unfullscreen();
					} else {
						Fullscreen();
					}
					DefaultIcon = StockIcons.GetPixbuf("NyIVIcon");
					isFullscreen = !isFullscreen;
					break;
				// Image
				case "ZoomIn":
					this.viewer.ZoomIn();
					SetStatusBarImageInfo();
					break;
				case "ZoomOut":
					this.viewer.ZoomOut();
					SetStatusBarImageInfo();
					break;
				case "Zoom100":
					this.viewer.Zoom100();
					SetStatusBarImageInfo();
					break;
				case "ZoomFit":
					this.viewer.ZoomFit();
					SetStatusBarImageInfo();
					break;
				// Go
				case "GoBack":
					imageList = new ImageList();
					if (this.viewer.ImageInfo == null) {
						filename = imageList.GetLast();
					} else {
						filename = imageList.GoBack(this.viewer.ImageInfo.FullName);
					}
					if (filename != null) LoadImage(filename, true);
					break;
				case "GoForward":
					imageList = new ImageList();
					if (this.viewer.ImageInfo == null) {
						filename = imageList.GetFirst();
					} else {
						filename = imageList.GoForward(this.viewer.ImageInfo.FullName);
					}
					if (filename != null) LoadImage(filename, true);
					break;
				// Help
				case "About":
					new Dialogs.About();
					break;
			}
			});
		}

		// ========================================
		// PUBLIC Properties
		// ========================================
		public UiMenuManager MenuManager {
			get { return(this.uiMenuManager); }
		}

		public Gtk.MenuBar MenuBar {
			get { return((Gtk.MenuBar) this.uiMenuManager.GetWidget("/MenuBar")); }
		}

		public Gtk.Toolbar ToolBar {
			get { return((Gtk.Toolbar) this.uiMenuManager.GetWidget("/ToolBar")); }
		}

		public Gtk.Statusbar StatusBar {
			get { return(this.statusBar); }
		}

		public Gtk.VBox VBox {
			get { return(this.vbox); }
		}
	}
}
