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
	public unsafe static class TypeImpl
	{
		public static Type GetTypeImpl(string typeName, bool throwOnError, bool ignoreCase)
		{
			if (typeName == null)
				throw new ArgumentNullException("typeName");

			// If we are ignoring casing then lower casing
			if (ignoreCase)
				typeName = typeName.ToLower();

			// Iterate through all the assemblies and look for the type name
			for (int i = 0; i < Runtime.Assemblies.Length; i++)
			{
				for (int j = 0; j < Runtime.Assemblies[i].types.Length; j++)
				{
					// Get type name for currently found type, lower casing it if we are ignoring casing
					string foundTypeName = (ignoreCase) ? Runtime.Assemblies[i].types[j].FullName.ToLower() : Runtime.Assemblies[i].types[j].FullName;

					// If its not a match then skip
					if (foundTypeName != typeName) continue;

					// If we get here then its a match so return it
					return Runtime.Assemblies[i].types[j];
				}
			}

			// If we didn't find a match then throw error if throwOnError, otherwise return null
			if (throwOnError)
				throw new TypeLoadException();
			else
				return null;
		}

		public static RuntimeTypeHandle GetTypeHandleImpl(void* obj)
		{
			// TypeDefinition is located at the beginning of object (i.e. *obj )
			RuntimeTypeHandle handle = new RuntimeTypeHandle();
			((uint*)&handle)[0] = ((uint*)obj)[0];
			return handle;
		}

		public static Type GetTypeFromHandleImpl(RuntimeTypeHandle handle)
		{
			// Iterate through all the assemblies and look for the type handle
			for (int i = 0; i < Runtime.Assemblies.Length; i++)
			{
				for (int j = 0; j < Runtime.Assemblies[i].types.Length; j++)
				{
					// If its not a match then skip
					if (Runtime.Assemblies[i].types[j].TypeHandle != handle)
						continue;

					// If we get here then its a match so return it
					return Runtime.Assemblies[i].types[j];
				}
			}

			// If we didn't find a match then return null
			return null;
		}
	}
}