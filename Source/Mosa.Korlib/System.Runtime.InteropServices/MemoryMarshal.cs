using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
	public static partial class MemoryMarshal
	{
		/// <summary>
		/// Returns a reference to the 0th element of the Span. If the Span is empty, returns a reference to the location where the 0th element
		/// would have been stored. Such a reference may or may not be null. It can be used for pinning but must never be dereferenced.
		/// </summary>
		public static ref T GetReference<T>(Span<T> span) => ref span._pointer.Value;

		/// <summary>
		/// Returns a reference to the 0th element of the ReadOnlySpan. If the ReadOnlySpan is empty, returns a reference to the location where the 0th element
		/// would have been stored. Such a reference may or may not be null. It can be used for pinning but must never be dereferenced.
		/// </summary>
		public static ref T GetReference<T>(ReadOnlySpan<T> span) => ref span._pointer.Value;

		/// <summary>
		/// Reads a structure of type T out of a read-only span of bytes.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Read<T>(ReadOnlySpan<byte> source)
			where T : struct
		{
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new Exception("Type is reference or contains references."); //ThrowHelper.ThrowInvalidTypeWithPointersNotSupported(typeof(T));

			if (Unsafe.SizeOf<T>() > source.Length)
				throw new Exception("Type length is above source length."); //ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);

			return Unsafe.ReadUnaligned<T>(ref GetReference(source));
		}

		/// <summary>
		/// Reads a structure of type T out of a span of bytes.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Read<T>(Span<byte> source)
			where T : struct
		{
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new Exception("Type is reference or contains references."); //ThrowHelper.ThrowInvalidTypeWithPointersNotSupported(typeof(T));

			if (Unsafe.SizeOf<T>() > source.Length)
				throw new Exception("Type length is above source length."); //ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);

			return Unsafe.ReadUnaligned<T>(ref GetReference(source));
		}
	}
}