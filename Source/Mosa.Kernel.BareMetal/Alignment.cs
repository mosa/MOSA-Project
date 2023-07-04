// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public static class Alignment
{
	public static uint AlignUp(uint position, uint alignment)
	{
		return position + (alignment - position % alignment) % alignment;
	}

	public static int AlignUp(int position, int alignment)
	{
		return (int)AlignDown((uint)position, (uint)alignment);
	}

	public static ulong AlignUp(ulong position, uint alignment)
	{
		return position + (alignment - position % alignment) % alignment;
	}

	public static ulong AlignUp(ulong position, ulong alignment)
	{
		return position + (alignment - position % alignment) % alignment;
	}

	public static ulong AlignUp(long position, uint alignment)
	{
		return AlignDown(position, (int)alignment);
	}

	public static uint AlignDown(uint position, uint alignment)
	{
		return position - (position % alignment);
	}

	public static int AlignDown(int position, int alignment)
	{
		return (int)AlignDown((uint)position, (uint)alignment);
	}

	public static ulong AlignDown(ulong position, uint alignment)
	{
		return position - (position % alignment);
	}

	public static ulong AlignDown(ulong position, int alignment)
	{
		return AlignDown(position, (uint)alignment);
	}

	public static ulong AlignDown(long position, int alignment)
	{
		return AlignDown((ulong)position, (uint)alignment);
	}

	public static ulong AlignDown(long position, uint alignment)
	{
		return AlignDown((ulong)position, alignment);
	}

	public static ulong AlignDown(ulong position, ulong alignment)
	{
		return position - (position % alignment);
	}
}
