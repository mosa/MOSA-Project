﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Reflection;
using Mosa.Runtime;
using Mosa.Runtime.Metadata;

namespace Mosa.Plug.Korlib.Runtime;

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
	private Type declaringType;
	private Type elementType;

	public override string AssemblyQualifiedName => assemblyQualifiedName;

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

	public override string FullName => fullname;

	public override int GenericParameterPosition => throw new NotSupportedException();

	public override Type[] GenericTypeArguments => new Type[0];

	public override bool IsConstructedGenericType =>
		// We don't know so just return false
		false;

	public override bool IsGenericParameter =>
		// We don't know so just return false
		false;

	public override string Name => name;

	public override string Namespace => @namespace;

	public override RuntimeTypeHandle TypeHandle => handle;

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
		return IsArrayImpl() ? 1 : 0;
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
		return typeCode is TypeCode.Array or TypeCode.SZArray;
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
		return typeCode is TypeCode.ManagedPointer or TypeCode.UnmanagedPointer;
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
