// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger
{
	public class Watch
	{
		public string Name { get; private set; }

		public ulong Address { get; private set; }

		public uint Size { get; private set; }

		public bool Signed { get; private set; }

		public Watch(string name, ulong address, uint size, bool signed = false)
		{
			Name = name;
			Address = address;
			Size = size;
			Signed = signed;
		}
	}
}
