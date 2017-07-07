// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger.DebugData
{
	public class ParameterInfo
	{
		public uint Index { get; set; }
		public uint Offset { get; set; }
		public string FullName { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Method { get; set; }
		public uint Attributes { get; set; }
	}
}
