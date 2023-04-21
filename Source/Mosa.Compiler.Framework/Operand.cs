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
public sealed partial class Operand
{
	#region Enums

	public enum LocationType
	{ Constant, VirtualRegister, PhysicalRegister, StackFrame, StackParameter };

	public enum ConstantType
	{ Default, Label, StaticField };

	#endregion Enums

	#region Static Properties

	public static readonly Operand NullObject = new Operand
	{
		Primitive = PrimitiveType.Object,
		Location = LocationType.Constant,
		Constant = ConstantType.Default,
		IsResolved = true,
		ConstantUnsigned64 = 0
	};

	#endregion Static Properties

	#region Properties

	public PrimitiveType Primitive { get; private set; }

	public LocationType Location { get; private set; }

	public ConstantType Constant { get; private set; }

	public ElementType Element { get; private set; }

	public ulong ConstantUnsigned64 { get; private set; }

	public double ConstantDouble { get; private set; }

	public float ConstantFloat { get; private set; }

	public int ConstantSigned32 { get => (int)ConstantUnsigned64; private set => ConstantUnsigned64 = (ulong)value; }

	public long ConstantSigned64 { get => (long)ConstantUnsigned64; private set => ConstantUnsigned64 = (ulong)value; }

	public uint ConstantUnsigned32 { get => (uint)ConstantUnsigned64; private set => ConstantUnsigned64 = value; }

	public List<InstructionNode> Definitions { get; }

	public MosaField Field { get; private set; }

	public bool HasLongParent => LongParent != null;

	public Operand High { get; private set; }

	public int Index { get; private set; }

	public bool IsInteger64 => Primitive == PrimitiveType.Int64;

	public bool IsInteger32 => Primitive == PrimitiveType.Int32;

	public bool IsConstant => Location == LocationType.Constant;

