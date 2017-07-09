// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger.DebugData
{
	public class TypeInfo
	{
		public ulong DefAddress { get; set; }
		public uint Size { get; set; }
		public string FullName { get; set; }
		public string BaseType { get; set; }
		public string DeclaringType { get; set; }
		public string ElementType { get; set; }

		public override string ToString()
		{
			return FullName;
		}
	}
}
