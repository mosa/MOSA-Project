// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common;

public static class Alignment
{
	public static uint AlignUp(uint position, uint alignment)
	{
		return position + (alignment - position % alignment) % alignment;
	}

	public static int AlignUp(int position, int alignment)
	{
		return position + (alignment - position % alignment) % alignment;
	}

	public static ulong AlignUp(ulong position, uint alignment)
	{
		return position + (alignment - position % alignment) % alignment;
	}

	public static uint AlignDown(uint position, uint alignment)
	{
		return position - (position % alignment);
	}

	public static int AlignDown(int position, int alignment)
	{
		return position - (position % alignment);
	}

	public static ulong AlignDown(ulong position, uint alignment)
	{
		return position - (position % alignment);
	}
}
