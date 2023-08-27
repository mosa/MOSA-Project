// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Reflection;
using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.System.Runtime.CompilerServices;

internal static class RuntimeHelpersPlug
{
	internal static int ObjectSequence = 0;

	[Plug("System.Runtime.CompilerServices.RuntimeHelpers::GetHashCode")]
	internal static unsafe int GetHashCode(object o)
	{
		return (int)Mosa.Runtime.Internal.GetObjectHashValue(o);
	}

	[Plug("System.Runtime.CompilerServices.RuntimeHelpers::Equals")]
	internal new static bool Equals(object o1, object o2)
	{
		// For now just compare the object locations
		return o1 == o2;
	}

	[Plug("System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray")]
	internal static void InitializeArray(Array array, RuntimeFieldHandle fldHandle)
	{
		var arrayAddress = Intrinsic.GetObjectAddress(array);
		var fieldDefinition = new FieldDefinition(new Pointer(fldHandle.Value));

		var arrayElements = arrayAddress + Pointer.Size;
		var fieldData = fieldDefinition.FieldData;
		var dataLength = fieldDefinition.Size;

		Mosa.Runtime.Internal.MemoryCopy(arrayElements, fieldData, dataLength);
	}

	[Plug("System.Runtime.CompilerServices.RuntimeHelpers::UnsafeCast")]
	internal static unsafe object UnsafeCast(object o)
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
		var typeDefinition = new TypeDefinition(new Pointer(type.TypeHandle.Value));

		if (typeDefinition.DefaultConstructor.IsNull)
			throw new ArgumentException("Type has no parameterless constructor.");

		var thisObject = Mosa.Runtime.Internal.AllocateObject(typeDefinition.Ptr, typeDefinition.Size);

		return Intrinsic.CreateInstanceSimple(typeDefinition.DefaultConstructor.Method, thisObject);
	}
}
