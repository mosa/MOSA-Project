// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Runtime.CompilerServices
{
	public static unsafe class Unsafe
	{
		/// <summary>
		/// Returns a pointer to the given by-ref parameter.
		/// </summary>
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void* AsPointer<T>(ref T value)
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>
		/// Returns the size of an object of the given type parameter.
		/// </summary>
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SizeOf<T>()
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>
		/// Casts the given object to the specified type, performs no dynamic type checking.
		/// </summary>
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: NotNullIfNotNull("value")]
		public static T As<T>([AllowNull] object value) where T : class
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>
		/// Reinterprets the given reference as a reference to a value of type <typeparamref name="TTo"/>.
		/// </summary>
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref TTo As<TFrom, TTo>(ref TFrom source)
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>
		/// Adds an element offset to the given reference.
		/// </summary>
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Add<T>(ref T source, int elementOffset)
		{
			return ref AddByteOffset(ref source, (IntPtr)(elementOffset * (nint)SizeOf<T>()));
		}

		/// <summary>
		/// Adds an element offset to the given reference.
		/// </summary>
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Add<T>(ref T source, IntPtr elementOffset)
		{
			return ref AddByteOffset(ref source, (IntPtr)((nint)elementOffset * (nint)SizeOf<T>()));
		}

		/// <summary>
		/// Adds an element offset to the given pointer.
		/// </summary>
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void* Add<T>(void* source, int elementOffset)
		{
			return (byte*)source + (elementOffset * (nint)SizeOf<T>());
		}

		/// <summary>
		/// Determines whether the specified references point to the same location.
		/// </summary>
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool AreSame<T>([AllowNull] ref T left, [AllowNull] ref T right)
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>
		/// Adds an byte offset to the given reference.
		/// </summary>
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
		{
			throw new PlatformNotSupportedException();
		}

		[Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUnaligned<T>(ref byte destination, T value)
        {
        	As<byte, T>(ref destination) = value;
        }

        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadUnaligned<T>(ref byte source)
        {
        	return As<byte, T>(ref source);
        }

		//[Intrinsic]
		//[NonVersionable]
		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static ref T AsRef<T>(void* source)
		//{
		//	return ref Unsafe.As<byte, T>(ref *(byte*)source);
		//}
	}
}
