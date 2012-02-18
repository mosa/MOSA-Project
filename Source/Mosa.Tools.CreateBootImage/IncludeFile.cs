/*
* (c) 2012 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.BootImage
{
	/// <summary>
	/// 
	/// </summary>
	public class IncludeFile
	{
		public string Filename;
		public string Newname;
		public bool ReadOnly = false;
		public bool Hidden = false;
		public bool Archive = true;
		public bool System = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="IncludeFile"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public IncludeFile(string filename)
		{
			Filename = filename;

			Newname = filename.Replace('\\', '/');

			int at = Newname.LastIndexOf('/');

			if (at > 0)
				Newname = Newname.Substring(at + 1, Newname.Length - at - 1);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IncludeFile"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="newname">The newname.</param>
		public IncludeFile(string filename, string newname)
		{
			Filename = filename;
			Newname = newname;
		}
	}
}
