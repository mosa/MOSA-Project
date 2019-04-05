// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Plug;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mosa.Plug.Korlib.System.Runtime.CompilerServices
{
	internal static class RuntimeHelpersPlug
	{
		[Plug("System.Runtime.CompilerServices.RuntimeHelpers::GetHashCode")]
		internal unsafe static int GetHashCode(Object o)
		{
			// For now use the obj location in memory as the hash code
			return Intrinsic.GetObjectAddress(o).ToInt32();
		}

		[Plug("System.Runtime.CompilerServices.RuntimeHelpers::Equals")]
		internal new static bool Equals(Object o1, Object o2)
		{
			// For now just compare the object locations
			// This will become more sophisticated when we introduce remote objects in the distant future
			return o1 == o2;
		}

		[Plug("System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray")]
		internal static void InitializeArray(Array array, RuntimeFieldHandle fldHandle)
		{
			var arrayAddress = Intrinsic.GetObjectAddress(array);
			var fieldDefinition = new FieldDefinition(fldHandle.Value);

			var arrayElements = arrayAddress + (IntPtr.Size * 3);
			var fieldData = fieldDefinition.FieldData;
			uint dataLength = fieldDefinition.OffsetOrSize;

			Mosa.Runtime.Internal.MemoryCopy(arrayElements, fieldData, dataLength);
		}

		[Plug("System.Runtime.CompilerServices.RuntimeHelpers::UnsafeCast")]
		internal unsafe static Object UnsafeCast(Object o)
		{
			return o;
		}

		[Plug("System.Runtime.CompilerServices.RuntimeHelpers::GetAssemblies")]
		internal static IEnumerable<Assembly> GetAssemblies()
		{
			var assemblies = new List<Assembly>();

			foreach (var assembly in Internal.Assemblies)
			{
				assemblies.Add(assembly);
			}

			return assemblies;
		}

		[Plug("System.Runtime.CompilerServices.RuntimeHelpers::CreateInstance")]
		internal static object CreateInstance(Type type, params object[] args)
		{
			var typeDefinition = new TypeDefinition(type.TypeHandle.Value);

			if (typeDefinition.DefaultConstructor.IsNull)
				throw new ArgumentException("Type has no parameterless constructor.");

			var thisObject = Mosa.Runtime.Internal.AllocateObject(type.TypeHandle, typeDefinition.Size);

			return Intrinsic.CreateInstanceSimple(typeDefinition.DefaultConstructor.Method, thisObject);
		}
	}
}
