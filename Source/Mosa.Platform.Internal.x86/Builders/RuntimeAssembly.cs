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
using System.Collections.Generic;

namespace System
{
	public sealed unsafe class RuntimeAssembly : Assembly
	{
		internal MetadataAssemblyStruct* assemblyStruct;
		internal LinkedList<Type> typeList = new LinkedList<Type>();
		internal LinkedList<RuntimeTypeHandle> typeHandles = new LinkedList<RuntimeTypeHandle>();

		public override IEnumerable<TypeInfo> DefinedTypes
		{
			get { throw new NotImplementedException(); }
		}

		internal RuntimeAssembly(uint* pointer)
		{
			this.assemblyStruct = (MetadataAssemblyStruct*)pointer;
			uint typeCount = (*this.assemblyStruct).NumberOfTypes;
			for (uint i = 0; i < typeCount; i++)
			{
				RuntimeTypeHandle handle = new RuntimeTypeHandle();
				((uint**)&handle)[0] = MetadataAssemblyStruct.GetTypeDefinitionAddress(assemblyStruct, i);
				this.typeHandles.Add(handle);
				this.typeList.Add(new RuntimeType(handle));
			}
		}
	}
}