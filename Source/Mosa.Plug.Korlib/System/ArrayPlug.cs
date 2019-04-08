// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Plug;
using System;
using System.Reflection;

namespace Mosa.Plug.Korlib.System
{
	// TODO: Implement properly for SZ arrays and multi dimensional arrays
	public static class ArrayPlug
	{
		[Plug("System.Array::Copy")]
		internal static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length, bool reliable)
		{
			var sourceArrayPtr = Intrinsic.GetObjectAddress<Array>(sourceArray);
			var destinationArrayPtr = Intrinsic.GetObjectAddress<Array>(destinationArray);

			// TODO: add more checks, allow type up-casting, add multi dimensional array support
			if (sourceArrayPtr == null)
				throw new ArgumentNullException(nameof(sourceArrayPtr));

			if (destinationArrayPtr == null)
				throw new ArgumentNullException(nameof(destinationArrayPtr));

			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length));

			var sourceLength = GetLength(sourceArrayPtr, 0);
			var destinationLength = GetLength(destinationArrayPtr, 0);

			if (sourceLength - sourceIndex < length)
				throw new ArgumentOutOfRangeException(nameof(length));

			if (destinationLength - destinationIndex < length)
				throw new ArgumentOutOfRangeException(nameof(length));

			// Get type info
			var typeStruct = new TypeDefinition(sourceArrayPtr);
			var typeCode = typeStruct.TypeCode;

			var size = (typeCode == TypeCode.ReferenceType) ? IntPtr.Size : (int)typeStruct.Size;

			Mosa.Runtime.Internal.MemoryCopy(
				destinationArrayPtr + (IntPtr.Size * 2) + (destinationIndex * size),
				sourceArrayPtr + (IntPtr.Size * 2) + (sourceIndex * size),
				(uint)(length * size)
			);
		}

		[Plug("System.Array::GetLength")]
		internal static int GetLength(IntPtr array, int dimension)
		{
			return (int)Intrinsic.Load32(array, IntPtr.Size * 2);
		}

		[Plug("System.Array::GetLowerBound")]
		internal static int GetLowerBound(IntPtr o, int dimension)
		{
			return 0; // TODO
		}
	}
}
