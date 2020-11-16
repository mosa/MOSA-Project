// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.MosaTypeSystem
{
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

		//public MosaType ValueType { get; }

		public BuiltInTypes(TypeSystem typeSystem, MosaModule corlib)
		{
			Void = typeSystem.GetTypeByName(corlib, "System", "Void");
			Boolean = typeSystem.GetTypeByName(corlib, "System", "Boolean");
			Char = typeSystem.GetTypeByName(corlib, "System", "Char");
			I1 = typeSystem.GetTypeByName(corlib, "System", "SByte");
			U1 = typeSystem.GetTypeByName(corlib, "System", "Byte");
			I2 = typeSystem.GetTypeByName(corlib, "System", "Int16");
			U2 = typeSystem.GetTypeByName(corlib, "System", "UInt16");
			I4 = typeSystem.GetTypeByName(corlib, "System", "Int32");
			U4 = typeSystem.GetTypeByName(corlib, "System", "UInt32");
			I8 = typeSystem.GetTypeByName(corlib, "System", "Int64");
			U8 = typeSystem.GetTypeByName(corlib, "System", "UInt64");
			R4 = typeSystem.GetTypeByName(corlib, "System", "Single");
			R8 = typeSystem.GetTypeByName(corlib, "System", "Double");
			String = typeSystem.GetTypeByName(corlib, "System", "String");
			Object = typeSystem.GetTypeByName(corlib, "System", "Object");
			I = typeSystem.GetTypeByName(corlib, "System", "IntPtr");
			U = typeSystem.GetTypeByName(corlib, "System", "UIntPtr");
			TypedRef = typeSystem.GetTypeByName(corlib, "System", "TypedReference");
			Pointer = Void.ToUnmanagedPointer();

			//ValueType = typeSystem.GetTypeByName(corlib, "System", "ValueType");
		}

		public MosaType GetType(BuiltInType builtInType)
		{
			switch (builtInType)
			{
				case BuiltInType.Void: return Void;
				case BuiltInType.Boolean: return Boolean;
				case BuiltInType.Char: return Char;
				case BuiltInType.SByte: return I1;
				case BuiltInType.Byte: return U1;
				case BuiltInType.Int16: return I2;
				case BuiltInType.UInt16: return U2;
				case BuiltInType.Int32: return I4;
				case BuiltInType.UInt32: return U4;
				case BuiltInType.Int64: return I8;
				case BuiltInType.UInt64: return U8;
				case BuiltInType.Single: return R4;
				case BuiltInType.Double: return R8;
				case BuiltInType.String: return String;
				case BuiltInType.Object: return Object;
				case BuiltInType.IntPtr: return I;
				case BuiltInType.UIntPtr: return U;
				case BuiltInType.TypedReference: return TypedRef;

				//case BuiltInType.ValueType: return ValueType;
				default: throw new CompilerException("Invalid BuildInType");
			}
		}

		public MosaType GetType(string builtInTypeName)
		{
			switch (builtInTypeName)
			{
				case "Void": return Void;
				case "Boolean": return Boolean;
				case "Char": return Char;
				case "SByte": return I1;
				case "Byte": return U1;
				case "Int16": return I2;
				case "UInt16": return U2;
				case "Int32": return I4;
				case "UInt32": return U4;
				case "Int64": return I8;
				case "UInt64": return U8;
				case "Single": return R4;
				case "Double": return R8;
				case "String": return String;
				case "Object": return Object;
				case "IntPtr": return I;
				case "UIntPtr": return U;
				case "TypedReference": return TypedRef;
				default: throw new CompilerException("Invalid BuildInType");
			}
		}
	}
}
