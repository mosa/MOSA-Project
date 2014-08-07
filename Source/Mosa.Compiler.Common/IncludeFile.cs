/*
* (c) 2012 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;

namespace Mosa.Compiler.Common
{
	/// <summary>
	///
	/// </summary>
	public class IncludeFile
	{
		public string Filename;

		public byte[] Content;

		public int Length { get { return Content.Length; } }

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
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.Win32NT)
				Filename = filename.Replace('\\', '/');

			//int at = Filename.LastIndexOf('/');
			//if (at > 0)
			//	Filename = Filename.Substring(at + 1, Filename.Length - at - 1);

			Filename = Path.GetFileName(filename);

			Content = File.ReadAllBytes(filename);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IncludeFile"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="newname">The newname.</param>
		public IncludeFile(string filename, string newname)
		{
			Filename = newname;

			Content = File.ReadAllBytes(filename);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IncludeFile"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="content">The content.</param>
		public IncludeFile(string filename, byte[] content)
		{
			Filename = filename;
			Content = content;
		}
	}
}