// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System.Reflection;

namespace System
{
	public sealed unsafe class RuntimeType : Type
	{
		private MDTypeDefinition* typeDefinition;
		private string assemblyQualifiedName;
		private string name;
		private string @namespace;
		private string fullname;
		private RuntimeTypeHandle handle;
		private TypeCode typeCode;
		internal TypeAttributes attributes; // FIXME: this should be private, only temporarily internal
		private RuntimeTypeHandle declaringTypeHandle;
		private RuntimeTypeHandle elementTypeHandle;
		private Type declaringType = null;
		private Type elementType = null;

		public override string AssemblyQualifiedName
		{
			get { return assemblyQualifiedName; }
		}

		public override Type DeclaringType
		{
			get
			{
				if (declaringType == null)

					// Declaring Type - Lazy load
					declaringType = Type.GetTypeFromHandle(declaringTypeHandle);

				return declaringType;
			}
		}

		public override string FullName
		{
			get { return fullname; }
		}

		public override int GenericParameterPosition
		{
			get { throw new NotSupportedException(); }
		}

		public override Type[] GenericTypeArguments
		{
			get { return new Type[0]; }
		}

		public override bool IsConstructedGenericType
		{
			// We don't know so just return false
			get { return false; }
		}

		public override bool IsGenericParameter
		{
			// We don't know so just return false
			get { return false; }
		}

		public override string Name
		{
			get { return name; }
		}

		public override string Namespace
		{
			get { return @namespace; }
		}

		public override RuntimeTypeHandle TypeHandle
		{
			get { return handle; }
		}

		internal RuntimeType(RuntimeTypeHandle handle)
		{
			this.handle = handle;
			typeDefinition = (MDTypeDefinition*)((uint**)&handle)[0];

			assemblyQualifiedName = typeDefinition->Name;   // TODO
			name = typeDefinition->Name;                    // TODO
			@namespace = typeDefinition->Name;              // TODO
			fullname = typeDefinition->Name;

			typeCode = typeDefinition->TypeCode;
			attributes = typeDefinition->Attributes;

			// Declaring Type
			if (typeDefinition->DeclaringType != null)
			{
				RuntimeTypeHandle declaringHandle = new RuntimeTypeHandle();
				((uint**)&declaringHandle)[0] = (uint*)typeDefinition->DeclaringType;
				declaringTypeHandle = declaringHandle;
			}

			// Element Type
			if ((*typeDefinition).ElementType != null)
			{
				RuntimeTypeHandle elementHandle = new RuntimeTypeHandle();
				((uint**)&elementHandle)[0] = (uint*)typeDefinition->ElementType;
				elementTypeHandle = elementHandle;
			}
		}

		public override int GetArrayRank()
		{
			// We don't know so just return 1 if array, 0 otherwise
			return (IsArrayImpl() == true) ? 1 : 0;
		}

		public override Type GetElementType()
		{
			if (elementType == null)

				// Element Type - Lazy load
				elementType = Type.GetTypeFromHandle(elementTypeHandle);
			return elementType;
		}

		public override Type GetGenericTypeDefinition()
		{
			// No planned support
			throw new NotSupportedException();
		}

		protected override bool HasElementTypeImpl()
		{
			return (elementTypeHandle.Value != IntPtr.Zero);
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
			return (attributes & TypeAttributes.VisibilityMask) > TypeAttributes.Public;
		}

		protected override bool IsPointerImpl()
		{
			return typeCode == TypeCode.ManagedPointer || typeCode == TypeCode.UnmanagedPointer;
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
