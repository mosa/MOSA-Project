// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime;
using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.System
{
	// TODO: Implement properly for SZ arrays and multi dimensional arrays
	public static class ArrayPlug
	{
		// TODO: Fix!!!
		[Plug("System.Array::Copy")]
		internal static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length, bool reliable)
		{
			var sourceArrayPtr = Intrinsic.GetObjectAddress<Array>(sourceArray);
			var destinationArrayPtr = Intrinsic.GetObjectAddress<Array>(destinationArray);

			// TODO: add more checks, allow type up-casting, add multi dimensional array support
			if (sourceArrayPtr.IsNull)
				throw new ArgumentNullException(nameof(sourceArrayPtr));

			if (destinationArrayPtr.IsNull)
				throw new ArgumentNullException(nameof(destinationArrayPtr));

			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length));

			var sourceLength = GetLength(sourceArrayPtr.ToIntPtr(), 0);
			var destinationLength = GetLength(destinationArrayPtr.ToIntPtr(), 0);

			if (sourceLength - sourceIndex < length)
				throw new ArgumentOutOfRangeException(nameof(length));

			if (destinationLength - destinationIndex < length)
				throw new ArgumentOutOfRangeException(nameof(length));

			// Get type info
			// Broken! (Size property loads at an invalid address most likely)
			/*var typeStruct = new TypeDefinition(new Pointer(sourceArrayPtr.ToIntPtr()));
			var typeCode = typeStruct.TypeCode;

			var size = (typeCode == TypeCode.ReferenceType) ? IntPtr.Size : (int)typeStruct.Size;*/

			var size = IntPtr.Size;

			Mosa.Runtime.Internal.MemoryCopy(
				destinationArrayPtr + (IntPtr.Size * 2) + (destinationIndex * size),
				sourceArrayPtr + (IntPtr.Size * 2) + (sourceIndex * size),
				(uint)(length * size)
			);
		}

		[Plug("System.Array::Clear")]
		internal static void Clear(Array array, int index, int length)
		{
			var arrayPtr = Intrinsic.GetObjectAddress<Array>(array);

			// TODO: add more checks, allow type up-casting, add multi dimensional array support
			if (arrayPtr.IsNull)
				throw new ArgumentNullException(nameof(arrayPtr));

			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length));

			var sourceLength = GetLength(arrayPtr.ToIntPtr(), 0);

			if (sourceLength - index < length)
				throw new ArgumentOutOfRangeException(nameof(length));

			// Get type info
			// Broken! (Size property loads at an invalid address most likely)
			/*var typeStruct = new TypeDefinition(new Pointer(arrayPtr.ToIntPtr()));
			var typeCode = typeStruct.TypeCode;

			var size = (typeCode == TypeCode.ReferenceType) ? IntPtr.Size : (int)typeStruct.Size;*/

			var size = IntPtr.Size;

			Mosa.Runtime.Internal.MemoryClear(
				arrayPtr + (IntPtr.Size * 2) + (index * size),
				(uint)(length * size)
			);
		}

		[Plug("System.Array::GetLength")]
		internal static int GetLength(IntPtr array, int dimension)
		{
			return (int)(new Pointer(array)).Load32(IntPtr.Size * 2);
		}

		[Plug("System.Array::GetLowerBound")]
		internal static int GetLowerBound(IntPtr o, int dimension)
		{
			return 0; // TODO
		}

		[Plug("System.Array::IndexOf")]
		internal static int IndexOf(Array array, object value, int startIndex, int count)
		{
			return -1; // TODO
		}
	}
}
