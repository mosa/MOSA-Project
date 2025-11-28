using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices;

public static class MemoryMarshal
{
	public static ReadOnlySpan<byte> AsBytes<T>(ReadOnlySpan<T> span) where T : struct
	{
		throw null;
	}

	public static Span<byte> AsBytes<T>(Span<T> span) where T : struct
	{
		throw null;
	}

	public static Memory<T> AsMemory<T>(ReadOnlyMemory<T> memory)
	{
		throw null;
	}

	public static ref readonly T AsRef<T>(ReadOnlySpan<byte> span) where T : struct
	{
		throw null;
	}

	public static ref T AsRef<T>(Span<byte> span) where T : struct
	{
		throw null;
	}

	public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span) where TFrom : struct where TTo : struct
	{
		throw null;
	}

	public static Span<TTo> Cast<TFrom, TTo>(Span<TFrom> span) where TFrom : struct where TTo : struct
	{
		throw null;
	}

	public static Memory<T> CreateFromPinnedArray<T>(T[]? array, int start, int length)
	{
		throw null;
	}

	public static ReadOnlySpan<T> CreateReadOnlySpan<T>([In] scoped ref T reference, int length)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static ReadOnlySpan<byte> CreateReadOnlySpanFromNullTerminated(byte* value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static ReadOnlySpan<char> CreateReadOnlySpanFromNullTerminated(char* value)
	{
		throw null;
	}

	public static Span<T> CreateSpan<T>(scoped ref T reference, int length)
	{
		throw null;
	}

	public static ref T GetArrayDataReference<T>(T[] array)
	{
		throw null;
	}

	public static ref byte GetArrayDataReference(Array array)
	{
		throw null;
	}

	public static ref T GetReference<T>(ReadOnlySpan<T> span)
	{
		throw null;
	}

	public static ref T GetReference<T>(Span<T> span)
	{
		throw null;
	}

	public static T Read<T>(ReadOnlySpan<byte> source) where T : struct
	{
		throw null;
	}

	public static IEnumerable<T> ToEnumerable<T>(ReadOnlyMemory<T> memory)
	{
		throw null;
	}

	public static bool TryGetArray<T>(ReadOnlyMemory<T> memory, out ArraySegment<T> segment)
	{
		throw null;
	}

	public static bool TryGetMemoryManager<T, TManager>(ReadOnlyMemory<T> memory, [NotNullWhen(true)] out TManager? manager) where TManager : MemoryManager<T>
	{
		throw null;
	}

	public static bool TryGetMemoryManager<T, TManager>(ReadOnlyMemory<T> memory, [NotNullWhen(true)] out TManager? manager, out int start, out int length) where TManager : MemoryManager<T>
	{
		throw null;
	}

	public static bool TryGetString(ReadOnlyMemory<char> memory, [NotNullWhen(true)] out string? text, out int start, out int length)
	{
		throw null;
	}

	public static bool TryRead<T>(ReadOnlySpan<byte> source, out T value) where T : struct
	{
		throw null;
	}

	public static bool TryWrite<T>(Span<byte> destination, in T value) where T : struct
	{
		throw null;
	}

	public static void Write<T>(Span<byte> destination, in T value) where T : struct
	{
	}
}
