using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Threading;

public static class Volatile
{
	public static bool Read([In] ref bool location)
	{
		throw null;
	}

	public static byte Read([In] ref byte location)
	{
		throw null;
	}

	public static double Read([In] ref double location)
	{
		throw null;
	}

	public static short Read([In] ref short location)
	{
		throw null;
	}

	public static int Read([In] ref int location)
	{
		throw null;
	}

	public static long Read([In] ref long location)
	{
		throw null;
	}

	public static IntPtr Read([In] ref IntPtr location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte Read([In] ref sbyte location)
	{
		throw null;
	}

	public static float Read([In] ref float location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort Read([In] ref ushort location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint Read([In] ref uint location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong Read([In] ref ulong location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static UIntPtr Read([In] ref UIntPtr location)
	{
		throw null;
	}

	[return: NotNullIfNotNull("location")]
	public static T Read<T>([In][NotNullIfNotNull("location")] ref T location) where T : class?
	{
		throw null;
	}

	public static void Write(ref bool location, bool value)
	{
	}

	public static void Write(ref byte location, byte value)
	{
	}

	public static void Write(ref double location, double value)
	{
	}

	public static void Write(ref short location, short value)
	{
	}

	public static void Write(ref int location, int value)
	{
	}

	public static void Write(ref long location, long value)
	{
	}

	public static void Write(ref IntPtr location, IntPtr value)
	{
	}

	[CLSCompliant(false)]
	public static void Write(ref sbyte location, sbyte value)
	{
	}

	public static void Write(ref float location, float value)
	{
	}

	[CLSCompliant(false)]
	public static void Write(ref ushort location, ushort value)
	{
	}

	[CLSCompliant(false)]
	public static void Write(ref uint location, uint value)
	{
	}

	[CLSCompliant(false)]
	public static void Write(ref ulong location, ulong value)
	{
	}

	[CLSCompliant(false)]
	public static void Write(ref UIntPtr location, UIntPtr value)
	{
	}

	public static void Write<T>([NotNullIfNotNull("value")] ref T location, T value) where T : class?
	{
	}
}
