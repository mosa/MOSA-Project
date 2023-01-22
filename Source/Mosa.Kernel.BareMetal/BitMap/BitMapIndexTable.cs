// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BitMap
{
	public /*readonly*/ struct BitMapIndexTable
	{
		private readonly Pointer Ptr;

		public BitMapIndexTable(Pointer page)
		{
			Ptr = page;
			Page.ClearPage(Ptr);
		}

		public void AddBitMapEntry(uint index, Pointer page)
		{
			Ptr.StorePointer(index * Pointer.Size, page);
		}

		public Pointer GetBitMapEntry(uint index)
		{
			return Ptr.LoadPointer((uint)(index * Pointer.Size));
		}
	}
}
