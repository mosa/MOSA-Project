// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Source
{
	public class SourceRegion
	{
		public int Address { get; set; }
		public int Length { get; set; }

		public int StartLine { get; set; }
		public int EndLine { get; set; }
		public int StartColumn { get; set; }
		public int EndColumn { get; set; }

		public string Source { get; set; }
	}
}
