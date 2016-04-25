// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System.Collections.Generic;
using System.Reflection;

namespace System
{
	public sealed unsafe class RuntimeAssembly : Assembly
	{
		internal MetadataAssemblyStruct* assemblyStruct;
		internal readonly List<RuntimeType> typeList;
		internal readonly LinkedList<RuntimeTypeHandle> typeHandles;
		internal List<RuntimeTypeInfo> typeInfoList = null;
		internal List<CustomAttributeData> customAttributesData = null;

		private string fullName;

		public override IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				if (customAttributesData == null)
				{
					// Custom Attributes Data - Lazy load
					if (assemblyStruct->CustomAttributes != null)
					{
						var customAttributesTablePtr = assemblyStruct->CustomAttributes;
						var customAttributesCount = customAttributesTablePtr[0];
						customAttributesData = new List<CustomAttributeData>((int)customAttributesCount);
						customAttributesTablePtr++;
						for (uint i = 0; i < customAttributesCount; i++)
						{
							var cad = new RuntimeCustomAttributeData((MetadataCAStruct*)customAttributesTablePtr[i]);
							customAttributesData.Add(cad);
						}
					}
					else
					{
						customAttributesData = new List<CustomAttributeData>();
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
					typeInfoList = new List<RuntimeTypeInfo>(typeList.Count);
					foreach (RuntimeType type in typeList)
						typeInfoList.Add(new RuntimeTypeInfo(type, this));
				}
				return typeInfoList;
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
				var list = new List<RuntimeType>();
				foreach (RuntimeType type in typeList)
				{
					if ((type.attributes & TypeAttributes.VisibilityMask) != TypeAttributes.Public)
						continue;
					list.Add(type);
				}
				return list;
			}
		}

		internal RuntimeAssembly(uint* pointer)
		{
			assemblyStruct = (MetadataAssemblyStruct*)pointer;
			fullName = Mosa.Runtime.Internal.InitializeMetadataString(assemblyStruct->Name);

			uint typeCount = (*assemblyStruct).NumberOfTypes;
			typeList = new List<RuntimeType>((int)typeCount);
			typeHandles = new LinkedList<RuntimeTypeHandle>();

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
			typeList.Add(type);
			return type;
		}
	}
}
