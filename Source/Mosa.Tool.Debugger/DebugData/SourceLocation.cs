// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.DebugData
{
	public class SourceLocation
	{
		public ulong Address { get; set; }
		public int Length { get; set; }

		public int MethodID { get; set; }
		public string MethodFullName { get; set; }

		public int Label { get; set; }
		public int SourceLabel { get; set; }
		public int StartLine { get; set; }
		public int StartColumn { get; set; }
		public int EndLine { get; set; }
		public int EndColumn { get; set; }

		public int SourceFileID { get; set; }
		public string SourceFilename { get; set; }
	}
}
