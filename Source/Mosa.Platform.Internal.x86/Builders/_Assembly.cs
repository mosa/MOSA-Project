/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Platform.Internal.x86;
using System.Reflection;

namespace System
{
	public sealed unsafe class _Assembly : Assembly
	{
		internal MetadataAssemblyStruct* assemblyStruct;
		internal Type[] types;
		internal RuntimeTypeHandle[] handles;

		public override Collections.Generic.IEnumerable<TypeInfo> DefinedTypes
		{
			get { throw new NotImplementedException(); }
		}

		internal _Assembly(uint* pointer)
		{
			this.assemblyStruct = (MetadataAssemblyStruct*)pointer;
			this.handles = new RuntimeTypeHandle[(*this.assemblyStruct).NumberOfTypes];
			this.types = new Type[(*this.assemblyStruct).NumberOfTypes];
			for (uint i = 0; i < this.types.Length; i++)
			{
				RuntimeTypeHandle handle = new RuntimeTypeHandle();
				((uint**)&handle)[0] = MetadataAssemblyStruct.GetTypeDefinitionAddress(assemblyStruct, i);
				this.handles[i] = handle;
				this.types[i] = new _Type(handle);
			}
		}
	}
}