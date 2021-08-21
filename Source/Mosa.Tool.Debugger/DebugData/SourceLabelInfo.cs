// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.DebugData
{
	public class SourceLabelInfo
	{
		public int MethodID { get; set; }
		public int StartOffset { get; set; }
		public int EndOffset { get { return StartOffset + Length - 1; } }
		public int Length { get; set; }
		public int Label { get; set; }
	}
}
