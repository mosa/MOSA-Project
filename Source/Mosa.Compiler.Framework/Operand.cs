// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Operand class
/// </summary>
public sealed class Operand
{
	#region Future

	// Primitive Type: Integer (32/64), Float (R4,R8), Object, Managed Pointer, ValueType
	// Operand Type: Virtual Register, Physical Register, Local Stack, Paramater, Static Field, Label
	// Attribute: Constant, Unresolved Constant

	public enum ElementTypeEnum
	{ Int32, Int64, R4, R8, Object, ManagedPointer, ValueType };

	#endregion Future

	#region Properties

	public ElementTypeEnum ElementType { get; private set; }

	/// <summary>
	/// Gets the constant double float point.
	/// </summary>
	public double ConstantDouble { get; private set; }

	/// <summary>
	/// Gets the single double float point.
	/// </summary>
	public float ConstantFloat { get; private set; }

	/// <summary>
	/// Gets or sets the constant signed integer.
	/// </summary>
	public int ConstantSigned32 { get => (int)ConstantUnsigned64; private set => ConstantUnsigned64 = (ulong)value; }

	/// <summary>
	/// Gets or sets the constant signed long integer.
	/// </summary>
	public long ConstantSigned64 { get => (long)ConstantUnsigned64; private set => ConstantUnsigned64 = (ulong)value; }

	/// <summary>
	/// Gets the constant integer.
	/// </summary>
	public uint ConstantUnsigned32 { get => (uint)ConstantUnsigned64; private set => ConstantUnsigned64 = value; }

	/// <summary>
	/// Gets the constant long integer.
	/// </summary>
	public ulong ConstantUnsigned64 { get; private set; }

	/// <summary>
	/// Holds a list of instructions, which define this operand.
	/// </summary>
	public List<InstructionNode> Definitions { get; }

	/// <summary>
	/// Retrieves the field.
	/// </summary>
	public MosaField Field { get; private set; }

	/// <summary>
	/// Gets a value indicating whether this instance has long parent.
	/// </summary>
	public bool HasLongParent => LongParent != null;

	/// <summary>
	/// Gets or sets the high operand.
	/// </summary>
	public Operand High { get; private set; }

	/// <summary>
	/// Gets the index.
	/// </summary>
	public int Index { get; private set; }

	/// <summary>
	/// Gets a value indicating whether [is 64 bit integer].
	/// </summary>
	public bool IsInteger64 => ElementType == ElementTypeEnum.Int64;

	public bool IsInteger32 => ElementType == ElementTypeEnum.Int32;

	/// <summary>
	/// Determines if the operand is a constant variable.
	/// </summary>
	public bool IsConstant { get; private set; }

	/// <summary>
	/// Gets a value indicating whether [is constant one].
	/// </summary>
	/// <exception cref="CompilerException"></exception>
	public bool IsConstantOne
	{
		get
		{
			if (!IsResolvedConstant)
				return false;
			else if (IsStackLocal || IsOnStack || IsParameter)
				return ConstantUnsigned64 == 1;
			else if (IsNull)
				return false;
			else if (IsR8)
				return ConstantDouble == 1;
			else if (IsR4)
				return ConstantFloat == 1;
			else
				return ConstantUnsigned64 == 1;
		}
	}

	/// <summary>
	/// Gets a value indicating whether [is constant zero].
	/// </summary>
	/// <exception cref="CompilerException"></exception>
	public bool IsConstantZero
	{
		get
		{
			if (!IsResolvedConstant)
				return false;
			else if (IsStackLocal || IsOnStack || IsParameter)
				return ConstantUnsigned64 == 0;
			else if (IsNull)
				return true;
			else if (IsR8)
				return ConstantDouble == 0;
			else if (IsR4)
				return ConstantFloat == 0;
			else
				return ConstantUnsigned64 == 0;
		}
	}

	/// <summary>
	/// Determines if the operand is a cpu register operand.
	/// </summary>
	public bool IsCPURegister { get; private set; }

	public bool IsFloatingPoint => IsR4 | IsR8;

	public bool IsHigh => LongParent.High == this;

	public bool IsInteger => ElementType == ElementTypeEnum.Int32 || ElementType == ElementTypeEnum.Int64;

	/// <summary>
	/// Determines if the operand is a label operand.
	/// </summary>
	public bool IsLabel { get; private set; }

	public bool IsLow => LongParent.Low == this;

	public bool IsManagedPointer => ElementType == ElementTypeEnum.ManagedPointer;

	/// <summary>
	/// Gets a value indicating whether [is null].
	/// </summary>
	public bool IsNull { get; private set; }

