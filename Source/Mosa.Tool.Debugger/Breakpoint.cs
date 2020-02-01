// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger
{
	public class BreakPoint
	{
		public string Name { get; }

		public ulong Address { get; }

		public string Description { get; private set; }

		public bool Temporary { get; set; }

		public BreakPoint(ulong address)
		{
			Address = address;
		}

		public BreakPoint(string name, ulong address, string description = null)
			: this(address)
		{
			Name = name;
			Description = description;
			Temporary = false;
		}

		public BreakPoint(string name, ulong address, bool temporary, string description = null)
			: this(address)
		{
			Name = name;
			Description = description;
			Temporary = temporary;
		}
	}
}
