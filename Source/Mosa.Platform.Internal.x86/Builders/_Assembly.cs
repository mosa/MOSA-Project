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
using Mosa.Platform.Internal.x86;
using x86Runtime = Mosa.Platform.Internal.x86.Runtime;

namespace System
{
	public sealed unsafe class _Assembly : Assembly
	{
		internal MetadataAssemblyStruct* assemblyStruct;
		internal Type[] types;
		internal RuntimeTypeHandle[] handles;

		internal _Assembly(uint* pointer)
		{
			this.assemblyStruct = (MetadataAssemblyStruct*)pointer;
			this.handles = new RuntimeTypeHandle[(*this.assemblyStruct).NumberOfTypes];
			this.types = new Type[(*this.assemblyStruct).NumberOfTypes];
			for (uint i = 0; i < this.types.Length; i++)
			{
				fixed (RuntimeTypeHandle* handle = &this.handles[i])
				{
					((uint*)handle)[0] = ((uint)this.assemblyStruct) + MetadataAssemblyStruct.TypesOffset + i;
				}
				this.handles[i] = this.handles[i];
				this.types[i] = new _Type(this.handles[i], this);
			}
		}
	}
}