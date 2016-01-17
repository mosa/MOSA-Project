// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Reflection;
using Mosa.Internal;

namespace System
{
	public sealed unsafe class RuntimeAssembly : Assembly
	{
		internal MetadataAssemblyStruct* assemblyStruct;
		internal LinkedList<RuntimeType> typeList = new LinkedList<RuntimeType>();
		internal LinkedList<RuntimeTypeHandle> typeHandles = new LinkedList<RuntimeTypeHandle>();
		internal LinkedList<RuntimeTypeInfo> typeInfoList = null;
		internal LinkedList<CustomAttributeData> customAttributesData = null;

		private string fullName;

		public override IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				if (customAttributesData == null)
				{
					// Custom Attributes Data - Lazy load
					customAttributesData = new LinkedList<CustomAttributeData>();
					if (assemblyStruct->CustomAttributes != null)
					{
						var customAttributesTablePtr = assemblyStruct->CustomAttributes;
						var customAttributesCount = customAttributesTablePtr[0];
						customAttributesTablePtr++;
						for (uint i = 0; i < customAttributesCount; i++)
						{
							var cad = new RuntimeCustomAttributeData((MetadataCAStruct*)customAttributesTablePtr[i]);
							customAttributesData.AddLast(cad);
						}
					}
				}

				return customAttributesData;
			}
		}

		public override IEnumerable<TypeInfo> DefinedTypes
		{
			get
			{
				if (typeInfoList == null)
				{
					// Type Info - Lazy load
					typeInfoList = new LinkedList<RuntimeTypeInfo>();
					foreach (RuntimeType type in typeList)
						typeInfoList.AddLast(new RuntimeTypeInfo(type, this));
				}

				var types = new LinkedList<TypeInfo>();
				foreach (var type in typeInfoList)
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
				var types = new LinkedList<Type>();
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
			assemblyStruct = (MetadataAssemblyStruct*)pointer;
			fullName = Mosa.Internal.Runtime.InitializeMetadataString(assemblyStruct->Name);

			uint typeCount = (*assemblyStruct).NumberOfTypes;
			for (uint i = 0; i < typeCount; i++)
			{
				var handle = new RuntimeTypeHandle();
				((uint**)&handle)[0] = (uint*)MetadataAssemblyStruct.GetTypeDefinitionAddress(assemblyStruct, i);

				if (typeHandles.Contains(handle))
					continue;

				ProcessType(handle);
			}
		}

		internal RuntimeType ProcessType(RuntimeTypeHandle handle)
		{
			typeHandles.AddLast(handle);
			var type = new RuntimeType(handle);
			typeList.AddLast(type);
			return type;
		}
	}
}
