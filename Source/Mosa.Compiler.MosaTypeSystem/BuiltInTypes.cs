// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.MosaTypeSystem;

public class BuiltInTypes
{
	public MosaType Void { get; }

	public MosaType Boolean { get; }

	public MosaType Char { get; }

	public MosaType I1 { get; }

	public MosaType U1 { get; }

	public MosaType I2 { get; }

	public MosaType U2 { get; }

	public MosaType I4 { get; }

	public MosaType U4 { get; }

	public MosaType I8 { get; }

	public MosaType U8 { get; }

	public MosaType R4 { get; }

	public MosaType R8 { get; }

	public MosaType String { get; }

	public MosaType Object { get; }

	public MosaType I { get; }

	public MosaType U { get; }

	public MosaType TypedRef { get; }

	public MosaType Pointer { get; }

	public MosaType ManagedPointer { get; }

	public BuiltInTypes(ITypeResolver typeResolver, MosaModule corlib)
	{
		Void = typeResolver.ResolveType(corlib, BuiltInType.Void);
		Boolean = typeResolver.ResolveType(corlib, BuiltInType.Boolean);
		Char = typeResolver.ResolveType(corlib, BuiltInType.Char);
		I1 = typeResolver.ResolveType(corlib, BuiltInType.SByte);
		U1 = typeResolver.ResolveType(corlib, BuiltInType.Byte);
		I2 = typeResolver.ResolveType(corlib, BuiltInType.Int16);
		U2 = typeResolver.ResolveType(corlib, BuiltInType.UInt16);
		I4 = typeResolver.ResolveType(corlib, BuiltInType.Int32);
		U4 = typeResolver.ResolveType(corlib, BuiltInType.UInt32);
		I8 = typeResolver.ResolveType(corlib, BuiltInType.Int64);
		U8 = typeResolver.ResolveType(corlib, BuiltInType.UInt64);
		R4 = typeResolver.ResolveType(corlib, BuiltInType.Single);
		R8 = typeResolver.ResolveType(corlib, BuiltInType.Double);
		String = typeResolver.ResolveType(corlib, BuiltInType.String);
		Object = typeResolver.ResolveType(corlib, BuiltInType.Object);
		I = typeResolver.ResolveType(corlib, BuiltInType.IntPtr);
		U = typeResolver.ResolveType(corlib, BuiltInType.UIntPtr);
		TypedRef = typeResolver.ResolveType(corlib, BuiltInType.TypedReference);

		Pointer = Void.ToUnmanagedPointer();
		ManagedPointer = Void.ToManagedPointer();
	}

	public MosaType GetType(BuiltInType builtInType)
	{
		return builtInType switch
		{
			BuiltInType.Void => Void,
			BuiltInType.Boolean => Boolean,
			BuiltInType.Char => Char,
			BuiltInType.SByte => I1,
			BuiltInType.Byte => U1,
			BuiltInType.Int16 => I2,
			BuiltInType.UInt16 => U2,
			BuiltInType.Int32 => I4,
			BuiltInType.UInt32 => U4,
			BuiltInType.Int64 => I8,
			BuiltInType.UInt64 => U8,
			BuiltInType.Single => R4,
			BuiltInType.Double => R8,
			BuiltInType.String => String,
			BuiltInType.Object => Object,
			BuiltInType.IntPtr => I,
			BuiltInType.UIntPtr => U,
			BuiltInType.TypedReference => TypedRef,
			_ => throw new CompilerException("Invalid BuildInType")
		};
	}
}
