// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public struct BitMapIndexTable
{
	private readonly Pointer Pointer;

	public BitMapIndexTable(Pointer page)
	{
		Pointer = page;
		Page.ClearPage(Pointer);
	}

	public void AddBitMapEntry(uint index, Pointer page)
	{
		Pointer.StorePointer(index * Pointer.Size, page);
	}

	public Pointer GetBitMapEntry(uint index)
	{
		return Pointer.LoadPointer((uint)(index * Pointer.Size));
	}
}
