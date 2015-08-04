// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
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

		public static void* UnsafeCast(void* o)
		{
			return o;
		}

		public static IEnumerable<Assembly> GetAssemblies()
		{
			LinkedList<Assembly> assemblies = new LinkedList<Assembly>();
			foreach (var assembly in Runtime.Assemblies)
				assemblies.AddLast(assembly);
			return assemblies;
		}

		public static object CreateInstance(Type type, params object[] args)
		{
			// Cheat
			var handle = type.TypeHandle;
			var typeStruct = (MetadataTypeStruct*)((uint**)&handle)[0];

			if (typeStruct->DefaultConstructor == null)
				throw new ArgumentException("Type has no parameterless constructor.");

			var thisObject = Runtime.AllocateObject(type.TypeHandle, typeStruct->Size);

			return Native.CreateInstanceSimple(typeStruct->DefaultConstructor->Method, thisObject);
		}
	}
}
