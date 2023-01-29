// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime;

/// <summary>
/// </summary>
public static class Internal
{
	#region Memory Manipulation

	public static void MemoryCopy(Pointer dest, Pointer src, uint count)
	{
		// FUTURE: Improve
		for (int i = 0; i < count; i++)
		{
			byte value = src.Load8(i);
			dest.Store8(i, value);
		}
	}

	public static void MemorySet(Pointer dest, byte value, uint count)
	{
		// FUTURE: Improve
		for (int i = 0; i < count; i++)
		{
			dest.Store8(i, value);
		}
	}

	public static void MemorySet(Pointer dest, ushort value, uint count)
	{
		// FUTURE: Improve
		for (int i = 0; i < count; i += 2)
		{
			dest.Store16(i, value);
		}
	}

	public static void MemorySet(Pointer dest, uint value, uint count)
	{
		// FUTURE: Improve
		for (int i = 0; i < count; i += 4)
		{
			dest.Store32(i, value);
		}
	}

	public static void MemoryClear(Pointer dest, uint count)
	{
		// FUTURE: Improve
		for (int i = 0; i < count; i++)
		{
			dest.Store8(i, 0);
		}
	}

	#endregion Memory Manipulation
}
