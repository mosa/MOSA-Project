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
using x86Runtime = Mosa.Platform.Internal.x86.Runtime;

namespace System
{
	public sealed unsafe class RuntimeAssembly : Assembly
	{
		internal MetadataAssemblyStruct* assemblyStruct;
		internal LinkedList<Type> typeList = new LinkedList<Type>();
		internal LinkedList<RuntimeTypeHandle> typeHandles = new LinkedList<RuntimeTypeHandle>();

		private string fullName;

		public override IEnumerable<TypeInfo> DefinedTypes
		{
			get { throw new NotImplementedException(); }
		}

		public override string FullName
		{
			get { return fullName; }
		}

		internal RuntimeAssembly(uint* pointer)
		{
			this.assemblyStruct = (MetadataAssemblyStruct*)pointer;
			this.fullName = x86Runtime.InitializeMetadataString((*this.assemblyStruct).Name);
			uint typeCount = (*this.assemblyStruct).NumberOfTypes;
			for (uint i = 0; i < typeCount; i++)
			{
				RuntimeTypeHandle handle = new RuntimeTypeHandle();
				((uint**)&handle)[0] = MetadataAssemblyStruct.GetTypeDefinitionAddress(assemblyStruct, i);

				if (this.typeHandles.Contains(handle))
					continue;

				this.ProcessType(handle);
			}
		}

		internal RuntimeType ProcessType(RuntimeTypeHandle handle)
		{
			this.typeHandles.Add(handle);
			var type = new RuntimeType(handle, this);
			this.typeList.Add(type);
			return type;
		}
	}
}