	public bool IsConstantOne
	{
		get
		{
			if (!IsResolvedConstant)
				return false;
			else if (IsLocalStack || IsOnStack || IsParameter)
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

	public bool IsConstantZero
	{
		get
		{
			if (!IsResolvedConstant)
				return false;
			else if (IsLocalStack || IsOnStack || IsParameter)
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

	public bool IsCPURegister => Location == LocationType.PhysicalRegister;

	public bool IsFloatingPoint => IsR4 | IsR8;

	public bool IsHigh => LongParent.High == this;

	public bool IsInteger => Primitive == PrimitiveType.Int32 || Primitive == PrimitiveType.Int64;

	public bool IsLabel => Constant == ConstantType.Label;

	public bool IsLow => LongParent.Low == this;

	public bool IsManagedPointer => Primitive == PrimitiveType.ManagedPointer;

	public bool IsNull => Primitive == PrimitiveType.Object && IsConstant && IsResolved && ConstantUnsigned64 == 0;

	public bool IsOnStack => IsLocalStack || IsParameter;

	public bool IsParameter => Location == LocationType.StackParameter;

	public bool IsPinned { get; private set; }

	public bool IsR4 => Primitive == PrimitiveType.R4;

	public bool IsR8 => Primitive == PrimitiveType.R8;

	public bool IsReferenceType => Primitive == PrimitiveType.Object;

	public bool IsResolved { get; set; }    // FUTURE: make set private

	public bool IsResolvedConstant => IsConstant && IsResolved;

	public bool IsLocalStack => Location == LocationType.StackFrame;

	public bool IsStaticField => Constant == ConstantType.StaticField;

	public bool IsUnresolvedConstant => IsConstant && !IsResolved;

	public bool IsValueType => Primitive == PrimitiveType.ValueType;

	public bool IsVirtualRegister => Location == LocationType.VirtualRegister;

	public Operand LongParent { get; private set; }

	public Operand Low { get; private set; }

	public MosaMethod Method { get; private set; }

	public string Name { get; private set; }

	public long Offset
	{
		get { Debug.Assert(IsResolved); return ConstantSigned64; }
		set { Debug.Assert(!IsResolved); ConstantSigned64 = value; }
	}

	public PhysicalRegister Register { get; private set; }

	public string StringData { get; private set; }

	public MosaType Type { get; private set; }

	public List<InstructionNode> Uses { get; }

	#endregion Properties

	#region Construction

	private Operand()
	{
		Definitions = new List<InstructionNode>();
		Uses = new List<InstructionNode>();
		Constant = ConstantType.Default;

		IsResolved = true;
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Operand"/>.
	/// </summary>
	/// <param name="type">The type of the operand.</param>
	private Operand(MosaType type)
		: this()
	{
		Type = type;
		Primitive = GetElementType(type);
	}

	public static PrimitiveType GetElementType(MosaType type)
	{
		if (type.IsEnum)
			type = type.GetEnumUnderlyingType();

		if (type.IsReferenceType)
			return PrimitiveType.Object;
		else if (type.IsI1 || type.IsI2 || type.IsI4 || type.IsU1 || type.IsU2 || type.IsU4)
			return PrimitiveType.Int32;
		else if (type.IsI8 || type.IsU8)
			return PrimitiveType.Int64;
		else if (type.IsR4)
			return PrimitiveType.R4;
		else if (type.IsR8)
			return PrimitiveType.R8;
		else if (type.IsManagedPointer)
			return PrimitiveType.ManagedPointer;
		else if (type.IsValueType)
			return PrimitiveType.ValueType;

		return PrimitiveType.Int32; // FIXME
	}

	#endregion Construction

	#region Factory Methods - Constants

	public static Operand CreateConstant32(uint value)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.Int32,
			Constant = ConstantType.Default,
			ConstantUnsigned32 = value,
		};
	}

	public static Operand CreateConstant64(ulong value)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.Int64,
			Constant = ConstantType.Default,
			ConstantUnsigned64 = value,
		};
	}

	public static Operand CreateConstantR4(float value)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.R4,
			Constant = ConstantType.Default,
			ConstantFloat = value,
		};
	}

	public static Operand CreateConstantR8(double value)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.R8,
			Constant = ConstantType.Default,
			ConstantDouble = value,
		};
	}

	public static Operand CreateLabel(string label, bool Is32Platform)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = Is32Platform ? PrimitiveType.Int32 : PrimitiveType.Int64,
			Constant = ConstantType.Label,
			Name = label,
			IsResolved = false,
			Offset = 0,
		};
	}

	public static Operand CreateLabelR4(string label)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.R4,
			Constant = ConstantType.Label,
			Name = label,
			IsResolved = false,
			Offset = 0,
		};
	}

	public static Operand CreateLabelR8(string label)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.R8,
			Constant = ConstantType.Label,
			Name = label,
			IsResolved = false,
			Offset = 0,
		};
	}

	public static Operand CreateLabelObject(string label)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.Object,
			Constant = ConstantType.Label,
			Name = label,
			Offset = 0,
		};
	}

	#endregion Factory Methods - Constants

	#region Factory Methods - Operands Copy

	public static Operand CreateVirtualRegister(Operand operand, int index)
	{
		Debug.Assert(operand.Type == null);

		return new Operand()
		{
			Location = LocationType.VirtualRegister,
			Primitive = operand.Primitive,
			Index = index
		};
	}

	public static Operand CreateCPURegister(Operand operand, PhysicalRegister register)
	{
		Debug.Assert(operand.Type == null);

		return new Operand()
		{
			Location = LocationType.PhysicalRegister,
			Primitive = operand.Primitive,
			Register = register
		};
	}

	#endregion Factory Methods - Operands Copy

	#region Factory Methods - CPURegister

	public static Operand CreateCPURegister32(PhysicalRegister register)
	{
		return new Operand()
		{
			Location = LocationType.PhysicalRegister,
			Primitive = PrimitiveType.Int32,
			Register = register
		};
	}

	public static Operand CreateCPURegister64(PhysicalRegister register)
	{
		return new Operand()
		{
			Location = LocationType.PhysicalRegister,
			Primitive = PrimitiveType.Int64,
			Register = register
		};
	}

	public static Operand CreateCPURegisterR4(PhysicalRegister register)
	{
		return new Operand()
		{
			Location = LocationType.PhysicalRegister,
			Primitive = PrimitiveType.R4,
			Register = register
		};
	}

	public static Operand CreateCPURegisterR8(PhysicalRegister register)
	{
		return new Operand()
		{
			Location = LocationType.PhysicalRegister,
			Primitive = PrimitiveType.R8,
			Register = register
		};
	}

	public static Operand CreateCPURegisterObject(PhysicalRegister register)
	{
		return new Operand()
		{
			Location = LocationType.PhysicalRegister,
			Primitive = PrimitiveType.Object,
			Register = register
		};
	}

	public static Operand CreateCPURegisterManagedPointer(PhysicalRegister register)
	{
		return new Operand()
		{
			Location = LocationType.PhysicalRegister,
			Primitive = PrimitiveType.ManagedPointer,
			Register = register
		};
	}

	public static Operand CreateCPURegisterNativeInteger(PhysicalRegister register, bool is32Platform)
	{
		return new Operand
		{
			Location = LocationType.PhysicalRegister,
			Primitive = is32Platform ? PrimitiveType.Int32 : PrimitiveType.Int64,
			Register = register
		};
	}

	#endregion Factory Methods - CPURegister

	#region Factory Methods - Standard

	public static Operand CreateVirtualRegister(PrimitiveType primitiveType, int index, MosaType type = null)
	{
		return new Operand
		{
			Location = LocationType.VirtualRegister,
			Primitive = primitiveType,
			Index = index,
			Type = primitiveType == PrimitiveType.ValueType ? type : null,
		};
	}

	public static Operand CreateStackLocal(PrimitiveType primitiveType, int index, bool pinned, MosaType type = null)
	{
		return new Operand
		{
			Primitive = primitiveType,
			Location = LocationType.StackFrame,
			Constant = ConstantType.Default,
			Index = index,
			IsPinned = pinned,
			Type = primitiveType == PrimitiveType.ValueType ? type : null,
		};
	}

	public static Operand CreateStackParameter(PrimitiveType primitiveType, ElementType elementType, int index, string name, int offset, MosaType type = null)
	{
		return new Operand
		{
			Primitive = primitiveType,
			Location = LocationType.StackParameter,
			Constant = ConstantType.Default,
			Element = elementType,
			Index = index,
			Name = name,
			ConstantSigned64 = offset,
			Type = type
		};
	}

	public static Operand CreateCPURegister(PrimitiveType primitiveType, PhysicalRegister register)
	{
		return new Operand()
		{
			Location = LocationType.PhysicalRegister,
			Primitive = primitiveType,
			Register = register
		};
	}

	public static Operand GetNullObject()
	{
		return NullObject;
	}

	#endregion Factory Methods - Standard

	#region Factory Methods - Long Operand

	public static Operand CreateHigh(Operand longOperand, int index)
	{
		Debug.Assert(longOperand.LongParent == null || longOperand.LongParent == longOperand);
		Debug.Assert(longOperand.High == null);

		Operand operand = null;

		if (longOperand.IsParameter)
		{
			operand = CreateStackParameter(longOperand.Primitive, longOperand.Element, longOperand.Index, $"{longOperand.Name} (High)", (int)longOperand.Offset + 4);
		}
		else if (longOperand.IsResolvedConstant)
		{
			operand = new Operand
			{
				Location = LocationType.Constant,
				Primitive = PrimitiveType.Int32,
				Constant = ConstantType.Default,
				ConstantUnsigned64 = (uint)(longOperand.ConstantUnsigned64 >> 32) & uint.MaxValue
			};
		}
		else if (longOperand.IsVirtualRegister)
		{
			operand = new Operand
			{
				Location = LocationType.VirtualRegister,
				Primitive = PrimitiveType.Int32,
				Index = index,
			};
		}
		else if (longOperand.IsLocalStack)
		{
			operand = new Operand
			{
				Location = LocationType.StackFrame,
				Primitive = PrimitiveType.Int32,
				Constant = ConstantType.Default,
				IsResolved = false,
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

	public static Operand CreateLow(Operand longOperand, int index)
	{
		Debug.Assert(longOperand.LongParent == null);
		Debug.Assert(longOperand.Low == null);

		Operand operand = null;

		if (longOperand.IsParameter)
		{
			operand = CreateStackParameter(longOperand.Primitive, longOperand.Element, longOperand.Index, $"{longOperand.Name} (Low)", (int)longOperand.Offset);
		}
		else if (longOperand.IsResolvedConstant)
		{
			operand = new Operand
			{
				Location = LocationType.Constant,
				Primitive = PrimitiveType.Int32,
				Constant = ConstantType.Default,
				ConstantUnsigned64 = longOperand.ConstantUnsigned64 & uint.MaxValue,
			};
		}
		else if (longOperand.IsVirtualRegister)
		{
			operand = new Operand
			{
				Location = LocationType.VirtualRegister,
				Primitive = PrimitiveType.Int32,
				Index = index,
			};
		}
		else if (longOperand.IsLocalStack)
		{
			operand = new Operand
			{
				Location = LocationType.StackFrame,
				Primitive = PrimitiveType.Int32,
				Constant = ConstantType.Default,
				IsResolved = false,
				ConstantSigned64 = 4,
				Index = 0,
				IsPinned = longOperand.IsPinned
			};
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

	#endregion Factory Methods - Long Operand

	public static Operand CreateStringLabel(string name, uint offset, string data)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.Object,
			Constant = ConstantType.Label,
			Name = name,
			Offset = offset,
			StringData = data,
			IsResolved = false
		};
	}

	public static Operand CreateLabel(MosaMethod method, bool is32Platform)
	{
		Debug.Assert(method != null);

		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = is32Platform ? PrimitiveType.Int32 : PrimitiveType.Int64,
			Constant = ConstantType.Label,
			Method = method,
			Name = method.FullName,
		};
	}

	public static Operand CreateStaticField(PrimitiveType primitiveType, MosaField field)
	{
		return new Operand()
		{
			Primitive = primitiveType,
			Location = LocationType.Constant,
			Constant = ConstantType.StaticField,
			Offset = 0,
			Field = field,
		};
	}

	#region Name Output

	public string GetElementString()
	{
		return Primitive switch
		{
			PrimitiveType.Int64 => "I8",
			PrimitiveType.Int32 => "I4",
			PrimitiveType.R4 => "R4",
			PrimitiveType.R8 => "R8",
			PrimitiveType.Object => "O",
			PrimitiveType.ManagedPointer => "MP",
			PrimitiveType.ValueType => "ValueType",
			_ => throw new CompilerException($"Unknown Type {Primitive}"),
		};
	}

	#endregion Name Output

	#region Override - ToString()

	public override string ToString()
	{
		var sb = new StringBuilder();

		if (IsConstant)
		{
			sb.Append("const=");

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
			sb.Append($"{Register}");
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
		else if (IsLocalStack && Name == null)
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
			sb.Append($" ({Field.FullName})");
		}
		else if (Name != null)
		{
			sb.Append($" ({Name})");
		}

		sb.Append($" [{GetElementString()}]");

		if (Primitive == PrimitiveType.ValueType)
		{
			sb.Append($" ({Type.FullName})");
		}

		return sb.ToString().Trim();
	}

	#endregion Override - ToString()

	internal void RenameIndex(int index)
	{
		Debug.Assert(IsVirtualRegister || IsLocalStack);

		Index = index;
	}
}
