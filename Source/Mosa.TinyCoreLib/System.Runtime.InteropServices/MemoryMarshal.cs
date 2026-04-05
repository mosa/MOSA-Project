using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices;

public static class MemoryMarshal
{
	public static ReadOnlySpan<byte> AsBytes<T>(ReadOnlySpan<T> span) where T : struct => throw new NotImplementedException();

	public static Span<byte> AsBytes<T>(Span<T> span) where T : struct => throw new NotImplementedException();

	public static Memory<T> AsMemory<T>(ReadOnlyMemory<T> memory) => throw new NotImplementedException();

	public static ref readonly T AsRef<T>(ReadOnlySpan<byte> span) where T : struct => throw new NotImplementedException();

	public static ref T AsRef<T>(Span<byte> span) where T : struct => throw new NotImplementedException();

	public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span) where TFrom : struct where TTo : struct
	{
		if (Internal.Impl.RuntimeHelpers.IsReferenceOrContainsReferences<TFrom>())
			Internal.Exceptions.Span.ThrowReferenceException();

		if (Internal.Impl.RuntimeHelpers.IsReferenceOrContainsReferences<TTo>())
			Internal.Exceptions.Span.ThrowReferenceException();

		var toSize = Unsafe.SizeOf<TTo>();

		if ((long)toSize * span.Length > int.MaxValue)
			Internal.Exceptions.Generic.Overflow();

		var fromSize = Unsafe.SizeOf<TFrom>();
		var newLength = fromSize != toSize ? fromSize * span.Length / toSize : span.Length;

		return new ReadOnlySpan<TTo>(ref Unsafe.As<TFrom, TTo>(ref GetReference(span)), newLength);
	}

	public static Span<TTo> Cast<TFrom, TTo>(Span<TFrom> span) where TFrom : struct where TTo : struct
	{
		if (Internal.Impl.RuntimeHelpers.IsReferenceOrContainsReferences<TFrom>())
			Internal.Exceptions.Span.ThrowReferenceException();

		if (Internal.Impl.RuntimeHelpers.IsReferenceOrContainsReferences<TTo>())
			Internal.Exceptions.Span.ThrowReferenceException();

		var toSize = Unsafe.SizeOf<TTo>();

		if ((long)toSize * span.Length > int.MaxValue)
			Internal.Exceptions.Generic.Overflow();

		var fromSize = Unsafe.SizeOf<TFrom>();
		var newLength = fromSize != toSize ? fromSize * span.Length / toSize : span.Length;

		return new Span<TTo>(ref Unsafe.As<TFrom, TTo>(ref GetReference(span)), newLength);
	}

	public static Memory<T> CreateFromPinnedArray<T>(T[]? array, int start, int length) => throw new NotImplementedException();

	public static ReadOnlySpan<T> CreateReadOnlySpan<T>([In] scoped ref T reference, int length) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public unsafe static ReadOnlySpan<byte> CreateReadOnlySpanFromNullTerminated(byte* value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public unsafe static ReadOnlySpan<char> CreateReadOnlySpanFromNullTerminated(char* value) => throw new NotImplementedException();

	public static Span<T> CreateSpan<T>(scoped ref T reference, int length) => throw new NotImplementedException();

	public static ref T GetArrayDataReference<T>(T[] array) => throw new NotImplementedException();

	public static ref byte GetArrayDataReference(Array array) => throw new NotImplementedException();

	public static ref T GetReference<T>(ReadOnlySpan<T> span) => ref span.backingPointer;

	public static ref T GetReference<T>(Span<T> span) => ref span.backingPointer;

	public static T Read<T>(ReadOnlySpan<byte> source) where T : struct
	{
		if (Internal.Impl.RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			Internal.Exceptions.Span.ThrowReferenceException();

		ArgumentOutOfRangeException.ThrowIfLessThan(source.Length, Unsafe.SizeOf<T>(), nameof(source));

		return Unsafe.ReadUnaligned<T>(ref GetReference(source));
	}

	public static IEnumerable<T> ToEnumerable<T>(ReadOnlyMemory<T> memory) => throw new NotImplementedException();

	public static bool TryGetArray<T>(ReadOnlyMemory<T> memory, out ArraySegment<T> segment) => throw new NotImplementedException();

	public static bool TryGetMemoryManager<T, TManager>(ReadOnlyMemory<T> memory, [NotNullWhen(true)] out TManager? manager) where TManager : MemoryManager<T> => throw new NotImplementedException();

	public static bool TryGetMemoryManager<T, TManager>(ReadOnlyMemory<T> memory, [NotNullWhen(true)] out TManager? manager, out int start, out int length) where TManager : MemoryManager<T> => throw new NotImplementedException();

	public static bool TryGetString(ReadOnlyMemory<char> memory, [NotNullWhen(true)] out string? text, out int start, out int length) => throw new NotImplementedException();

	public static bool TryRead<T>(ReadOnlySpan<byte> source, out T value) where T : struct => throw new NotImplementedException();

	public static bool TryWrite<T>(Span<byte> destination, in T value) where T : struct => throw new NotImplementedException();

	public static void Write<T>(Span<byte> destination, in T value) where T : struct => throw new NotImplementedException();
}
