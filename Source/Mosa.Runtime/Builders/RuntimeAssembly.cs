// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System.Collections.Generic;
using System.Reflection;

namespace System
{
	public sealed unsafe class RuntimeAssembly : Assembly
	{
		internal MDAssemblyDefinition* assemblyDefinition;
		internal readonly LinkedList<RuntimeType> typeList;
		internal readonly LinkedList<RuntimeTypeHandle> typeHandles;
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
					if (assemblyDefinition->CustomAttributes != null)
					{
						var customAttributesTablePtr = assemblyDefinition->CustomAttributes;
						var customAttributesCount = customAttributesTablePtr->NumberOfAttributes;
						customAttributesData = new LinkedList<CustomAttributeData>();
						for (uint i = 0; i < customAttributesCount; i++)
						{
							var cad = new RuntimeCustomAttributeData(customAttributesTablePtr->GetCustomAttribute(i));
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
				var list = new LinkedList<RuntimeType>();
				foreach (RuntimeType type in typeList)
				{
					if ((type.attributes & TypeAttributes.VisibilityMask) != TypeAttributes.Public)
						continue;
					list.AddLast(type);
				}
				return list;
			}
		}

		internal RuntimeAssembly(MDAssemblyDefinition* pointer)
		{
			assemblyDefinition = pointer;
			fullName = assemblyDefinition->Name;

			uint typeCount = assemblyDefinition->NumberOfTypes;

			typeList = new LinkedList<RuntimeType>();
			typeHandles = new LinkedList<RuntimeTypeHandle>();

			for (uint i = 0; i < typeCount; i++)
			{
				var handle = new RuntimeTypeHandle();
				((uint**)&handle)[0] = (uint*)assemblyDefinition->GetTypeDefinition(i);

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
