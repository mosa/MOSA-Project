// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Common
{
	/// <summary>
	/// Include File
	/// </summary>
	public class IncludeFile
	{
		public string Filename { get; set; }

		public byte[] Content { get; set; }

		public int Length { get { return Content.Length; } }

		public bool ReadOnly { get; set; } = false;
		public bool Hidden { get; set; } = false;
		public bool Archive { get; set; } = true;
		public bool System { get; set; } = false;

		public string SourceFileName { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="IncludeFile"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public IncludeFile(string filename)
		{
			SourceFileName = filename;

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
			SourceFileName = filename;
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
