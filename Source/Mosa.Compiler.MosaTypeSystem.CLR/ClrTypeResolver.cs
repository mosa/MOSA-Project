using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.MosaTypeSystem.CLR
{
	public class ClrTypeResolver : ITypeResolver
	{
		private readonly Dictionary<Tuple<MosaModule?, string?>, MosaType?> typeLookup;

		public ClrTypeResolver()
		{
			typeLookup = new();
		}

		public void AddType(Tuple<MosaModule?, string?> key, MosaType? value)
		{
			typeLookup[key] = value;
		}

		public MosaType? ResolveType(MosaModule? module, BuiltInType type)
		{
			return type switch
			{
				BuiltInType.Void => GetTypeByName(module, "System.Void"),
				BuiltInType.Boolean => GetTypeByName(module, "System.Boolean"),
				BuiltInType.Char => GetTypeByName(module, "System.Char"),
				BuiltInType.SByte => GetTypeByName(module, "System.SByte"),
				BuiltInType.Byte => GetTypeByName(module, "System.Byte"),
				BuiltInType.Int16 => GetTypeByName(module, "System.Int16"),
				BuiltInType.UInt16 => GetTypeByName(module, "System.UInt16"),
				BuiltInType.Int32 => GetTypeByName(module, "System.Int32"),
				BuiltInType.UInt32 => GetTypeByName(module, "System.UInt32"),
				BuiltInType.Int64 => GetTypeByName(module, "System.Int64"),
				BuiltInType.UInt64 => GetTypeByName(module, "System.UInt64"),
				BuiltInType.Single => GetTypeByName(module, "System.Single"),
				BuiltInType.Double => GetTypeByName(module, "System.Double"),
				BuiltInType.String => GetTypeByName(module, "System.String"),
				BuiltInType.Object => GetTypeByName(module, "System.Object"),
				BuiltInType.IntPtr => GetTypeByName(module, "System.IntPtr"),
				BuiltInType.UIntPtr => GetTypeByName(module, "System.UIntPtr"),
				BuiltInType.TypedReference => GetTypeByName(module, "System.TypedReference"),
				BuiltInType.ValueType => GetTypeByName(module, "System.ValueType"),
				_ => throw new CompilerException("Searching for invalid built-in type.")
			};
		}

		public MosaType? ResolveType(MosaModule? module, MosaTypeCode type)
		{
			return type switch
			{
				MosaTypeCode.Void => GetTypeByName(module, "System.Void"),
				MosaTypeCode.Boolean => GetTypeByName(module, "System.Boolean"),
				MosaTypeCode.Char => GetTypeByName(module, "System.Char"),
				MosaTypeCode.I1 => GetTypeByName(module, "System.SByte"),
				MosaTypeCode.U1 => GetTypeByName(module, "System.Byte"),
				MosaTypeCode.I2 => GetTypeByName(module, "System.Int16"),
				MosaTypeCode.U2 => GetTypeByName(module, "System.UInt16"),
				MosaTypeCode.I4 => GetTypeByName(module, "System.Int32"),
				MosaTypeCode.U4 => GetTypeByName(module, "System.UInt32"),
				MosaTypeCode.I8 => GetTypeByName(module, "System.Int64"),
				MosaTypeCode.U8 => GetTypeByName(module, "System.UInt64"),
				MosaTypeCode.R4 => GetTypeByName(module, "System.Single"),
				MosaTypeCode.R8 => GetTypeByName(module, "System.Double"),
				MosaTypeCode.String => GetTypeByName(module, "System.String"),
				MosaTypeCode.ValueType => GetTypeByName(module, "System.ValueType"),
				MosaTypeCode.Array => GetTypeByName(module, "System.Array"),
				MosaTypeCode.TypedRef => GetTypeByName(module, "System.TypedReference"),
				MosaTypeCode.I => GetTypeByName(module, "System.IntPtr"),
				MosaTypeCode.U => GetTypeByName(module, "System.UIntPtr"),
				MosaTypeCode.Object => GetTypeByName(module, "System.Object"),
				MosaTypeCode.SZArray => GetTypeByName(module, "System.Array"),
				_ => throw new CompilerException("Searching for invalid type code.")
			};
		}

		public MosaType? GetTypeByName(IList<MosaModule?> modules, string fullName)
		{
			foreach (var module in modules)
			{
				var result = GetTypeByName(module, fullName);
				if (result != null)
					return result;
			}

			return null;
		}

		public MosaType? GetTypeByName(MosaModule? module, string fullName)
		{
			return typeLookup.TryGetValue(Tuple.Create(module, fullName), out var result) ? result : null;
		}
	}
}
