// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger.DebugData
{
	public class FieldInfo
	{
		public uint Index { get; set; }
		public string FullName { get; set; }
		public string Name { get; set; }
		public string FieldType { get; set; }
		public ulong Address { get; set; }
		public uint Attributes { get; set; }
		public uint Offset { get; set; }
		public uint DataLength { get; set; }
		public ulong DataAddress { get; set; }
		public string Type { get; set; }
	}
}
