// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime
{
	public static class InternalsForObject
	{
		public static Type GetType(object obj)
		{
			// Get the handle of the object
			//var handle = GetTypeHandle2(obj);
			var o = Intrinsic.GetObjectAddress(obj);
			var handle = new RuntimeTypeHandle(Intrinsic.LoadPointer(o));

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
