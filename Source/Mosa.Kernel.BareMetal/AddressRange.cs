// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal
{
	public struct AddressRange
	{
		public IntPtr Address { get; }
		public ulong Size { get; }

		public AddressRange(IntPtr address, uint size)
		{
			Address = address;
			Size = size;
		}

		public AddressRange(uint address, uint size)
		{
			Address = new IntPtr(address);
			Size = size;
		}

		public AddressRange(ulong address, uint size)
		{
			Address = new IntPtr((long)address);
			Size = size;
		}

		public AddressRange(ulong address, ulong size)
		{
			Address = new IntPtr((long)address);
			Size = size;
		}
	}
}
