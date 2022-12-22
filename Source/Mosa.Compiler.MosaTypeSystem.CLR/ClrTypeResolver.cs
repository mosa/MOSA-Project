using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.MosaTypeSystem.CLR;

public class ClrTypeResolver : ITypeResolver
{
	public MosaType ResolveType(TypeSystem typeSystem, MosaModule module, BuiltInType type)
	{
		return type switch
		{
			BuiltInType.Void => typeSystem.GetTypeByName(module, "System", "Void"),
			BuiltInType.Boolean => typeSystem.GetTypeByName(module, "System", "Boolean"),
			BuiltInType.Char => typeSystem.GetTypeByName(module, "System", "Char"),
			BuiltInType.SByte => typeSystem.GetTypeByName(module, "System", "SByte"),
			BuiltInType.Byte => typeSystem.GetTypeByName(module, "System", "Byte"),
			BuiltInType.Int16 => typeSystem.GetTypeByName(module, "System", "Int16"),
			BuiltInType.UInt16 => typeSystem.GetTypeByName(module, "System", "UInt16"),
			BuiltInType.Int32 => typeSystem.GetTypeByName(module, "System", "Int32"),
			BuiltInType.UInt32 => typeSystem.GetTypeByName(module, "System", "UInt32"),
			BuiltInType.Int64 => typeSystem.GetTypeByName(module, "System", "Int64"),
			BuiltInType.UInt64 => typeSystem.GetTypeByName(module, "System", "UInt64"),
			BuiltInType.Single => typeSystem.GetTypeByName(module, "System", "Single"),
			BuiltInType.Double => typeSystem.GetTypeByName(module, "System", "Double"),
			BuiltInType.String => typeSystem.GetTypeByName(module, "System", "String"),
			BuiltInType.Object => typeSystem.GetTypeByName(module, "System", "Object"),
			BuiltInType.IntPtr => typeSystem.GetTypeByName(module, "System", "IntPtr"),
			BuiltInType.UIntPtr => typeSystem.GetTypeByName(module, "System", "UIntPtr"),
			BuiltInType.TypedReference => typeSystem.GetTypeByName(module, "System", "TypedReference"),
			BuiltInType.ValueType => typeSystem.GetTypeByName(module, "System", "ValueType"),
			_ => throw new CompilerException("Searching for invalid built-in type.")
		};
	}
}
