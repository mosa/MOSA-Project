// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger
{
	public class AddBreakPointArgs
	{
		public string Name { get; private set; }

		public string Description { get; private set; }

		public ulong Address { get; private set; }

		public AddBreakPointArgs(string name, ulong address, string description = null)
		{
			Name = name;
			Address = address;
			Description = description;
		}
	}
}
