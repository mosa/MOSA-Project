// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger
{
	public class BreakPoint
	{
		public string Name { get; }

		public ulong Address { get; }

		public string Description { get; private set; }

		public BreakPoint(ulong address)
		{
			Address = address;
		}

		public BreakPoint(string name, ulong address, string description = null)
			: this(address)
		{
			Name = name;
			Description = description;
		}
	}
}
