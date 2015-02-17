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
	public sealed unsafe class RuntimeTypeInfo : TypeInfo
	{
		private MetadataTypeStruct* typeStruct;
		private Assembly assembly;

		//private RuntimeTypeHandle handle;
		private string assemblyQualifiedName;

		private string name;
		private string @namespace;
		private string fullname;
		private TypeCode typeCode;
		private TypeAttributes attributes;
		private Type baseType;
		private Type declaringType;
		private Type elementType;
		private Type asType;
		private LinkedList<CustomAttributeData> customAttributesData = null;

		internal readonly Type ValueType = typeof(ValueType);
		internal readonly Type EnumType = typeof(Enum);

		public override string AssemblyQualifiedName
		{
			get { return this.assemblyQualifiedName; }
		}

		public override Assembly Assembly
		{
			get { return this.assembly; }
		}

		public override TypeAttributes Attributes
		{
			get { return this.attributes; }
		}

		public override Type BaseType
		{
			get { return (this.IsInterface) ? null : this.baseType; }
		}

		public override bool ContainsGenericParameters
		{
			get { throw new NotImplementedException(); }
		}

		public override IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				if (this.customAttributesData == null)
				{
					// Custom Attributes Data - Lazy load
					this.customAttributesData = new LinkedList<CustomAttributeData>();
					if (this.typeStruct->CustomAttributes != null)
					{
						var customAttributesTablePtr = this.typeStruct->CustomAttributes;
						var customAttributesCount = customAttributesTablePtr[0];
						customAttributesTablePtr++;
						for (uint i = 0; i < customAttributesCount; i++)
						{
							RuntimeCustomAttributeData cad = new RuntimeCustomAttributeData((MetadataCAStruct*)customAttributesTablePtr[i]);
							this.customAttributesData.AddLast(cad);
						}
					}
				}

				return this.customAttributesData;
			}
		}

		public override MethodBase DeclaringMethod
		{
			get { throw new NotImplementedException(); }
		}

		public override Type DeclaringType
		{
			get { return this.declaringType; }
		}

		public override string FullName
		{
			get { return this.fullname; }
		}

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
			get { return this.BaseType == EnumType; }
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

		public override string Name
		{
			get { return this.name; }
		}

		public override string Namespace
		{
			get { return this.@namespace; }
		}

		public RuntimeTypeInfo(RuntimeType type, Assembly assembly)
		{
			var handle = type.TypeHandle;
			this.asType = type;
			this.assembly = assembly;
			//this.handle = handle;
			this.typeStruct = (MetadataTypeStruct*)((uint**)&handle)[0];

			this.assemblyQualifiedName = x86Runtime.InitializeMetadataString(this.typeStruct->Name);	// TODO
			this.name = x86Runtime.InitializeMetadataString(this.typeStruct->Name);					// TODO
			this.@namespace = x86Runtime.InitializeMetadataString(this.typeStruct->Name);				// TODO
			this.fullname = x86Runtime.InitializeMetadataString(this.typeStruct->Name);

			this.typeCode = (TypeCode)(this.typeStruct->Attributes >> 24);
			this.attributes = (TypeAttributes)(this.typeStruct->Attributes & 0x00FFFFFF);

			// Base Type
			if (this.typeStruct->ParentType != null)
			{
				RuntimeTypeHandle parentHandle = new RuntimeTypeHandle();
				((uint**)&parentHandle)[0] = (uint*)this.typeStruct->ParentType;
				this.baseType = Type.GetTypeFromHandle(parentHandle);
			}

			// Declaring Type
			if (this.typeStruct->DeclaringType != null)
			{
				RuntimeTypeHandle declaringHandle = new RuntimeTypeHandle();
				((uint**)&declaringHandle)[0] = (uint*)this.typeStruct->DeclaringType;
				this.declaringType = Type.GetTypeFromHandle(declaringHandle);
			}

			// Element Type
			if (this.typeStruct->ElementType != null)
			{
				RuntimeTypeHandle elementHandle = new RuntimeTypeHandle();
				((uint**)&elementHandle)[0] = (uint*)this.typeStruct->ElementType;
				this.elementType = Type.GetTypeFromHandle(elementHandle);
			}
		}

		public override Type AsType()
		{
			return this.asType;
		}

		public override int GetArrayRank()
		{
			// We don't know so just return 1 if array, 0 otherwise
			return (this.IsArrayImpl() == true) ? 1 : 0;
		}

		public override Type GetElementType()
		{
			return this.elementType;
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
			return (this.elementType != null);
		}

		protected override bool IsArrayImpl()
		{
			return this.typeCode == TypeCode.Array || this.typeCode == TypeCode.SZArray;
		}

		protected override bool IsByRefImpl()
		{
			// We don't know so just return false
			return false;
		}

		protected override bool IsNestedImpl()
		{
			return (this.attributes & TypeAttributes.VisibilityMask) > TypeAttributes.Public;
		}

		protected override bool IsPointerImpl()
		{
			return this.typeCode == TypeCode.ManagedPointer || this.typeCode == TypeCode.UnmanagedPointer;
		}

		protected override bool IsPrimitiveImpl()
		{
			return (this.typeCode == TypeCode.Boolean
				|| this.typeCode == TypeCode.Char
				|| (this.typeCode >= TypeCode.I && this.typeCode <= TypeCode.I8)
				|| (this.typeCode >= TypeCode.U && this.typeCode <= TypeCode.U8)
				|| this.typeCode == TypeCode.R4
				|| this.typeCode == TypeCode.R8);
		}

		protected override bool IsValueTypeImpl()
		{
			Type thisType = this.AsType();
			if (thisType == ValueType || thisType == EnumType)
				return false;

			return this.IsSubclassOf(ValueType);
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
