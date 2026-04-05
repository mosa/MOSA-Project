using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Internal;

namespace System.Runtime.CompilerServices;

public static class Unsafe
{
	public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
		=> ref Impl.Unsafe.AddByteOffset(ref source, byteOffset);

	[CLSCompliant(false)]
	public static ref T AddByteOffset<T>(ref T source, UIntPtr byteOffset) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static unsafe void* Add<T>(void* source, int elementOffset)
		=> (byte*)source + elementOffset * (nint)SizeOf<T>();

	public static ref T Add<T>(ref T source, int elementOffset)
		=> ref AddByteOffset(ref source, elementOffset * (nint)SizeOf<T>());

	public static ref T Add<T>(ref T source, IntPtr elementOffset)
		=> ref AddByteOffset(ref source, elementOffset * SizeOf<T>());

	[CLSCompliant(false)]
	public static ref T Add<T>(ref T source, UIntPtr elementOffset) => throw new NotImplementedException();

	public static bool AreSame<T>([In][AllowNull] ref T left, [In][AllowNull] ref T right)
		=> Impl.Unsafe.AreSame(ref left, ref right);

	[CLSCompliant(false)]
	public static unsafe void* AsPointer<T>(ref T value) => Impl.Unsafe.AsPointer(ref value);

	[CLSCompliant(false)]
	public static unsafe ref T AsRef<T>(void* source) => ref As<byte, T>(ref *(byte*)source);

	public static ref T AsRef<T>([In] scoped ref T source) => throw new NotImplementedException();

	[return: NotNullIfNotNull("o")]
	public static T? As<T>(object? o) where T : class? => Impl.Unsafe.As<T>(o);

	public static ref TTo As<TFrom, TTo>(ref TFrom source) => ref Impl.Unsafe.As<TFrom, TTo>(ref source);

	public static TTo BitCast<TFrom, TTo>(TFrom source) where TFrom : struct where TTo : struct
		=> throw new NotImplementedException();

	public static IntPtr ByteOffset<T>([In][AllowNull] ref T origin, [In][AllowNull] ref T target)
		=> throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void CopyBlock(ref byte destination, [In] ref byte source, uint byteCount)
		=> throw new NotImplementedException();

	[CLSCompliant(false)]
	public static unsafe void CopyBlock(void* destination, void* source, uint byteCount)
		=> throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void CopyBlockUnaligned(ref byte destination, [In] ref byte source, uint byteCount)
		=> throw new NotImplementedException();

	[CLSCompliant(false)]
	public static unsafe void CopyBlockUnaligned(void* destination, void* source, uint byteCount)
		=> throw new NotImplementedException();

	[CLSCompliant(false)]
	public static unsafe void Copy<T>(void* destination, [In] ref T source) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static unsafe void Copy<T>(ref T destination, void* source) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void InitBlock(ref byte startAddress, byte value, uint byteCount)
		=> throw new NotImplementedException();

	[CLSCompliant(false)]
	public static unsafe void InitBlock(void* startAddress, byte value, uint byteCount)
		=> throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
	{
		for (var i = 0U; i < byteCount; i++)
			AddByteOffset(ref startAddress, i) = value;
	}

	[CLSCompliant(false)]
	public static unsafe void InitBlockUnaligned(void* startAddress, byte value, uint byteCount)
	{
		for (var i = 0U; i < byteCount; i++)
			AddByteOffset(ref *(byte*)startAddress, i) = value;
	}

	public static bool IsAddressGreaterThan<T>([In][AllowNull] ref T left, [In][AllowNull] ref T right)
		=> throw new NotImplementedException();

	public static bool IsAddressLessThan<T>([In][AllowNull] ref T left, [In][AllowNull] ref T right)
		=> throw new NotImplementedException();

	public static bool IsNullRef<T>([In] ref T source)
	{
		unsafe
		{
			return AsPointer(ref source) == null;
		}
	}

	// TODO
	public static ref T NullRef<T>() => throw new NotImplementedException();

	public static T ReadUnaligned<T>([In] ref byte source) => As<byte, T>(ref source);

	[CLSCompliant(false)]
	public static unsafe T ReadUnaligned<T>(void* source) => As<byte, T>(ref *(byte*)source);

	[CLSCompliant(false)]
	public static unsafe T Read<T>(void* source) => As<byte, T>(ref *(byte*)source);

	public static void SkipInit<T>(out T value) => throw new NotImplementedException();

	public static int SizeOf<T>() => Impl.Unsafe.SizeOf<T>();

	public static ref T SubtractByteOffset<T>(ref T source, IntPtr byteOffset) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static ref T SubtractByteOffset<T>(ref T source, UIntPtr byteOffset) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static unsafe void* Subtract<T>(void* source, int elementOffset) => throw new NotImplementedException();

	public static ref T Subtract<T>(ref T source, int elementOffset) => throw new NotImplementedException();

	public static ref T Subtract<T>(ref T source, IntPtr elementOffset) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static ref T Subtract<T>(ref T source, UIntPtr elementOffset) => throw new NotImplementedException();

	public static ref T Unbox<T>(object box) where T : struct => throw new NotImplementedException();

	public static void WriteUnaligned<T>(ref byte destination, T value)
		=> As<byte, T>(ref destination) = value;

	[CLSCompliant(false)]
	public static unsafe void WriteUnaligned<T>(void* destination, T value)
		=> As<byte, T>(ref *(byte*)destination) = value;

	[CLSCompliant(false)]
	public static unsafe void Write<T>(void* destination, T value)
		=> As<byte, T>(ref *(byte*)destination) = value;
}
