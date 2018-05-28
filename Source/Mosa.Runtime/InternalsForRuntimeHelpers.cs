// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mosa.Runtime
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

		public static void InitializeArray(IntPtr array, RuntimeFieldHandle handle)
		{
			var fieldDefinition = new FieldDefinition(handle.Value);

			var arrayElements = array + (IntPtr.Size * 3);
			var fieldData = fieldDefinition.FieldData;
			uint dataLength = fieldDefinition.OffsetOrSize;

			Internal.MemoryCopy(arrayElements, fieldData, dataLength);
		}

		public static void* UnsafeCast(void* o)
		{
			return o;
		}

		public static IEnumerable<Assembly> GetAssemblies()
		{
			var assemblies = new LinkedList<Assembly>();
			foreach (var assembly in Internal.Assemblies)
				assemblies.AddLast(assembly);
			return assemblies;
		}

		public static object CreateInstance(Type type, params object[] args)
		{
			var typeDefinition = new TypeDefinition(type.TypeHandle.Value);

			if (typeDefinition.DefaultConstructor.IsNull)
				throw new ArgumentException("Type has no parameterless constructor.");

			var thisObject = Internal.AllocateObject(type.TypeHandle, typeDefinition.Size);

			return Intrinsic.CreateInstanceSimple(typeDefinition.DefaultConstructor.Method, thisObject);
		}
	}
}
