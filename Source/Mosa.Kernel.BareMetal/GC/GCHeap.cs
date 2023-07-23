// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.GC;

public struct GCHeap
{
	private readonly Pointer Pointer;

	public GCHeap(Pointer entry) => Pointer = entry;

	public Pointer Address
	{
		get => Pointer.LoadPointer();
		set => Pointer.StorePointer(value);
	}

	public uint Size
	{
		get => Pointer.Load32(Pointer.Size);
		set => Pointer.Store32(Pointer.Size, value);
	}

	public uint Used
	{
		get => Pointer.Load32(Pointer.Size + sizeof(uint));
		set => Pointer.Store32(Pointer.Size + sizeof(uint), value);
	}

	public Pointer EndAddress => Address + Size;
}
