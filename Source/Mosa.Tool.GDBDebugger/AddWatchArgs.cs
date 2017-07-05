// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger
{
	public class AddWatchArgs
	{
		public string Name { get; private set; }

		public ulong Address { get; private set; }

		public int Length { get; private set; }

		public AddWatchArgs(string name, ulong address, int length)
		{
			Name = name;
			Address = address;
			Length = length;
		}
	}
}
