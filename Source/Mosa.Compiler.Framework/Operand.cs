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

	public bool HasParent => Parent != null;

	public Operand High { get; private set; }

	public int Index { get; private set; }

	public bool IsInt64 => Primitive == PrimitiveType.Int64;

	public bool IsInt32 => Primitive == PrimitiveType.Int32;

	public bool IsConstant => Location == LocationType.Constant || Location == LocationType.StackFrame || Location == LocationType.StackParameter;

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

	public bool IsHigh => Parent.High == this;

	public bool IsInteger => Primitive == PrimitiveType.Int32 || Primitive == PrimitiveType.Int64;

	public bool IsLabel => Constant == ConstantType.Label;

	public bool IsLow => Parent.Low == this;

	public bool IsManagedPointer => Primitive == PrimitiveType.ManagedPointer;

	public bool IsNull => Primitive == PrimitiveType.Object && IsConstant && IsResolved && ConstantUnsigned64 == 0;

	public bool IsOnStack => IsLocalStack || IsParameter;

	public bool IsParameter => Location == LocationType.StackParameter;

	public bool IsPinned { get; private set; }

	public bool IsPrimitive => Primitive != PrimitiveType.ValueType;

	public bool IsR4 => Primitive == PrimitiveType.R4;

	public bool IsR8 => Primitive == PrimitiveType.R8;

	public bool IsObject => Primitive == PrimitiveType.Object;

	public bool IsResolved { get; internal set; }    // FUTURE: make set private

	public bool IsResolvedConstant => IsConstant && IsResolved;

	public bool IsLocalStack => Location == LocationType.StackFrame;

	public bool IsStaticField => Constant == ConstantType.StaticField;

	public bool IsUnresolvedConstant => IsConstant && !IsResolved;

	public bool IsValueType => Primitive == PrimitiveType.ValueType;

	public bool IsVirtualRegister => Location == LocationType.VirtualRegister;

	public Operand Parent { get; private set; }

	public Operand Low { get; private set; }

	public MosaMethod Method { get; private set; }

	public string Name { get; private set; }

	public long Offset
	{
		get { Debug.Assert(IsResolved); return ConstantSigned64; }
		set { Debug.Assert(!IsResolved); ConstantSigned64 = value; }
	}

	public PhysicalRegister Register { get; private set; }

	public uint Size { get; private set; }

	public string StringData { get; private set; }

	public MosaType Type { get; private set; }

	public List<InstructionNode> Uses { get; }

	#endregion Properties

	#region Static Constants

	public static readonly Operand Constant32_0 = CreateConstant32Internal(0);
	public static readonly Operand Constant32_1 = CreateConstant32Internal(1);
	public static readonly Operand Constant32_2 = CreateConstant32Internal(2);
	public static readonly Operand Constant32_3 = CreateConstant32Internal(3);
	public static readonly Operand Constant32_4 = CreateConstant32Internal(4);
	public static readonly Operand Constant32_8 = CreateConstant32Internal(8);
	public static readonly Operand Constant32_15 = CreateConstant32Internal(15);
	public static readonly Operand Constant32_16 = CreateConstant32Internal(16);
	public static readonly Operand Constant32_24 = CreateConstant32Internal(24);
	public static readonly Operand Constant32_31 = CreateConstant32Internal(31);
	public static readonly Operand Constant32_32 = CreateConstant32Internal(32);
	public static readonly Operand Constant32_64 = CreateConstant32Internal(64);
	public static readonly Operand Constant32_FF = CreateConstant32Internal(0xFF);
	public static readonly Operand Constant32_FFFF = CreateConstant32Internal(0xFFFF);
	public static readonly Operand Constant32_FFFFFFFF = CreateConstant32Internal(0xFFFFFFFF);

	public static readonly Operand Constant32_0b1000 = Constant32_8;
	public static readonly Operand Constant32_0b1001 = CreateConstant32Internal(0b1001);
	public static readonly Operand Constant32_0b1010 = CreateConstant32Internal(0b1010);
	public static readonly Operand Constant32_0b1011 = CreateConstant32Internal(0b1011);
	public static readonly Operand Constant32_0b1100 = CreateConstant32Internal(0b1100);
	public static readonly Operand Constant32_0b1101 = CreateConstant32Internal(0b1101);
	public static readonly Operand Constant32_0b1110 = CreateConstant32Internal(0b1110);
	public static readonly Operand Constant32_0b1111 = Constant32_15;

	public static readonly Operand Constant32_TILDE_FF = CreateConstant32Internal(~(uint)0xFF);
	public static readonly Operand Constant32_TILDE_FFFF = CreateConstant32Internal(~(uint)0xFFFF);

	public static readonly Operand Constant64_0 = CreateConstant64Internal(0);
	public static readonly Operand Constant64_1 = CreateConstant64Internal(1);
	public static readonly Operand Constant64_2 = CreateConstant64Internal(2);
	public static readonly Operand Constant64_3 = CreateConstant64Internal(3);
	public static readonly Operand Constant64_4 = CreateConstant64Internal(4);
	public static readonly Operand Constant64_8 = CreateConstant64Internal(8);
	public static readonly Operand Constant64_15 = CreateConstant64Internal(15);
	public static readonly Operand Constant64_16 = CreateConstant64Internal(16);
	public static readonly Operand Constant64_24 = CreateConstant64Internal(24);
	public static readonly Operand Constant64_31 = CreateConstant64Internal(31);
	public static readonly Operand Constant64_32 = CreateConstant64Internal(32);
	public static readonly Operand Constant64_64 = CreateConstant64Internal(64);
	public static readonly Operand Constant64_FF = CreateConstant64Internal(0xFF);
	public static readonly Operand Constant64_FFFF = CreateConstant64Internal(0xFFFF);
	public static readonly Operand Constant64_FFFFFFFF = CreateConstant64Internal(0xFFFFFFFF);

	public static readonly Operand ConstantR4_0 = CreateConstantR4Internal(0f);
	public static readonly Operand ConstantR4_1 = CreateConstantR4Internal(1f);

	public static readonly Operand ConstantR8_0 = CreateConstantR8Internal(0d);
	public static readonly Operand ConstantR8_1 = CreateConstantR8Internal(1d);

	#endregion Static Constants

	#region Construction

	private Operand()
	{
		Definitions = new List<InstructionNode>();
		Uses = new List<InstructionNode>();
		Constant = ConstantType.Default;

		IsResolved = true;
	}

	#endregion Construction

	#region Factory Methods - Constants

	private static Operand CreateConstant32Internal(uint value)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.Int32,
			Constant = ConstantType.Default,
			ConstantUnsigned32 = value,
		};
	}

	public static Operand CreateConstant32(uint value)
	{
		switch (value)
		{
			case 0: return Constant32_0;
			case 1: return Constant32_1;
			case 2: return Constant32_2;
			case 4: return Constant32_4;
			case 8: return Constant32_8;
			case 15: return Constant32_15;
			case 16: return Constant32_16;
			case 24: return Constant32_24;
			case 31: return Constant32_31;
			case 32: return Constant32_32;
			case 64: return Constant32_64;
			case 0xFF: return Constant32_FF;
			case 0xFFFF: return Constant32_FFFF;
			case 0xFFFFFFFF: return Constant32_FFFFFFFF;
			case 0b1001: return Constant32_0b1001;
			case 0b1010: return Constant32_0b1010;
			case 0b1011: return Constant32_0b1011;
			case 0b1100: return Constant32_0b1100;
			case 0b1101: return Constant32_0b1101;
			case 0b1110: return Constant32_0b1110;
			default: break;
		}

		return CreateConstant32Internal(value);
	}

	public static Operand CreateConstant32(int value)
	{
		return CreateConstant32((uint)value);
	}

	private static Operand CreateConstant64Internal(ulong value)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.Int64,
			Constant = ConstantType.Default,
			ConstantUnsigned64 = value,
		};
	}

	public static Operand CreateConstant64(ulong value)
	{
		switch (value)
		{
			case 0: return Constant64_0;
			case 1: return Constant64_1;
			case 2: return Constant64_2;
			case 4: return Constant64_4;
			case 8: return Constant64_8;
			case 15: return Constant64_15;
			case 16: return Constant64_16;
			case 24: return Constant64_24;
			case 31: return Constant64_31;
			case 32: return Constant64_32;
			case 64: return Constant64_64;
			case 0xFF: return Constant64_FF;
			case 0xFFFF: return Constant64_FFFF;
			case 0xFFFFFFFF: return Constant64_FFFFFFFF;
			default: break;
		}

		return CreateConstant64Internal(value);
	}

	public static Operand CreateConstant64(long value)
	{
		return CreateConstant64((ulong)value);
	}

	private static Operand CreateConstantR4Internal(float value)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.R4,
			Constant = ConstantType.Default,
			ConstantFloat = value,
		};
	}

	public static Operand CreateConstantR4(float value)
	{
		switch (value)
		{
			case 0: return ConstantR4_0;
			case 1: return ConstantR4_1;
			default: break;
		}

		return CreateConstantR4Internal(value);
	}

	private static Operand CreateConstantR8Internal(double value)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.R8,
			Constant = ConstantType.Default,
			ConstantDouble = value,
		};
	}

	public static Operand CreateConstantR8(double value)
	{
		switch (value)
		{
			case 0: return ConstantR8_0;
			case 1: return ConstantR8_1;
			default: break;
		}

		return CreateConstantR8Internal(value);
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

	#region Factory Methods - Operands

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

	#endregion Factory Methods - Operands

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
			IsResolved = false,
			Type = primitiveType == PrimitiveType.ValueType ? type : null,
		};
	}

	public static Operand CreateStackParameter(PrimitiveType primitiveType, ElementType elementType, int index, string name, int offset, uint size, MosaType type = null)
	{
		return new Operand
		{
			Primitive = primitiveType,
			Location = LocationType.StackParameter,
			Constant = ConstantType.Default,
			Element = elementType,
			Index = index,
			Size = size,
			Name = name,
			ConstantSigned64 = offset,
			Type = type,
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

	#endregion Factory Methods - Standard

	#region Factory Methods - Long Operand

	public static Operand CreateHigh(Operand longOperand, int index = 0)
	{
		Debug.Assert(longOperand.Parent == null || longOperand.Parent == longOperand);
		Debug.Assert(longOperand.High == null);

		Operand operand = null;

		if (longOperand.IsParameter)
		{
			operand = new Operand
			{
				Primitive = longOperand.Primitive,
				Location = LocationType.StackParameter,
				Constant = ConstantType.Default,
				Element = longOperand.Element,
				Index = longOperand.Index,
				Size = 0,
				Name = $"{longOperand.Name} (High)",
				ConstantSigned64 = longOperand.Offset + 4,
				Type = longOperand.Type,
				Parent = longOperand
			};
		}
		else if (longOperand.IsVirtualRegister)
		{
			operand = new Operand
			{
				Location = LocationType.VirtualRegister,
				Primitive = PrimitiveType.Int32,
				Index = index,
				Parent = longOperand
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
				Parent = longOperand
			};
		}
		else if (longOperand.IsResolvedConstant)
		{
			operand = CreateConstant32((uint)(longOperand.ConstantUnsigned64 >> 32) & uint.MaxValue);
			operand.Parent = longOperand;
		}

		Debug.Assert(operand != null);

		longOperand.High = operand;

		return operand;
	}

	public static Operand CreateLow(Operand longOperand, int index = 0)
	{
		Debug.Assert(longOperand.Parent == null);
		Debug.Assert(longOperand.Low == null);

		Operand operand = null;

		if (longOperand.IsParameter)
		{
			operand = new Operand
			{
				Primitive = longOperand.Primitive,
				Location = LocationType.StackParameter,
				Constant = ConstantType.Default,
				Element = longOperand.Element,
				Index = longOperand.Index,
				Size = 0,
				Name = $"{longOperand.Name} (Low)",
				ConstantSigned64 = longOperand.Offset,
				Type = longOperand.Type,
				Parent = longOperand
			};
		}
		else if (longOperand.IsVirtualRegister)
		{
			operand = new Operand
			{
				Location = LocationType.VirtualRegister,
				Primitive = PrimitiveType.Int32,
				Index = index,
				Parent = longOperand
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
				IsPinned = longOperand.IsPinned,
				Parent = longOperand
			};
		}
		else if (longOperand.IsResolvedConstant)
		{
			operand = CreateConstant32((uint)longOperand.ConstantUnsigned64);
			operand.Parent = longOperand;
		}

		Debug.Assert(operand != null);

		longOperand.Low = operand;

		return operand;
	}

	#endregion Factory Methods - Long Operand

	#region Factory Methods - Labels

	public static Operand CreateStringLabel(string name, uint offset, string data)
	{
		return new Operand
		{
			Location = LocationType.Constant,
			Primitive = PrimitiveType.Object,
			Constant = ConstantType.Label,
			Name = name,
			IsResolved = false,
			Offset = offset,
			StringData = data,
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
			IsResolved = false,
			Offset = 0,
			Field = field,
		};
	}

	#endregion Factory Methods - Labels

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
			if (IsLabel)
			{
				sb.Append($"label={Name}");
			}
			else
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

				if (IsParameter)
				{
					if (!HasParent)
					{
						sb.Append($" (p{Index})");
					}
					else
					{
						sb.Append($" (p{Index}<t{Parent.Index}{(IsHigh ? "H" : "L")}>)");
					}
				}
				else if (IsLocalStack)
				{
					if (!HasParent)
					{
						sb.Append($" (t{Index})");
					}
					else
					{
						sb.Append($" (t{Index}<t{Parent.Index}{(IsHigh ? "H" : "L")}>)");
					}
				}
			}
		}
		else if (IsCPURegister)
		{
			sb.Append($"{Register}");
		}
		else if (IsVirtualRegister)
		{
			if (!HasParent)
			{
				sb.Append($"v{Index}");
			}
			else
			{
				sb.Append($"(v{Index}<v{Parent.Index}{(IsHigh ? "H" : "L")}>)");
			}
		}
		else if (IsStaticField)
		{
			sb.Append($" ({Field.FullName})");
		}

		sb.Append($" [{GetElementString()}]");

		if (Primitive == PrimitiveType.ValueType && Type != null)
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
