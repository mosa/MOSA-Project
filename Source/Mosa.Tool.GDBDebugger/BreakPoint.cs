// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger
{
	public class BreakPoint
	{
		public string Name { get; private set; }

		public ulong Address { get; private set; }

		public BreakPoint(ulong address)
		{
			Address = address;
		}

		public BreakPoint(string name, ulong address)
			: this(address)
		{
			Name = name;
		}
	}
}
