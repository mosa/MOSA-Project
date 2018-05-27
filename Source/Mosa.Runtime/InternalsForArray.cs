// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Metadata;
using System;
using System.Reflection;

namespace Mosa.Runtime
{
	// TODO: Implement properly for SZ arrays and multi dimensional arrays
	public unsafe static class InternalsForArray
	{
		private static void Copy(UIntPtr sourceArray, int sourceIndex, UIntPtr destinationArray, int destinationIndex, int length, bool reliable)
		{
			// TODO: add more checks, allow type upcasting, add multi dimensional array support
			if (sourceArray == null)
				throw new ArgumentNullException(nameof(sourceArray));
			if (destinationArray == null)
				throw new ArgumentNullException(nameof(destinationArray));

			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length));

			var sourceLength = GetLength(sourceArray, 0);
			var destinationLength = GetLength(destinationArray, 0);

			if (sourceLength - sourceIndex < length)
				throw new ArgumentOutOfRangeException(nameof(length));

			if (destinationLength - destinationIndex < length)
				throw new ArgumentOutOfRangeException(nameof(length));

			// Get type info
			var typeStruct = (TypeDefinition*)((uint*)sourceArray);
			var typeCode = typeStruct->TypeCode;

			var size = (typeCode == TypeCode.ReferenceType) ? UIntPtr.Size : (int)typeStruct->Size;

			Internal.MemoryCopy(
				destinationArray + (UIntPtr.Size * 2) + (destinationIndex * size),
				sourceArray + (UIntPtr.Size * 2) + (sourceIndex * size),
				(uint)(length * size)
			);
		}

		public static int GetLength(UIntPtr o, int dimension)
		{
			return *(((int*)o) + 2);
		}

		public static int GetLowerBound(UIntPtr o, int dimension)
		{
			return 0; // TODO
		}
	}
}
