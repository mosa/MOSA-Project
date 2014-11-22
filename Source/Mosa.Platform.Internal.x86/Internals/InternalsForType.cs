/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class InternalsForType
	{
		public static Type GetTypeImpl(string typeName, bool throwOnError)
		{
			if (typeName == null)
				throw new ArgumentNullException("typeName");

			// Iterate through all the assemblies and look for the type name
			foreach (RuntimeAssembly assembly in Runtime.Assemblies)
			{
				foreach (RuntimeType type in assembly.typeList)
				{
					// Get type name for currently found type, if its not a match then skip
					if (type.FullName != typeName) continue;

					// If we get here then its a match so return it
					return type;
				}
			}

			// If we didn't find a match then throw error if throwOnError, otherwise return null
			if (throwOnError)
				throw new TypeLoadException();
			else
				return null;
		}

		public static Type GetTypeFromHandleImpl(RuntimeTypeHandle handle)
		{
			// Iterate through all the assemblies and look for the type handle
			foreach (RuntimeAssembly assembly in Runtime.Assemblies)
			{
				foreach (RuntimeType type in assembly.typeList)
				{
					// If its not a match then skip
					if (type.TypeHandle != handle)
						continue;

					// If we get here then its a match so return it
					return type;
				}
			}

			// If we didn't find a match then return null
			return null;
		}
	}
}