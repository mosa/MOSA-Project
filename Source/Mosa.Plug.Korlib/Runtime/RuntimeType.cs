// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using System;
using System.Reflection;

namespace Mosa.Plug.Korlib.Runtime
{
	public sealed unsafe class RuntimeType : Type
	{
		private readonly TypeDefinition typeDefinition;
		private readonly string assemblyQualifiedName;
		private readonly string name;
		private readonly string @namespace;
		private readonly string fullname;
		private readonly RuntimeTypeHandle handle;
		private readonly TypeCode typeCode;
		internal TypeAttributes attributes; // FIXME: this should be private, only temporarily internal
		private readonly RuntimeTypeHandle declaringTypeHandle;
		private readonly RuntimeTypeHandle elementTypeHandle;
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
				{
					// Declaring Type - Lazy load
					declaringType = Type.GetTypeFromHandle(declaringTypeHandle);
				}

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
			typeDefinition = new TypeDefinition(new Pointer(handle.Value));

			assemblyQualifiedName = typeDefinition.Name;   // TODO
			name = typeDefinition.Name;                    // TODO
			@namespace = typeDefinition.Name;              // TODO
			fullname = typeDefinition.Name;

			typeCode = typeDefinition.TypeCode;
			attributes = typeDefinition.Attributes;

			// Declaring Type
			if (!typeDefinition.DeclaringType.IsNull)
			{
				declaringTypeHandle = new RuntimeTypeHandle(typeDefinition.DeclaringType.Ptr.ToIntPtr());
			}

			// Element Type
			if (!typeDefinition.ElementType.IsNull)
			{
				elementTypeHandle = new RuntimeTypeHandle(typeDefinition.ElementType.Ptr.ToIntPtr());
			}
		}

		public override int GetArrayRank()
		{
			// We don't know so just return 1 if array, 0 otherwise
			return (IsArrayImpl()) ? 1 : 0;
		}

		public override Type GetElementType()
		{
			if (elementType == null)
			{
				// Element Type - Lazy load
				elementType = Type.GetTypeFromHandle(elementTypeHandle);
			}

			return elementType;
		}

		public override Type GetGenericTypeDefinition()
		{
			// No planned support
			throw new NotSupportedException();
		}

		protected override bool HasElementTypeImpl()
		{
			return elementTypeHandle.Value != IntPtr.Zero;
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
