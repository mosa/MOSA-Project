// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator
{
	public class SimSymbol
	{
		public string Name { get; private set; }

		public ulong Address { get; private set; }

		public ulong Size { get; private set; }

		public ulong EndAddress { get { return Size == 0 ? Address : Address + Size - 1; } }

		public SimSymbol(string name, ulong address, ulong size)
		{
			Name = name;
			Address = address;
			Size = size;
		}

		public override string ToString()
		{
			return "0x" + Address.ToString("X") + " - " + "0x" + EndAddress.ToString("X") + " " + Name + " (" + Size.ToString() + ")";
		}
	}
}
