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
		internal LinkedList<RuntimeTypeInfo> typeInfoList = new LinkedList<RuntimeTypeInfo>();
		internal LinkedList<CustomAttributeData> customAttributesData = new LinkedList<CustomAttributeData>();

		private string fullName;

		public override IEnumerable<CustomAttributeData> CustomAttributes
		{
			get { return customAttributesData; }
		}

		public override IEnumerable<TypeInfo> DefinedTypes
		{
			get
			{
				LinkedList<TypeInfo> types = new LinkedList<TypeInfo>();
				foreach (var type in this.typeInfoList)
					types.AddLast(type);
				return types;
			}
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
				foreach (RuntimeType type in this.typeList)
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
			this.fullName = x86Runtime.InitializeMetadataString(this.assemblyStruct->Name);

			uint typeCount = (*this.assemblyStruct).NumberOfTypes;
			for (uint i = 0; i < typeCount; i++)
			{
				RuntimeTypeHandle handle = new RuntimeTypeHandle();
				((uint**)&handle)[0] = (uint*)MetadataAssemblyStruct.GetTypeDefinitionAddress(assemblyStruct, i);

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
			foreach (RuntimeType type in this.typeList)
			{
				type.FindRelativeTypes();
			}

			// Get Custom Attributes Data (must be done after RuntimeTypes have been completely resolved!)
			if (this.assemblyStruct->CustomAttributes != null)
			{
				var customAttributesTablePtr = this.assemblyStruct->CustomAttributes;
				var customAttributesCount = customAttributesTablePtr[0];
				customAttributesTablePtr++;
				for (uint i = 0; i < customAttributesCount; i++)
				{
					RuntimeCustomAttributeData cad = new RuntimeCustomAttributeData((MetadataCAStruct*)customAttributesTablePtr[i]);
					customAttributesData.AddLast(cad);
				}
			}

			foreach (RuntimeType type in this.typeList)
			{
				this.typeInfoList.AddLast(new RuntimeTypeInfo(type, this));
			}
		}
	}
}