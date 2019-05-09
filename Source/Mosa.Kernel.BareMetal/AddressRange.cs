// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal
{
	public struct AddressRange
	{
		public IntPtr Address { get; }
		public int Size { get; }

		public AddressRange(IntPtr address, int size)
		{
			Address = address;
			Size = size;
		}

		public AddressRange(int address, int size)
		{
			Address = new IntPtr(address);
			Size = size;
		}

		public AddressRange(long address, int size)
		{
			Address = new IntPtr(address);
			Size = size;
		}
	}
}
