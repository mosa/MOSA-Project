// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.DebugData
{
	public class SourceInfo
	{
		public int MethodID { get; set; }
		public int Offset { get; set; } // same as Label
		public int Label { get { return Offset; } set { Offset = value; } }
		public int StartLine { get; set; }
		public int StartColumn { get; set; }
		public int EndLine { get; set; }
		public int EndColumn { get; set; }
		public int SourceFileID { get; set; }
	}
}
