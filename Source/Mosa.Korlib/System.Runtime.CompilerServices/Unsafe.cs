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
        /// Initializes a block of memory at the given location with a given initial value
        /// without assuming architecture dependent alignment of the address.
        /// </summary>
        /*[Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
        {
            for (uint i = 0; i < byteCount; i++)
                AddByteOffset(ref startAddress, i) = value;
        }*/

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

		/// <summary>
        /// Adds an byte offset to the given reference.
        /// </summary>
		// TODO: Implement nuint
        /*[Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref T AddByteOffset<T>(ref T source, nuint byteOffset)
        {
            return ref AddByteOffset(ref source, (IntPtr)(void*)byteOffset);
        }*/

		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUnaligned<T>(ref byte destination, T value)
		{
			As<byte, T>(ref destination) = value;
		}

		/// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUnaligned<T>(void* destination, T value)
        {
        	As<byte, T>(ref *(byte*)destination) = value;
        }

		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T ReadUnaligned<T>(ref byte source)
		{
			return As<byte, T>(ref source);
		}

		/// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadUnaligned<T>(void* source)
        {
        	return As<byte, T>(ref *(byte*)source);
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(void* source)
        {
            return As<byte, T>(ref *(byte*)source);
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(ref byte source)
        {
            return As<byte, T>(ref source);
        }

		/// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(void* destination, T value)
        {
            As<byte, T>(ref *(byte*)destination) = value;
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(ref byte destination, T value)
        {
            As<byte, T>(ref destination) = value;
        }

        /// <summary>
        /// Returns if a given by-ref to type <typeparamref name="T"/> is a null reference.
        /// </summary>
        /// <remarks>
        /// This check is conceptually similar to "(void*)(&amp;source) == nullptr".
        /// </remarks>
        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullRef<T>(ref T source)
        {
            return Unsafe.AsPointer(ref source) == null;
 
            // ldarg.0
            // ldc.i4.0
            // conv.u
            // ceq
            // ret
        }

		//[Intrinsic]
		//[NonVersionable]
		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static ref T AsRef<T>(void* source)
		//{
		//	return ref As<byte, T>(ref *(byte*)source);
		//}
	}
}
