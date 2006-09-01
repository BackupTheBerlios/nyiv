/* [ AboutDialog.cs ] NyIV (About Dialog)
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
using Pango;
using System;
using System.Text;

using Niry;
using Niry.GUI.Gtk2;

using NyIV;

namespace NyIV.GUI.Dialogs {
	public class About : Gtk.Window {
		private ScrollBox scrollBox;
		private Gtk.Image imageLogo;
		private Gtk.VBox vbox;

		protected string[] authors = new string[] {
			"Matteo Bertozzi"
		};
		
		protected string[] thanks = new string[] {
			"Laura Marastoni",
			"Ambra Cavalletto",
			"http://www.yellowicon.com/"
		};

		public About() : base("About " + Info.Name) {
			// Initialize Dialog Window Options
			this.SetDefaultSize(320, 223);

			// Initialize VBox
			this.vbox = new Gtk.VBox(false, 0);
			this.Add(this.vbox);

			// Initialize Image Logo
			this.imageLogo = StockIcons.GetImage("NyIVLogo");
			this.vbox.PackStart(this.imageLogo, false, false, 0);

			// Initialize Credit ScollBox
			this.scrollBox = new ScrollBox(CreditText);
			this.scrollBox.SetSizeRequest(320, 95);
			this.vbox.PackStart(this.scrollBox, false, false, 0);

			this.ShowAll();
		}

		private string CreditText {
			get {
				StringBuilder sb = new StringBuilder();
				
				sb.Append("<span size='x-large'><b>");
				sb.Append(Info.Name + " " + Info.Version);
				sb.Append("</b></span>\n");
				sb.Append ("\n<b>Developed By:</b>\n");

				foreach (string s in authors) {
					sb.Append(s);
					sb.Append("\n");
				}

				sb.Append("\n<b>Special Thanks To:</b>\n");
				foreach (string s in thanks) {
					sb.Append(s);
					sb.Append("\n");
				}

				sb.Append("\n<b>License:</b>\n");
				sb.Append("Released Under the GNU GPL\n");
				sb.Append("GNU General Public License.\n");

				sb.Append("\n<b>Copyright:</b>\n");
				sb.Append("(C) 2005-2006 By Matteo Bertozzi\n");
				
				return(sb.ToString());
			}
		}
	}
}
