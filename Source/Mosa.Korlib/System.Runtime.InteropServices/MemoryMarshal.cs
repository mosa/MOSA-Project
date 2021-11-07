// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
				throw new ArgumentException("Type is reference or contains references.");

			if (Unsafe.SizeOf<T>() > source.Length)
				throw new ArgumentException("Type length is above source length.");

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
				throw new ArgumentException("Type is reference or contains references.");

			if (Unsafe.SizeOf<T>() > source.Length)
				throw new ArgumentException("Type length is above source length.");

			return Unsafe.ReadUnaligned<T>(ref GetReference(source));
		}

		/// <summary>
        /// Casts a Span of one primitive type <typeparamref name="TFrom"/> to another primitive type <typeparamref name="TTo"/>.
        /// These types may not contain pointers or references. This is checked at runtime in order to preserve type safety.
        /// </summary>
        /// <remarks>
        /// Supported only for platforms that support misaligned memory access or when the memory block is aligned by other means.
        /// </remarks>
        /// <param name="span">The source slice, of type <typeparamref name="TFrom"/>.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <typeparamref name="TFrom"/> or <typeparamref name="TTo"/> contains pointers.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<TTo> Cast<TFrom, TTo>(Span<TFrom> span)
            where TFrom : struct
            where TTo : struct
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<TFrom>())
                throw new Exception("Type is reference or contains references.");
            if (RuntimeHelpers.IsReferenceOrContainsReferences<TTo>())
                throw new Exception("Type is reference or contains references.");

            // Use unsigned integers - unsigned division by constant (especially by power of 2)
            // and checked casts are faster and smaller.
            uint fromSize = (uint)Unsafe.SizeOf<TFrom>();
            uint toSize = (uint)Unsafe.SizeOf<TTo>();
            uint fromLength = (uint)span.Length;
            int toLength;
            if (fromSize == toSize)
            {
                // Special case for same size types - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
                // should be optimized to just `length` but the JIT doesn't do that today.
                toLength = (int)fromLength;
            }
            else if (fromSize == 1)
            {
                // Special case for byte sized TFrom - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
                // becomes `(ulong)fromLength / (ulong)toSize` but the JIT can't narrow it down to `int`
                // and can't eliminate the checked cast. This also avoids a 32 bit specific issue,
                // the JIT can't eliminate long multiply by 1.
                toLength = (int)(fromLength / toSize);
            }
            else
            {
                // Ensure that casts are done in such a way that the JIT is able to "see"
                // the uint->ulong casts and the multiply together so that on 32 bit targets
                // 32x32to64 multiplication is used.
                ulong toLengthUInt64 = (ulong)fromLength * (ulong)fromSize / (ulong)toSize;
                // TODO: checked keyword
                toLength = (int)toLengthUInt64;
            }

            return new Span<TTo>(
                ref Unsafe.As<TFrom, TTo>(ref span._pointer.Value),
                toLength);
        }

        /// <summary>
        /// Casts a ReadOnlySpan of one primitive type <typeparamref name="TFrom"/> to another primitive type <typeparamref name="TTo"/>.
        /// These types may not contain pointers or references. This is checked at runtime in order to preserve type safety.
        /// </summary>
        /// <remarks>
        /// Supported only for platforms that support misaligned memory access or when the memory block is aligned by other means.
        /// </remarks>
        /// <param name="span">The source slice, of type <typeparamref name="TFrom"/>.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <typeparamref name="TFrom"/> or <typeparamref name="TTo"/> contains pointers.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span)
            where TFrom : struct
            where TTo : struct
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<TFrom>())
                throw new Exception("Type is reference or contains references.");
            if (RuntimeHelpers.IsReferenceOrContainsReferences<TTo>())
                throw new Exception("Type is reference or contains references.");

            // Use unsigned integers - unsigned division by constant (especially by power of 2)
            // and checked casts are faster and smaller.
            uint fromSize = (uint)Unsafe.SizeOf<TFrom>();
            uint toSize = (uint)Unsafe.SizeOf<TTo>();
            uint fromLength = (uint)span.Length;
            int toLength;
            if (fromSize == toSize)
            {
                // Special case for same size types - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
                // should be optimized to just `length` but the JIT doesn't do that today.
                toLength = (int)fromLength;
            }
            else if (fromSize == 1)
            {
                // Special case for byte sized TFrom - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
                // becomes `(ulong)fromLength / (ulong)toSize` but the JIT can't narrow it down to `int`
                // and can't eliminate the checked cast. This also avoids a 32 bit specific issue,
                // the JIT can't eliminate long multiply by 1.
                toLength = (int)(fromLength / toSize);
            }
            else
            {
                // Ensure that casts are done in such a way that the JIT is able to "see"
                // the uint->ulong casts and the multiply together so that on 32 bit targets
                // 32x32to64 multiplication is used.
                ulong toLengthUInt64 = (ulong)fromLength * (ulong)fromSize / (ulong)toSize;
                // TODO: checked keyword
                toLength = (int)toLengthUInt64;
            }

            return new ReadOnlySpan<TTo>(
                ref Unsafe.As<TFrom, TTo>(ref MemoryMarshal.GetReference(span)),
                toLength);
        }
	}
}
