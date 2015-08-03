// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	public static class TypeSystemExtensions
	{
		/// <summary>
		/// Gets the type on the stack.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// The equivalent stack type code.
		/// </returns>
		/// <exception cref="InvalidCompilerException"></exception>
		public static StackTypeCode GetStackTypeCode(this MosaType type)
		{
			switch (type.IsEnum ? type.GetEnumUnderlyingType().TypeCode : type.TypeCode)
			{
				case MosaTypeCode.Boolean:
				case MosaTypeCode.Char:
				case MosaTypeCode.I1:
				case MosaTypeCode.U1:
				case MosaTypeCode.I2:
				case MosaTypeCode.U2:
				case MosaTypeCode.I4:
				case MosaTypeCode.U4:
					return StackTypeCode.Int32;

				case MosaTypeCode.I8:
				case MosaTypeCode.U8:
					return StackTypeCode.Int64;

				case MosaTypeCode.R4:
				case MosaTypeCode.R8:
					return StackTypeCode.F;

				case MosaTypeCode.I:
				case MosaTypeCode.U:
					return StackTypeCode.N;

				case MosaTypeCode.ManagedPointer:
					return StackTypeCode.ManagedPointer;

				case MosaTypeCode.UnmanagedPointer:
				case MosaTypeCode.FunctionPointer:
					return StackTypeCode.UnmanagedPointer;

				case MosaTypeCode.String:
				case MosaTypeCode.ValueType:
				case MosaTypeCode.ReferenceType:
				case MosaTypeCode.Array:
				case MosaTypeCode.Object:
				case MosaTypeCode.SZArray:
				case MosaTypeCode.Var:
				case MosaTypeCode.MVar:
					return StackTypeCode.O;

				case MosaTypeCode.Void:
					return StackTypeCode.Unknown;
			}
			throw new InvalidCompilerException(string.Format("Can't transform Type {0} to StackTypeCode.", type));
		}

		public static MosaType GetStackType(this MosaType type)
		{
			switch (type.GetStackTypeCode())
			{
				case StackTypeCode.Int32:
					return type.TypeSystem.BuiltIn.I4;

				case StackTypeCode.Int64:
					return type.TypeSystem.BuiltIn.I8;

				case StackTypeCode.N:
					return type.TypeSystem.BuiltIn.I;

				case StackTypeCode.F:
					if (type.IsR4)
						return type.TypeSystem.BuiltIn.R4;
					return type.TypeSystem.BuiltIn.R8;

				case StackTypeCode.O:
					return type;

				case StackTypeCode.UnmanagedPointer:
				case StackTypeCode.ManagedPointer:
					return type;
			}
			throw new CompilerException("Can't convert '" + type.FullName + "' to stack type.");
		}

		public static MosaType GetStackTypeFromCode(this TypeSystem typeSystem, StackTypeCode code)
		{
			switch (code)
			{
				case StackTypeCode.Int32:
					return typeSystem.BuiltIn.I4;

				case StackTypeCode.Int64:
					return typeSystem.BuiltIn.I8;

				case StackTypeCode.N:
					return typeSystem.BuiltIn.I;

				case StackTypeCode.F:
					return typeSystem.BuiltIn.R8;

				case StackTypeCode.O:
					return typeSystem.BuiltIn.Object;

				case StackTypeCode.UnmanagedPointer:
					return typeSystem.BuiltIn.Pointer;

				case StackTypeCode.ManagedPointer:
					return typeSystem.BuiltIn.Object.ToManagedPointer();
			}
			throw new CompilerException("Can't convert stack type code'" + code + "' to type.");
		}

		public static MosaType GetTypeFromTypeCode(this TypeSystem typeSystem, MosaTypeCode code)
		{
			switch (code)
			{
				case MosaTypeCode.Void: return typeSystem.BuiltIn.Void;
				case MosaTypeCode.Boolean: return typeSystem.BuiltIn.Boolean;
				case MosaTypeCode.Char: return typeSystem.BuiltIn.Char;
				case MosaTypeCode.I1: return typeSystem.BuiltIn.I1;
				case MosaTypeCode.U1: return typeSystem.BuiltIn.U1;
				case MosaTypeCode.I2: return typeSystem.BuiltIn.I2;
				case MosaTypeCode.U2: return typeSystem.BuiltIn.U2;
				case MosaTypeCode.I4: return typeSystem.BuiltIn.I4;
				case MosaTypeCode.U4: return typeSystem.BuiltIn.U4;
				case MosaTypeCode.I8: return typeSystem.BuiltIn.I8;
				case MosaTypeCode.U8: return typeSystem.BuiltIn.U8;
				case MosaTypeCode.R4: return typeSystem.BuiltIn.R4;
				case MosaTypeCode.R8: return typeSystem.BuiltIn.R8;
				case MosaTypeCode.I: return typeSystem.BuiltIn.I;
				case MosaTypeCode.U: return typeSystem.BuiltIn.U;
				case MosaTypeCode.String: return typeSystem.BuiltIn.String;
				case MosaTypeCode.TypedRef: return typeSystem.BuiltIn.TypedRef;
				case MosaTypeCode.Object: return typeSystem.BuiltIn.Object;
			}
			throw new CompilerException("Can't convert type code '" + code + "' to type.");
		}
	}
}