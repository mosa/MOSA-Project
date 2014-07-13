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
		public static RuntimeTypeHandle* GetHandleFromObject(void* obj)
		{
			// TypeDefinition is located at the beginning of object (i.e. *obj )
			RuntimeTypeHandle handle = new RuntimeTypeHandle();
			((uint*)&handle)[0] = ((uint*)obj)[0];
			return &handle;
		}

		public static string GetFullName(RuntimeTypeHandle* handle)
		{
			// Name pointer located at the beginning of the TypeDefinition
			MetadataTypeStruct* typeDefinition = (MetadataTypeStruct*)((uint*)handle)[0];
			return Runtime.InitializeMetadataString((*typeDefinition).Name);
		}

		public static TypeAttributes GetAttributes(RuntimeTypeHandle* handle)
		{
			// Type attributes located at 3rd position of TypeDefinition
			MetadataTypeStruct* typeDefinition = (MetadataTypeStruct*)((uint*)handle)[0];
			return (TypeAttributes)(*typeDefinition).Attributes;
		}
	}
}