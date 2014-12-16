/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Platform.Internal.x86;
using System.Collections.Generic;
using System.Reflection;
using x86Runtime = Mosa.Platform.Internal.x86.Runtime;

namespace System
{
	public sealed unsafe class RuntimeAssembly : Assembly
	{
		internal MetadataAssemblyStruct* assemblyStruct;
		internal LinkedList<RuntimeType> typeList = new LinkedList<RuntimeType>();
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

		public override IEnumerable<Type> ExportedTypes
		{
			get
			{
				LinkedList<Type> types = new LinkedList<Type>();
				foreach (RuntimeType type in typeList)
				{
					if ((type.attributes & TypeAttributes.VisibilityMask) != TypeAttributes.Public)
						continue;
					types.AddLast(type);
				}
				return types;
			}
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
			this.typeHandles.AddLast(handle);
			var type = new RuntimeType(handle);
			this.typeList.AddLast(type);
			return type;
		}

		internal void Phase2()
		{
			foreach (RuntimeType type in typeList)
			{
				type.FindRelativeTypes();
			}
		}
	}
}