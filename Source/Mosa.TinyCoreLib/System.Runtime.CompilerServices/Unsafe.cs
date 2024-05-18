using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

public static class Unsafe
{
	public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ref T AddByteOffset<T>(ref T source, UIntPtr byteOffset)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void* Add<T>(void* source, int elementOffset)
	{
		throw null;
	}

	public static ref T Add<T>(ref T source, int elementOffset)
	{
		throw null;
	}

	public static ref T Add<T>(ref T source, IntPtr elementOffset)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ref T Add<T>(ref T source, UIntPtr elementOffset)
	{
		throw null;
	}

	public static bool AreSame<T>([In][AllowNull] ref T left, [In][AllowNull] ref T right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void* AsPointer<T>(ref T value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static ref T AsRef<T>(void* source)
	{
		throw null;
	}

	public static ref T AsRef<T>([In] scoped ref T source)
	{
		throw null;
	}

	[return: NotNullIfNotNull("o")]
	public static T? As<T>(object? o) where T : class?
	{
		throw null;
	}

	public static ref TTo As<TFrom, TTo>(ref TFrom source)
	{
		throw null;
	}

	public static TTo BitCast<TFrom, TTo>(TFrom source) where TFrom : struct where TTo : struct
	{
		throw null;
	}

	public static IntPtr ByteOffset<T>([In][AllowNull] ref T origin, [In][AllowNull] ref T target)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static void CopyBlock(ref byte destination, [In] ref byte source, uint byteCount)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void CopyBlock(void* destination, void* source, uint byteCount)
	{
	}

	[CLSCompliant(false)]
	public static void CopyBlockUnaligned(ref byte destination, [In] ref byte source, uint byteCount)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void CopyBlockUnaligned(void* destination, void* source, uint byteCount)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void Copy<T>(void* destination, [In] ref T source)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void Copy<T>(ref T destination, void* source)
	{
	}

	[CLSCompliant(false)]
	public static void InitBlock(ref byte startAddress, byte value, uint byteCount)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void InitBlock(void* startAddress, byte value, uint byteCount)
	{
	}

	[CLSCompliant(false)]
	public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void InitBlockUnaligned(void* startAddress, byte value, uint byteCount)
	{
	}

	public static bool IsAddressGreaterThan<T>([In][AllowNull] ref T left, [In][AllowNull] ref T right)
	{
		throw null;
	}

	public static bool IsAddressLessThan<T>([In][AllowNull] ref T left, [In][AllowNull] ref T right)
	{
		throw null;
	}

	public static bool IsNullRef<T>([In] ref T source)
	{
		throw null;
	}

	public static ref T NullRef<T>()
	{
		throw null;
	}

	public static T ReadUnaligned<T>([In] ref byte source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static T ReadUnaligned<T>(void* source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static T Read<T>(void* source)
	{
		throw null;
	}

	public static void SkipInit<T>(out T value)
	{
		throw null;
	}

	public static int SizeOf<T>()
	{
		throw null;
	}

	public static ref T SubtractByteOffset<T>(ref T source, IntPtr byteOffset)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ref T SubtractByteOffset<T>(ref T source, UIntPtr byteOffset)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void* Subtract<T>(void* source, int elementOffset)
	{
		throw null;
	}

	public static ref T Subtract<T>(ref T source, int elementOffset)
	{
		throw null;
	}

	public static ref T Subtract<T>(ref T source, IntPtr elementOffset)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ref T Subtract<T>(ref T source, UIntPtr elementOffset)
	{
		throw null;
	}

	public static ref T Unbox<T>(object box) where T : struct
	{
		throw null;
	}

	public static void WriteUnaligned<T>(ref byte destination, T value)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void WriteUnaligned<T>(void* destination, T value)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void Write<T>(void* destination, T value)
	{
	}
}
