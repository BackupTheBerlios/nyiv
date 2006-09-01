/* [ ImageList.cs  ] NyIV Image List
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

namespace NyIV.GUI {
	public class ImageList {
		private DirectoryInfo dirInfo;

		public ImageList() {
			this.dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
		}

		public bool IsImage (string fileName) {
			try {
				Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(fileName);
				pixbuf.Dispose();
				return(true);
			} catch (GLib.GException) {
				return(false);
			}			
		}

		public string GetFirst() {
			foreach (FileInfo fileInfo in dirInfo.GetFiles()) {
				if (fileInfo.Name.StartsWith(".")) continue;
				if (IsImage(fileInfo.FullName) == false) continue;
				return(fileInfo.FullName);
			}
			return(null);
		}

		public string GetLast() {
			string lastFileName = null;
			foreach (FileInfo fileInfo in dirInfo.GetFiles()) {
				if (fileInfo.Name.StartsWith(".")) continue;
				if (IsImage(fileInfo.FullName) == false) continue;
				lastFileName = fileInfo.FullName;
			}
			return(lastFileName);
		}

		public string GoBack (string current) {
			if (current == null) return(GetLast());

			string lastFileName = null;
			foreach (FileInfo fileInfo in dirInfo.GetFiles()) {
				if (fileInfo.Name.StartsWith(".")) continue;
				if (IsImage(fileInfo.FullName) == false) continue;

				if (current.Equals(fileInfo.FullName) == true)
					return((lastFileName == null) ? GetLast() : lastFileName);

				lastFileName = fileInfo.FullName;
			}
			return(null);
		}

		public string GoForward (string current) {
			if (current == null) return(GetFirst());

			bool found = false;
			foreach (FileInfo fileInfo in dirInfo.GetFiles()) {
				if (fileInfo.Name.StartsWith(".")) continue;
				if (IsImage(fileInfo.FullName) == false) continue;

				if (found == true)
					return(fileInfo.FullName);

				if (current.Equals(fileInfo.FullName) == true)
					found = true;
			}
			return((found == false) ? null : GetFirst());
		}
	}
}
