// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.MosaTypeSystem.CLR;

public class ClrTypeResolver : ITypeResolver
{
	private readonly Dictionary<Tuple<MosaModule, string>, MosaType?> typeLookup;

	public ClrTypeResolver()
	{
		typeLookup = new();
	}

	public void AddType(Tuple<MosaModule, string> key, MosaType? value)
	{
		typeLookup[key] = value;
	}

	public MosaType ResolveType(MosaModule module, BuiltInType type)
	{
		var typeName = GetTypeName(type);
		var resolvedType = GetTypeByName(module, typeName);

		if (resolvedType == null)
			throw new InvalidOperationCompilerException($"Type {typeName} is null or does not exist!");

		return resolvedType;
	}

	public MosaType ResolveType(MosaModule module, MosaTypeCode type)
	{
		var typeName = GetTypeName(type);
		var resolvedType = GetTypeByName(module, typeName);

		if (resolvedType == null)
			throw new InvalidOperationCompilerException($"Type {typeName} is null or does not exist!");

		return resolvedType;
	}

	public MosaType? GetTypeByName(IList<MosaModule> modules, string fullName)
	{
		foreach (var module in modules)
		{
			var result = GetTypeByName(module, fullName);
			if (result != null)
				return result;
		}

		return null;
	}

	public MosaType? GetTypeByName(MosaModule module, string fullName)
	{
		return typeLookup.TryGetValue(Tuple.Create(module, fullName), out var result) ? result : null;
	}

	private static string GetTypeName(BuiltInType type)
	{
		return type switch
		{
			BuiltInType.Void => "System.Void",
			BuiltInType.Boolean => "System.Boolean",
			BuiltInType.Char => "System.Char",
			BuiltInType.SByte => "System.SByte",
			BuiltInType.Byte => "System.Byte",
			BuiltInType.Int16 => "System.Int16",
			BuiltInType.UInt16 => "System.UInt16",
			BuiltInType.Int32 => "System.Int32",
			BuiltInType.UInt32 => "System.UInt32",
			BuiltInType.Int64 => "System.Int64",
			BuiltInType.UInt64 => "System.UInt64",
			BuiltInType.Single => "System.Single",
			BuiltInType.Double => "System.Double",
			BuiltInType.String => "System.String",
			BuiltInType.Object => "System.Object",
			BuiltInType.IntPtr => "System.IntPtr",
			BuiltInType.UIntPtr => "System.UIntPtr",
			BuiltInType.TypedReference => "System.TypedReference",
			BuiltInType.ValueType => "System.ValueType",
			_ => throw new CompilerException("Searching for invalid built-in type.")
		};
	}

	private static string GetTypeName(MosaTypeCode type)
	{
		return type switch
		{
			MosaTypeCode.Void => "System.Void",
			MosaTypeCode.Boolean => "System.Boolean",
			MosaTypeCode.Char => "System.Char",
			MosaTypeCode.I1 => "System.SByte",
			MosaTypeCode.U1 => "System.Byte",
			MosaTypeCode.I2 => "System.Int16",
			MosaTypeCode.U2 => "System.UInt16",
			MosaTypeCode.I4 => "System.Int32",
			MosaTypeCode.U4 => "System.UInt32",
			MosaTypeCode.I8 => "System.Int64",
			MosaTypeCode.U8 => "System.UInt64",
			MosaTypeCode.R4 => "System.Single",
			MosaTypeCode.R8 => "System.Double",
			MosaTypeCode.String => "System.String",
			MosaTypeCode.Object => "System.Object",
			MosaTypeCode.I => "System.IntPtr",
			MosaTypeCode.U => "System.UIntPtr",
			MosaTypeCode.TypedRef => "System.TypedReference",
			MosaTypeCode.ValueType => "System.ValueType",
			MosaTypeCode.Array => "System.Array",
			MosaTypeCode.SZArray => "System.Array",
			_ => throw new CompilerException("Searching for invalid type code.")
		};
	}
}
