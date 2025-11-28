using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Threading;

public static class Interlocked
{
	public static int Add(ref int location1, int value)
	{
		throw null;
	}

	public static long Add(ref long location1, long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint Add(ref uint location1, uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong Add(ref ulong location1, ulong value)
	{
		throw null;
	}

	public static int And(ref int location1, int value)
	{
		throw null;
	}

	public static long And(ref long location1, long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint And(ref uint location1, uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong And(ref ulong location1, ulong value)
	{
		throw null;
	}

	public static double CompareExchange(ref double location1, double value, double comparand)
	{
		throw null;
	}

	public static int CompareExchange(ref int location1, int value, int comparand)
	{
		throw null;
	}

	public static long CompareExchange(ref long location1, long value, long comparand)
	{
		throw null;
	}

	public static IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static UIntPtr CompareExchange(ref UIntPtr location1, UIntPtr value, UIntPtr comparand)
	{
		throw null;
	}

	[return: NotNullIfNotNull("location1")]
	public static object? CompareExchange(ref object? location1, object? value, object? comparand)
	{
		throw null;
	}

	public static float CompareExchange(ref float location1, float value, float comparand)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint CompareExchange(ref uint location1, uint value, uint comparand)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong CompareExchange(ref ulong location1, ulong value, ulong comparand)
	{
		throw null;
	}

	[return: NotNullIfNotNull("location1")]
	public static T CompareExchange<T>(ref T location1, T value, T comparand) where T : class?
	{
		throw null;
	}

	public static int Decrement(ref int location)
	{
		throw null;
	}

	public static long Decrement(ref long location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint Decrement(ref uint location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong Decrement(ref ulong location)
	{
		throw null;
	}

	public static double Exchange(ref double location1, double value)
	{
		throw null;
	}

	public static int Exchange(ref int location1, int value)
	{
		throw null;
	}

	public static long Exchange(ref long location1, long value)
	{
		throw null;
	}

	public static IntPtr Exchange(ref IntPtr location1, IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static UIntPtr Exchange(ref UIntPtr location1, UIntPtr value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("location1")]
	public static object? Exchange([NotNullIfNotNull("value")] ref object? location1, object? value)
	{
		throw null;
	}

	public static float Exchange(ref float location1, float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint Exchange(ref uint location1, uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong Exchange(ref ulong location1, ulong value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("location1")]
	public static T Exchange<T>([NotNullIfNotNull("value")] ref T location1, T value) where T : class?
	{
		throw null;
	}

	public static int Increment(ref int location)
	{
		throw null;
	}

	public static long Increment(ref long location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint Increment(ref uint location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong Increment(ref ulong location)
	{
		throw null;
	}

	public static void MemoryBarrier()
	{
	}

	public static void MemoryBarrierProcessWide()
	{
	}

	public static int Or(ref int location1, int value)
	{
		throw null;
	}

	public static long Or(ref long location1, long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint Or(ref uint location1, uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong Or(ref ulong location1, ulong value)
	{
		throw null;
	}

	public static long Read([In] ref long location)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong Read([In] ref ulong location)
	{
		throw null;
	}
}
