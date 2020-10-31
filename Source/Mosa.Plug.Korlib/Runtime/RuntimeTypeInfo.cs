// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mosa.Plug.Korlib.Runtime
{
	public sealed unsafe class RuntimeTypeInfo : TypeInfo
	{
		private readonly TypeDefinition typeDefinition;
		private readonly TypeCode typeCode;
		private readonly Type baseType;
		private readonly Type elementType;
		private readonly Type asType;
		private List<CustomAttributeData> customAttributesData = null;

		internal readonly Type ValueType = typeof(ValueType);
		internal readonly Type EnumType = typeof(Enum);

		public override string AssemblyQualifiedName { get; }

		public override Assembly Assembly { get; }

		public override TypeAttributes Attributes { get; }

		public override Type BaseType { get { return (IsInterface) ? null : baseType; } }

		public override bool ContainsGenericParameters => throw new NotImplementedException();

		public override IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				if (customAttributesData == null)
				{
					// Custom Attributes Data - Lazy load
					customAttributesData = new List<CustomAttributeData>();
					if (!typeDefinition.CustomAttributes.IsNull)
					{
						var customAttributesTable = typeDefinition.CustomAttributes;
						var customAttributesCount = customAttributesTable.NumberOfAttributes;
						for (uint i = 0; i < customAttributesCount; i++)
						{
							RuntimeCustomAttributeData cad = new RuntimeCustomAttributeData(customAttributesTable.GetCustomAttribute(i));
							customAttributesData.Add(cad);
						}
					}
				}

				return customAttributesData;
			}
		}

		public override MethodBase DeclaringMethod => throw new NotImplementedException();

		public override Type DeclaringType { get; }

		public override string FullName { get; }

		public override int GenericParameterPosition
		{
			get { throw new NotSupportedException(); }
		}

		public override Type[] GenericTypeArguments
		{
			get { return new Type[0]; }
		}

		public override bool IsEnum
		{
			get { return BaseType == EnumType; }
		}

		public override bool IsGenericParameter
		{
			// We don't know so just return false
			get { return false; }
		}

		public override bool IsGenericType
		{
			// We don't know so just return false
			get { return false; }
		}

		public override bool IsGenericTypeDefinition
		{
			// We don't know so just return false
			get { return false; }
		}

		public override bool IsSerializable
		{
			// We don't know so just return false
			get { return false; }
		}

		public override string Name { get; }

		public override string Namespace { get; }

		public RuntimeTypeInfo(RuntimeType type, Assembly assembly)
		{
			asType = type;
			Assembly = assembly;

			var handle = type.TypeHandle;

			typeDefinition = new TypeDefinition(new Pointer(handle.Value));

			AssemblyQualifiedName = typeDefinition.Name;   // TODO
			Name = typeDefinition.Name;                    // TODO
			Namespace = typeDefinition.Name;              // TODO
			FullName = typeDefinition.Name;

			typeCode = typeDefinition.TypeCode;
			Attributes = typeDefinition.Attributes;

			// Base Type
			if (!typeDefinition.ParentType.IsNull)
			{
				var parentHandle = new RuntimeTypeHandle(typeDefinition.ParentType.Ptr.ToIntPtr());

				baseType = Type.GetTypeFromHandle(parentHandle);
			}

			// Declaring Type
			if (!typeDefinition.DeclaringType.IsNull)
			{
				var declaringHandle = new RuntimeTypeHandle(typeDefinition.DeclaringType.Ptr.ToIntPtr());

				DeclaringType = Type.GetTypeFromHandle(declaringHandle);
			}

			// Element Type
			if (!typeDefinition.ElementType.IsNull)
			{
				var elementHandle = new RuntimeTypeHandle(typeDefinition.ElementType.Ptr.ToIntPtr());

				elementType = Type.GetTypeFromHandle(elementHandle);
			}
		}

		public override Type AsType()
		{
			return asType;
		}

		public override int GetArrayRank()
		{
			// We don't know so just return 1 if array, 0 otherwise
			return IsArrayImpl() ? 1 : 0;
		}

		public override Type GetElementType()
		{
			return elementType;
		}

		public override Type[] GetGenericParameterConstraints()
		{
			// No planned support
			throw new NotSupportedException();
		}

		public override Type GetGenericTypeDefinition()
		{
			// No planned support
			throw new NotSupportedException();
		}

		protected override bool HasElementTypeImpl()
		{
			return elementType != null;
		}

		protected override bool IsArrayImpl()
		{
			return typeCode == TypeCode.Array || typeCode == TypeCode.SZArray;
		}

		protected override bool IsByRefImpl()
		{
			// We don't know so just return false
			return false;
		}

		protected override bool IsNestedImpl()
		{
			return (Attributes & TypeAttributes.VisibilityMask) > TypeAttributes.Public;
		}

		protected override bool IsPointerImpl()
		{
			return typeCode == TypeCode.ManagedPointer || typeCode == TypeCode.UnmanagedPointer;
		}

		protected override bool IsPrimitiveImpl()
		{
			return typeCode == TypeCode.Boolean
				|| typeCode == TypeCode.Char
				|| (typeCode >= TypeCode.I && typeCode <= TypeCode.I8)
				|| (typeCode >= TypeCode.U && typeCode <= TypeCode.U8)
				|| typeCode == TypeCode.R4
				|| typeCode == TypeCode.R8;
		}

		protected override bool IsValueTypeImpl()
		{
			Type thisType = AsType();
			if (thisType == ValueType || thisType == EnumType)
				return false;

			return IsSubclassOf(ValueType);
		}

		public override Type MakeArrayType()
		{
			// No planned support
			throw new NotSupportedException();
		}

		public override Type MakeArrayType(int rank)
		{
			// No planned support
			throw new NotSupportedException();
		}

		public override Type MakeByRefType()
		{
			// No planned support
			throw new NotSupportedException();
		}

		public override Type MakeGenericType(params Type[] typeArguments)
		{
			// No planned support
			throw new NotSupportedException();
		}

		public override Type MakePointerType()
		{
			// No planned support
			throw new NotSupportedException();
		}
	}
}
