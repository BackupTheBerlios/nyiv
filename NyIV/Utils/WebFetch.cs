/* [ Utils/WebFetch.cs  ] NyIV WebFetch
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
using System.Net;

using Niry;
using Niry.Utils;

namespace NyIV.Utils {
	public class WebFetch {
		// ============================================
		// PRIVATE Members
		// ============================================
		private byte[] data = null;

		// ============================================
		// PUBLIC Constructor
		// ============================================
		public WebFetch (string url) {
			// Make Request
			WebRequest request = WebRequest.Create(url);
			WebProxy proxy = SetupProxy();
			if (proxy != null) request.Proxy = proxy;

			// Get Response
			WebResponse response = request.GetResponse();
			Stream stream = response.GetResponseStream();

			// Get Data
			this.data = FileUtils.ReadStreamFully(stream, 32768);

			stream.Close();
			response.Close();
		}

		// ============================================
		// PUBLIC Methods
		// ============================================
		public void Save (string filename) {
			FileStream fstream = File.Create(filename);
			fstream.Write(this.data, 0, this.data.Length);
			fstream.Close();
		}

		// ============================================
		// PROTECTED Methods
		// ============================================
		protected WebProxy SetupProxy() {
			if (Proxy.EnableProxy == false)
				return(null);

			WebProxy proxy = new WebProxy(Proxy.Host, Proxy.Port);
			proxy.BypassProxyOnLocal = true;

			if (Proxy.UseProxyAuth == true) {
				proxy.Credentials = new NetworkCredential(Proxy.Username, Proxy.Password);
			}

			return(proxy);
		}

		// ============================================
		// PUBLIC Properties
		// ============================================
		public byte[] Data {
			get { return(this.data); }
		}
	}
}
