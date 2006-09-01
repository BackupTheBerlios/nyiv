/* [ GUI/Dialogs/LoadWebImage.cs ] NyIV (Load Web Image Dialog)
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
using Glade;
using System;

namespace NyIV.GUI.Dialogs {
	public class LoadWebImage {
		[Glade.WidgetAttribute]
		private Gtk.Dialog dialog;
		[Glade.WidgetAttribute]
		private Gtk.Image image;
		[Glade.WidgetAttribute]
		private Gtk.Entry entryUrl;	
		[Glade.WidgetAttribute]
		private Gtk.CheckButton checkSave;

		public LoadWebImage() {
			XML xml = new XML(null, "LoadWebImageDialog.glade", "dialog", null);
			xml.Autoconnect(this);

			this.image.Pixbuf = StockIcons.GetPixbuf("NyIVImage");

			this.dialog.ShowAll();
		}

		public ResponseType Run() {
			return((ResponseType) dialog.Run());
		}

		public void Destroy() {
			dialog.Destroy();
		}

		// ============================================
		// PUBLIC Properties
		// ============================================
		public bool Save {
			get { return(checkSave.Active); }
		}

		public string Url {
			get { return((entryUrl.Text == "") ? null : entryUrl.Text); }
		}
	}
}
