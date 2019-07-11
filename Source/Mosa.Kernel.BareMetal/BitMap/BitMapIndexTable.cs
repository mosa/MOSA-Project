// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	public /*readonly*/ struct BitMapIndexTable
	{
		private readonly IntPtr Ptr;

		public BitMapIndexTable(IntPtr page)
		{
			Ptr = page;
			Page.ClearPage(Ptr);
		}

		public void AddBitMapEntry(uint index, IntPtr page)
		{
			Ptr.StorePointer(index * IntPtr.Size, page);
		}

		public IntPtr GetBitMapEntry(uint index)
		{
			return Ptr.LoadPointer((uint)(index * IntPtr.Size));
		}
	}
}