	/// <summary>
	/// Gets a value indicating whether this instance is on stack.
	/// </summary>
	public bool IsOnStack => IsStackLocal || IsParameter;

	/// <summary>
	/// Determines if the operand is a local variable operand.
	/// </summary>
	public bool IsParameter { get; private set; }

	public bool IsPinned { get; private set; }

	public bool IsR4 => ElementType == ElementTypeEnum.R4;

	public bool IsR8 => ElementType == ElementTypeEnum.R8;

	public bool IsReferenceType => ElementType == ElementTypeEnum.Object;

	public bool IsResolved { get; set; }    // FUTURE: make set private

	/// <summary>
	/// Gets a value indicating whether this instance is resolved constant.
	/// </summary>
	public bool IsResolvedConstant => IsConstant && IsResolved;

	/// <summary>
	/// Determines if the operand is a local stack operand.
	/// </summary>
	public bool IsStackLocal { get; private set; }

	/// <summary>
	/// Determines if the operand is a runtime member operand.
	/// </summary>
	public bool IsStaticField { get; private set; }

	public bool IsString { get; private set; }

	/// <summary>
	/// Gets a value indicating whether this instance is unresolved constant.
	/// </summary>
	public bool IsUnresolvedConstant => IsConstant && !IsResolved;

	public bool IsValueType => ElementType == ElementTypeEnum.ValueType;

	/// <summary>
	/// Determines if the operand is a virtual register operand.
	/// </summary>
	public bool IsVirtualRegister { get; private set; }

	/// <summary>
	/// Gets the split64 parent.
	/// </summary>
	public Operand LongParent { get; private set; }

	/// <summary>
	/// Gets or sets the low operand.
	/// </summary>
	public Operand Low { get; private set; }

	/// <summary>
	/// Retrieves the method.
	/// </summary>
	public MosaMethod Method { get; private set; }

	/// <summary>
	/// Gets the name.
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Gets or sets the offset.
	/// </summary>
	public long Offset
	{
		get { Debug.Assert(IsResolved); return ConstantSigned64; }
		set { Debug.Assert(!IsResolved); ConstantSigned64 = value; }
	}

	/// <summary>
	/// Retrieves the register, where the operand is located.
	/// </summary>
	public PhysicalRegister Register { get; private set; }

	/// <summary>
	/// Gets the string data.
	/// </summary>
	public string StringData { get; private set; }

	/// <summary>
	/// Returns the type of the operand.
	/// </summary>
	public MosaType Type { get; private set; }

	/// <summary>
	/// Holds a list of instructions, which use this operand.
	/// </summary>
	public List<InstructionNode> Uses { get; }

	#endregion Properties

	#region Construction

