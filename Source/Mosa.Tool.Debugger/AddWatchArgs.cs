// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger
{
	public class AddWatchArgs
	{
		public string Name { get; private set; }

		public ulong Address { get; private set; }

		public uint Length { get; private set; }

		public AddWatchArgs(string name, ulong address, uint length)
		{
			Name = name;
			Address = address;
			Length = length;
		}
	}
}
