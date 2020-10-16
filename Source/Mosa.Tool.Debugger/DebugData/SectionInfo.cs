// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.DebugData
{
	public class SectionInfo
	{
		public uint Offset { get; set; }
		public ulong Address { get; set; }
		public int Size { get; set; }
		public string Kind { get; set; }
		public string Name { get; set; }
	}
}
