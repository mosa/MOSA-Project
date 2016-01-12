// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime
{
	public unsafe static class InternalsForType
	{
		public static Type GetTypeImpl(string typeName, bool throwOnError)
		{
			if (typeName == null)
				throw new ArgumentNullException("typeName");

			// Iterate through all the assemblies and look for the type name
			foreach (var assembly in Internal.Assemblies)
			{
				foreach (var type in assembly.typeList)
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
			foreach (var assembly in Internal.Assemblies)
			{
				foreach (var type in assembly.typeList)
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
