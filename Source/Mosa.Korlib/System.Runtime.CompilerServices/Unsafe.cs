// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.Versioning;

namespace System.Runtime.CompilerServices
{
	public static unsafe class Unsafe
	{
		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T As<T>(object value) where T : class
		{
			throw new PlatformNotSupportedException();
		}

		[Intrinsic]
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref TTo As<TFrom, TTo>(ref TFrom source)
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

		//[Intrinsic]
		//[NonVersionable]
		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static ref T AsRef<T>(void* source)
		//{
		//	return ref Unsafe.As<byte, T>(ref *(byte*)source);
		//}
	}
}
