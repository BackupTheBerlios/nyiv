/* [ Viewer.cs  ] NyIV Viewer
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
using System.Text;

using Niry;
using Niry.Utils;
using Niry.GUI.Gtk2;

namespace NyIV.GUI {
	public class Viewer : Gtk.ScrolledWindow {
		// ========================================
		// PROTECTED Members
		// ========================================
		protected Gdk.Pixbuf original;
		protected Gtk.Image image;
		protected FileInfo fileInfo;
		protected int height;
		protected int width;

		// ========================================
		// PUBLIC Constructors
		// ========================================
		public Viewer() {
			// Initialize Members
			this.original = null;
			this.fileInfo = null;

			// Initialize Image
			this.image = new Gtk.Image();
			AddWithViewport(this.image);
		}

		// ========================================
		// PUBLIC Methods
		// ========================================
		public void Open (string filename, bool changeDir) {
			this.fileInfo = new FileInfo(filename);

			if (changeDir == true)
				Environment.CurrentDirectory = this.fileInfo.Directory.FullName;

			this.original = new Gdk.Pixbuf(filename);
			Image = this.original;
			this.height = Image.Height;
			this.width = Image.Width;
		}

		public void ZoomIn() {
			int w = Image.Width + 10;
			int h = Image.Height + 10;
			Image = ImageUtils.Resize(this.original, w, h);
		}

		public void ZoomOut() {
			int w = Image.Width - 10;
			int h = Image.Height - 10;
			if (w > 0 && h > 0)
				Image = ImageUtils.Resize(this.original, w, h);
		}

		public void Zoom100() {
			Image = this.original;
		}

		public void ZoomFit() {
			int w = this.Allocation.Width - 15;
			int h = this.Allocation.Height - 15;
			if (w > 0 && h > 0)
				Image = ImageUtils.Resize(this.original, w, h);
		}

		public string GetImageInfo() {
			StringBuilder info = new StringBuilder();
			
			info.AppendFormat("{0}x{1} pixels ", Image.Width, Image.Height);
			info.AppendFormat("({0}%) ", (int) (((float) Image.Width / (float) width)*100));
			info.AppendFormat(" - Original: {0}x{1} pixels ", width, height);
			info.AppendFormat("{0}", FileUtils.GetSizeString(fileInfo.Length));
			
			return(info.ToString());		
		}

		// ========================================
		// PUBLIC Properties
		// ========================================
		public FileInfo ImageInfo {
			get { return(this.fileInfo); }
		}

		public Gdk.Pixbuf Image {
			get { return(this.image.Pixbuf); }
			set {
				this.image.Pixbuf = value;
				this.ShowAll();
			}
		}
	}
}
