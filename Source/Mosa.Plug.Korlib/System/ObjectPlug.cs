// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;
using System;

namespace Mosa.Plug.Korlib.System
{
	// TODO: Implement properly for SZ arrays and multi dimensional arrays
	public static class ObjectPlug
	{
		[Plug("System.Object::MemberwiseClone")]
		internal static object MemberwiseClone(object obj)
		{
			return null;
		}

		[Plug("System.Object::GetType")]
		internal static Type GetType(object obj)
		{
			// Get the handle of the object
			var handle = GetTypeHandle(obj);

			// Iterate through all the assemblies and look for the type handle
			foreach (var assembly in Internal.Assemblies)
			{
				foreach (var type in assembly.typeList)
				{
					// If its not a match then skip
					if (!type.TypeHandle.Equals(handle))
						continue;

					// If we get here then its a match so return it
					return type;
				}
			}

			// If we didn't find a match then return null
			return null;
		}

		[Plug("System.Object::GetTypeHandle")]
		internal static RuntimeTypeHandle GetTypeHandle(object obj)
		{
			var o = Intrinsic.GetObjectAddress(obj);

			return new RuntimeTypeHandle(Intrinsic.LoadPointer(o));
		}
	}
}
