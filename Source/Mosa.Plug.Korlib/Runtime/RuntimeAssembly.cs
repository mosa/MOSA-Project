// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mosa.Plug.Korlib.Runtime
{
	public sealed unsafe class RuntimeAssembly : Assembly
	{
		internal readonly List<RuntimeType> typeList;
		internal AssemblyDefinition assemblyDefinition;
		internal readonly List<RuntimeTypeHandle> typeHandles;
		internal List<RuntimeTypeInfo> typeInfoList = null;
		internal List<CustomAttributeData> customAttributesData = null;

		private readonly string fullName;

		public override IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				if (customAttributesData == null)
				{
					// Custom Attributes Data - Lazy load
					if (!assemblyDefinition.CustomAttributes.IsNull)
					{
						var customAttributesTablePtr = assemblyDefinition.CustomAttributes;
						var customAttributesCount = customAttributesTablePtr.NumberOfAttributes;
						customAttributesData = new List<CustomAttributeData>();
						for (uint i = 0; i < customAttributesCount; i++)
						{
							var cad = new RuntimeCustomAttributeData(customAttributesTablePtr.GetCustomAttribute(i));
							customAttributesData.Add(cad);
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
					typeInfoList = new List<RuntimeTypeInfo>();
					foreach (var type in typeList)
					{
						typeInfoList.Add(new RuntimeTypeInfo(type, this));
					}
				}

				var types = new List<TypeInfo>();

				foreach (var type in typeInfoList)
					types.Add(type);

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
				var list = new List<RuntimeType>();
				foreach (var type in typeList)
				{
					if ((type.attributes & TypeAttributes.VisibilityMask) != TypeAttributes.Public)
						continue;
					list.Add(type);
				}
				return list;
			}
		}

		internal RuntimeAssembly(IntPtr pointer)
		{
			assemblyDefinition = new AssemblyDefinition(new Pointer(pointer));
			fullName = assemblyDefinition.Name;

			typeList = new List<RuntimeType>();
			typeHandles = new List<RuntimeTypeHandle>();

			uint typeCount = assemblyDefinition.NumberOfTypes;

			for (uint i = 0; i < typeCount; i++)
			{
				var handle = new RuntimeTypeHandle(assemblyDefinition.GetTypeDefinition(i).Ptr.ToIntPtr());

				if (typeHandles.Contains(handle))
					continue;

				ProcessType(handle);
			}
		}

		internal RuntimeType ProcessType(RuntimeTypeHandle handle)
		{
			typeHandles.Add(handle);
			var type = new RuntimeType(handle);
			typeList.Add(type);
			return type;
		}
	}
}
