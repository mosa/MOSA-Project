/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class InternalsForRuntimeHelpers
	{
		public static int GetHashCode(void* obj)
		{
			// For now use the obj location in memory as the hash code
			return (int)obj;
		}

		public new static bool Equals(Object o1, Object o2)
		{
			// For now just compare the object locations
			// This will become more sophisticated when we introduce remote objects in the distant future
			return (o1 == o2);
		}

		public static void InitializeArray(uint* array, RuntimeFieldHandle handle)
		{
			uint* fieldDefinition = ((uint**)&handle)[0];
			byte* arrayElements = (byte*)(array + 3);

			// See FieldDefinition for format of field handle
			byte* fieldData = (byte*)*(fieldDefinition + 4);
			uint dataLength = *(fieldDefinition + 5);
			while (dataLength > 0)
			{
				*arrayElements = *fieldData;
				arrayElements++;
				fieldData++;
				dataLength--;
			}
		}
	}
}