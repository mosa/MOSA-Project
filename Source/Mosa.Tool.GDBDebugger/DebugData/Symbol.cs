// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger.DebugData
{
	public class Symbol
	{
		public ulong Address { get; set; }
		public uint Offset { get; set; }
		public int Size { get; set; }
		public string Kind { get; set; }
		public string Name { get; set; }
	}
}