	private Operand()
	{
		Definitions = new List<InstructionNode>();
		Uses = new List<InstructionNode>();
		IsParameter = false;
		IsStackLocal = false;
		IsConstant = false;
		IsVirtualRegister = false;
		IsLabel = false;
		IsCPURegister = false;
		IsStaticField = false;
		IsParameter = false;
		IsResolved = false;
		IsString = false;
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Operand"/>.
	/// </summary>
	/// <param name="type">The type of the operand.</param>
	private Operand(MosaType type)
		: this()
	{
		Type = type;
		ElementType = GetElementType(type);
	}

	private Operand(Operand operand)
	: this()
	{
		ElementType = operand.ElementType;

		if (ElementType == ElementTypeEnum.ValueType)
			Type = operand.Type;
	}

	public static ElementTypeEnum GetElementType(MosaType type)
	{
		if (type.IsEnum)
			type = type.GetEnumUnderlyingType();

		if (type.IsReferenceType)
			return ElementTypeEnum.Object;
		else if (type.IsI1 || type.IsI2 || type.IsI4 || type.IsU1 || type.IsU2 || type.IsU4)
			return ElementTypeEnum.Int32;
		else if (type.IsI8 || type.IsU8)
			return ElementTypeEnum.Int64;
		else if (type.IsR4)
			return ElementTypeEnum.R4;
		else if (type.IsR8)
			return ElementTypeEnum.R8;
		else if (type.IsManagedPointer)
			return ElementTypeEnum.ManagedPointer;
		else if (type.IsValueType)
			return ElementTypeEnum.ValueType;

		return ElementTypeEnum.Int32; // FIXME
	}

	#endregion Construction

	#region Static Factory Constructors v2 [Experimental]

	public static Operand CreateVirtual32(int index)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.Int32,
			IsVirtualRegister = true,
			IsConstant = false,
			IsResolved = false,
			Index = index,
		};
	}

	public static Operand CreateVirtual64(int index)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.Int64,
			IsVirtualRegister = true,
			Index = index,
		};
	}

	public static Operand CreateVirtualR4(int index)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.R4,
			IsVirtualRegister = true,
			Index = index,
		};
	}

	public static Operand CreateVirtualR8(int index)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.R8,
			IsVirtualRegister = true,
			Index = index,
		};
	}

	public static Operand CreateConstant32(uint value)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.Int32,
			IsConstant = true,
			ConstantUnsigned32 = value,
			IsResolved = true,
		};
	}

	public static Operand CreateConstant64(ulong value)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.Int64,
			IsConstant = true,
			ConstantUnsigned64 = value,
			IsResolved = true,
		};
	}

	public static Operand CreateConstantR4(float value)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.R4,
			IsConstant = true,
			ConstantFloat = value,
			IsResolved = true,
		};
	}

	public static Operand CreateConstantR8(double value)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.R8,
			IsConstant = true,
			ConstantDouble = value,
			IsResolved = true,
		};
	}

	public static Operand CreateVirtualManagedPointer(int index)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.ManagedPointer,
			IsVirtualRegister = true,
			Index = index,
		};
	}

	public static Operand CreateVirtualObject(int index)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.Object,
			IsVirtualRegister = true,
			Index = index,
		};
	}

	public static Operand CreateObjectNull()
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.Object,
			IsConstant = true,
			ConstantUnsigned64 = 0,
			IsNull = true,
			IsResolved = true,
		};
	}

	public static Operand CreateManagedPointer()
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.ManagedPointer,
			IsConstant = false,
			ConstantUnsigned64 = 0,
			IsNull = false,
			IsResolved = true,
		};
	}

	public static Operand CreateLabel(string label, bool Is32Platform)
	{
		return new Operand
		{
			ElementType = Is32Platform ? ElementTypeEnum.Int32 : ElementTypeEnum.Int64,
			IsLabel = true,
			Name = label,
			Offset = 0,
			IsConstant = true
		};
	}

	public static Operand CreateLabelR4(string label)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.R4,
			IsLabel = true,
			Name = label,
			Offset = 0,
			IsConstant = true
		};
	}

	public static Operand CreateLabelR8(string label)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.R8,
			IsLabel = true,
			Name = label,
			Offset = 0,
			IsConstant = true
		};
	}

	public static Operand CreateLabelObject(string label)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.Object,
			IsLabel = true,
			Name = label,
			Offset = 0,
			IsConstant = true
		};
	}

	public static Operand CreateVirtualRegister(Operand operand, int index)
	{
		return new Operand(operand)
		{
			IsVirtualRegister = true,
			Index = index
		};
	}

	public static Operand CreateCPURegister(Operand operand, PhysicalRegister register)
	{
		return new Operand(operand)
		{
			IsCPURegister = true,
			Register = register
		};
	}

	public static Operand CreateCPURegister32(PhysicalRegister register)
	{
		return new Operand()
		{
			ElementType = ElementTypeEnum.Int32,
			IsCPURegister = true,
			Register = register
		};
	}

	public static Operand CreateCPURegister64(PhysicalRegister register)
	{
		return new Operand()
		{
			ElementType = ElementTypeEnum.Int64,
			IsCPURegister = true,
			Register = register
		};
	}

	public static Operand CreateCPURegisterR4(PhysicalRegister register)
	{
		return new Operand()
		{
			ElementType = ElementTypeEnum.R4,
			IsCPURegister = true,
			Register = register
		};
	}

	public static Operand CreateCPURegisterR8(PhysicalRegister register)
	{
		return new Operand()
		{
			ElementType = ElementTypeEnum.R8,
			IsCPURegister = true,
			Register = register
		};
	}

	public static Operand CreateCPURegisterObject(PhysicalRegister register)
	{
		return new Operand()
		{
			ElementType = ElementTypeEnum.Object,
			IsCPURegister = true,
			Register = register
		};
	}

	public static Operand CreateCPURegisterManagedPointer(PhysicalRegister register)
	{
		return new Operand()
		{
			ElementType = ElementTypeEnum.ManagedPointer,
			IsCPURegister = true,
			Register = register
		};
	}

	public static Operand CreateCPURegisterNativeInteger(PhysicalRegister register, bool is32Platform)
	{
		return new Operand
		{
			ElementType = is32Platform ? ElementTypeEnum.Int32 : ElementTypeEnum.Int64,
			IsCPURegister = true,
			Register = register
		};
	}

	#endregion Static Factory Constructors v2 [Experimental]

	#region Static Factory Constructors

	/// <summary>
	/// Creates the high 32 bit portion of a 64-bit <see cref="Operand" />.
	/// </summary>
	/// <param name="longOperand">The long operand.</param>
	/// <param name="index">The index.</param>
	/// <param name="typeSystem">The type system.</param>
	/// <returns></returns>
	public static Operand CreateHighSplitForLong(Operand longOperand, int index, TypeSystem typeSystem)
	{
		Debug.Assert(longOperand.LongParent == null || longOperand.LongParent == longOperand);
		Debug.Assert(longOperand.High == null);

		Operand operand = null;

		if (longOperand.IsParameter)
		{
			operand = CreateStackParameter(typeSystem.BuiltIn.U4, longOperand.Index, longOperand.Name + " (High)", (int)longOperand.Offset + 4);
		}
		else if (longOperand.IsResolvedConstant)
		{
			operand = new Operand(typeSystem.BuiltIn.U4)
			{
				IsConstant = true,
				IsResolved = true,
				ConstantUnsigned64 = (uint)(longOperand.ConstantUnsigned64 >> 32) & uint.MaxValue
			};
		}
		else if (longOperand.IsVirtualRegister)
		{
			operand = new Operand(typeSystem.BuiltIn.U4)
			{
				IsVirtualRegister = true,
				Index = index,
			};
		}
		else if (longOperand.IsStackLocal)
		{
			operand = new Operand(typeSystem.BuiltIn.I4)
			{
				IsConstant = true,
				IsResolved = false,
				IsStackLocal = true,
				ConstantSigned64 = 4
			};
		}
		else if (longOperand.IsStaticField)
		{
			//future
		}

		Debug.Assert(operand != null);

		operand.LongParent = longOperand;
		longOperand.High = operand;

		return operand;
	}

	/// <summary>
	/// Creates the low 32 bit portion of a 64-bit <see cref="Operand" />.
	/// </summary>
	/// <param name="longOperand">The long operand.</param>
	/// <param name="index">The index.</param>
	/// <param name="typeSystem">The type system.</param>
	/// <returns></returns>
	public static Operand CreateLowSplitForLong(Operand longOperand, int index, TypeSystem typeSystem)
	{
		Debug.Assert(longOperand.LongParent == null);
		Debug.Assert(longOperand.Low == null);

		Operand operand = null;

		if (longOperand.IsParameter)
		{
			operand = CreateStackParameter(typeSystem.BuiltIn.U4, longOperand.Index, longOperand.Name + " (Low)", (int)longOperand.Offset);
		}
		else if (longOperand.IsResolvedConstant)
		{
			operand = new Operand(typeSystem.BuiltIn.U4)
			{
				IsConstant = true,
				IsResolved = true,
				ConstantUnsigned64 = longOperand.ConstantUnsigned64 & uint.MaxValue,
			};
		}
		else if (longOperand.IsVirtualRegister)
		{
			operand = new Operand(typeSystem.BuiltIn.U4)
			{
				IsVirtualRegister = true,
				Index = index,
			};
		}
		else if (longOperand.IsStackLocal)
		{
			operand = CreateStackLocal(typeSystem.BuiltIn.I4, 0, longOperand.IsPinned);
		}
		else if (longOperand.IsStaticField)
		{
			//future
		}

		Debug.Assert(operand != null);

		operand.LongParent = longOperand;
		longOperand.Low = operand;

		return operand;
	}

	/// <summary>
	/// Creates the stack local.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="index">The index.</param>
	/// <param name="pinned">if set to <c>true</c> [pinned].</param>
	/// <returns></returns>
	public static Operand CreateStackLocal(MosaType type, int index, bool pinned)
	{
		return new Operand(type)
		{
			Index = index,
			IsStackLocal = true,
			IsConstant = true,
			IsPinned = pinned
		};
	}

	/// <summary>
	/// Creates the stack parameter.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="index">The index.</param>
	/// <param name="name">The name.</param>
	/// <param name="isThis">if set to <c>true</c> [is this].</param>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	public static Operand CreateStackParameter(MosaType type, int index, string name, int offset)
	{
		return new Operand(type)
		{
			IsParameter = true,
			Index = index,
			IsConstant = true,
			IsResolved = true,
			Name = name,
			ConstantSigned64 = offset
		};
	}

	/// <summary>
	/// Creates a new runtime member <see cref="Operand" />.
	/// </summary>
	/// <param name="field">The field.</param>
	/// <param name="typeSystem">The type system.</param>
	/// <returns></returns>
	public static Operand CreateStaticField(MosaField field, TypeSystem typeSystem)
	{
		Debug.Assert(field.IsStatic);

		var type = field.FieldType.IsReferenceType ? typeSystem.BuiltIn.Object : field.FieldType.ToManagedPointer();

		return new Operand(type)
		{
			IsStaticField = true, // field.IsStatic
			Offset = 0,
			Field = field,
			IsConstant = true
		};
	}

	/// <summary>
	/// Creates the string symbol.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="name">The name.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="data">The data.</param>
	/// <returns></returns>
	public static Operand CreateStringSymbol(string name, uint offset, string data)
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.Object,
			IsLabel = true,
			Name = name,
			IsConstant = true,
			Offset = offset,
			StringData = data,
			IsString = true,
		};
	}

	public static Operand CreateSymbol(MosaMethod method, bool is32Platform)
	{
		Debug.Assert(method != null);

		return new Operand
		{
			ElementType = is32Platform ? ElementTypeEnum.Int32 : ElementTypeEnum.Int64,
			IsLabel = true,
			Method = method,
			Name = method.FullName,
			IsConstant = true
		};
	}

	/// <summary>
	/// Creates a new virtual register <see cref="Operand" />.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	public static Operand CreateVirtualRegister(MosaType type, int index)
	{
		return new Operand(type)
		{
			IsVirtualRegister = true,
			Index = index
		};
	}

	/// <summary>
	/// Gets the null constant <see cref="Operand" />.
	/// </summary>
	/// <returns></returns>
	public static Operand GetNull()
	{
		return new Operand
		{
			ElementType = ElementTypeEnum.Object,
			IsNull = true,
			IsConstant = true,
			IsResolved = true
		};
	}

	#endregion Static Factory Constructors

	#region Name Output

	public string GetElementString()
	{
		return ElementType switch
		{
			ElementTypeEnum.Int64 => "I8",
			ElementTypeEnum.Int32 => "I4",
			ElementTypeEnum.R4 => "R4",
			ElementTypeEnum.R8 => "R8",
			ElementTypeEnum.Object => "O",
			ElementTypeEnum.ManagedPointer => "MP",
			ElementTypeEnum.ValueType => "ValueType",
			_ => throw new CompilerException($"Unknown Type {ElementType}"),
		};
	}

	#endregion Name Output

	#region Object Overrides

	public override string ToString()
	{
		var sb = new StringBuilder();

		if (IsConstant)
		{
			sb.Append(" const=");

			if (!IsResolved)
			{
				sb.Append("unresolved");

				if (ConstantSigned64 != 0)

					sb.Append($" offset={ConstantSigned64}");
			}
			else if (IsNull)
			{
				sb.Append("null");
			}
			else if (IsOnStack)
			{
				sb.Append(ConstantSigned64);
			}
			else if (IsR8)
			{
				sb.Append(ConstantDouble);
			}
			else if (IsR4)
			{
				sb.Append(ConstantFloat);
			}
			else
			{
				sb.Append(ConstantSigned64);
			}

			sb.Append(' ');
		}
		else if (IsCPURegister)
		{
			sb.Append($" {Register}");
		}

		if (IsVirtualRegister)
		{
			if (!HasLongParent)
			{
				sb.Append($"v{Index}");
			}
			else
			{
				sb.Append($"(v{Index}<v{LongParent.Index}{(IsHigh ? "H" : "L")}>)");
			}
		}
		else if (IsParameter)
		{
			if (!HasLongParent)
			{
				sb.Append($"(p{Index})");
			}
			else
			{
				sb.Append($"(p{Index}<t{LongParent.Index}{(IsHigh ? "H" : "L")}>)");
			}
		}
		else if (IsStackLocal && Name == null)
		{
			if (!HasLongParent)
			{
				sb.Append($"(t{Index})");
			}
			else
			{
				sb.Append($"(t{Index}<t{LongParent.Index}{(IsHigh ? "H" : "L")}>)");
			}
		}
		else if (IsStaticField)
		{
			sb.Append($" ({Field.FullName}) ");
		}
		else if (Name != null)
		{
			sb.Append($" ({Name}) ");
		}

		sb.Append($" [{GetElementString()}]");

		if (ElementType == ElementTypeEnum.ValueType)
		{
			sb.Append($" ({Type.FullName})");
		}

		return sb.ToString();
	}

	#endregion Object Overrides

	internal void RenameIndex(int index)
	{
		Debug.Assert(IsVirtualRegister || IsStackLocal);
		//Debug.Assert(!IsStackLocal);

		Index = index;
	}
}
