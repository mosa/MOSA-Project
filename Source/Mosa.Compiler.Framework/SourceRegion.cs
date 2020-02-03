// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Mapping of method address to source code location
	/// </summary>
	public class SourceRegion
	{
		/// <summary>
		/// Local address (Offset)
		/// </summary>
		public int Address { get; set; }

		/// <summary>
		/// How many bytes after Address belongs to this SourceRegion
		/// </summary>
		public int Length { get; set; }

		public int StartLine { get; set; }
		public int EndLine { get; set; }
		public int StartColumn { get; set; }
		public int EndColumn { get; set; }

		public string Filename { get; set; }

		/// <summary>
		/// A valid source location requires at least a address, filename and line number to be usable.
		/// Valid does not mean that all other properties are available, too.
		/// </summary>
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(Filename) && StartLine > 0 && Address >= 0;
			}
		}
	}
}
