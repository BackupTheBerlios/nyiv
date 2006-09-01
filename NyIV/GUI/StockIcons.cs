/* [ GUI/StockIcons.cs ] NyIV  (Stock Icons)
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
using System.Collections;

using Gtk;

using Niry;
using Niry.GUI.Gtk2;

namespace NyIV.GUI {
	public static class StockIcons {
		private static readonly string[] stock_icons = {
			"NyIVImage",
			"NyIVProxy",
		};

		private static readonly string[] stock_images_names = {
			"NyIVImage",
			"NyIVIcon",
			"NyIVLogo",
			"NyIVProxy"
		};

		// [name] = Gtk.Image
		private static Hashtable stock_images = new Hashtable();

		public static void Initialize () {
			Gtk.IconFactory factory = new Gtk.IconFactory();
			factory.AddDefault();
			
			// Stock Icons
			foreach (string name in stock_icons) {
				Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(null, name + ".png");
				Gtk.IconSet iconset = new Gtk.IconSet(pixbuf);				

				factory.Add(name, iconset);
			}

			// Stock Images
			foreach (string name in stock_images_names) {
				stock_images.Add(name, new Gdk.Pixbuf(null, name + ".png"));
			}
		}
		
		public static Gdk.Pixbuf GetPixbuf (string name) {
			return((Gdk.Pixbuf) stock_images[name]);
		}

		public static Gdk.Pixbuf GetPixbuf (string name, int size) {
			return(ImageUtils.Resize((Gdk.Pixbuf) stock_images[name], size, size));
		}

		public static Gdk.Pixbuf GetPixbuf (string name, int width, int height) {
			return(ImageUtils.Resize((Gdk.Pixbuf) stock_images[name], width, height));
		}

		public static Gtk.Image GetImage (string name) {
			return(new Gtk.Image((Gdk.Pixbuf) stock_images[name]));
		}

		public static Gtk.Image GetImage (string name, int size) {
			return(new Gtk.Image(GetPixbuf(name, size, size)));
		}

		public static Gtk.Image GetImage (string name, int width, int height) {
			return(new Gtk.Image(GetPixbuf(name, width, height)));
		}

		public static Gdk.Pixbuf GetFileIconPixbuf (string ext) {
			string type = "FileType" + ext;
			if (ext == null || IsPresent(type) == false) 
				return(GetPixbuf("FileTypeGeneric"));
			return(GetPixbuf(type));
		}

		public static Gdk.Pixbuf GetFileIconPixbuf (string ext, int size) {
			string type = "FileType" + ext;
			if (ext == null || IsPresent(type) == false) 
				return(GetPixbuf("FileTypeGeneric", size));
			return(GetPixbuf(type, size));
		}

		public static Gtk.Image GetFileIconImage (string ext) {
			string type = "FileType" + ext;
			if (ext == null || IsPresent(type) == false) 
				return(GetImage("FileTypeGeneric"));
			return(GetImage(type));
		}

		public static bool IsPresent (string name) {
			return(stock_images.ContainsKey(name));
		}
	}
}
