/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Operand class
	/// </summary>
	public sealed class Operand
	{
		#region Data members

		[Flags]
		private enum OperandType { Undefined = 0, Constant = 1, StackLocal = 2, Parameter = 4, Symbol = 16, CPURegister = 64, SSA = 128, RuntimeMember = 256, MemoryAddress = 512, VirtualRegister = 1024, Label = 2048 };

		/// <summary>
		///
		/// </summary>
		private readonly OperandType operandType;

		/// <summary>
		/// The namespace of the operand.
		/// </summary>
		private readonly SigType sigType;

		/// <summary>
		/// Holds a list of instructions, which define this operand.
		/// </summary>
		private List<int> definitions;

		/// <summary>
		/// Holds a list of instructions, which use this operand.
		/// </summary>
		private List<int> uses;

		/// <summary>
		/// Holds the index.
		/// </summary>
		private int index;

		/// <summary>
		/// The register where the operand is stored.
		/// </summary>
		private Register register;

		/// <summary>
		/// The operand for the offset base.
		/// </summary>
		private Operand offsetBase;

		/// <summary>
		/// The operand of the parent.
		/// </summary>
		private Operand parent;

		/// <summary>
		/// Holds the runtime member.
		/// </summary>
		private RuntimeMember runtimeMember;

		#endregion Data members

		#region Properties

		/// <summary>
		/// Returns the type of the operand.
		/// </summary>
		public SigType Type { get { return sigType; } }

		/// <summary>
		/// Returns a list of instructions, which use this operand.
		/// </summary>
		public List<int> Definitions { get { return definitions; } }

		/// <summary>
		/// Returns the value of the constant.
		/// </summary>
		public object Value { get; private set; }

		/// <summary>
		/// Returns a list of instructions, which use this operand.
		/// </summary>
		public List<int> Uses { get { return uses; } }

		/// <summary>
		/// Retrieves the register, where the operand is located.
		/// </summary>
		public Register Register { get { return register; } private set { register = value; } }

		/// <summary>
		/// Retrieves the offset base.
		/// </summary>
		public Operand OffsetBase { get { return offsetBase; } private set { offsetBase = value; } }

		/// <summary>
		/// Gets the effective base.
		/// </summary>
		/// <value>
		/// The effective base.
		/// </value>
		public Register EffectiveOffsetBase { get { return offsetBase != null ? offsetBase.register : register; } }

		/// <summary>
		/// Gets the base operand.
		/// </summary>
		public Operand SSAParent { get { return IsSSA ? parent : null; } private set { parent = value; } }

		/// <summary>
		/// Holds the address offset if used together with a base register or the absolute address, if register is null.
		/// </summary>
		/// <value>
		/// The offset.
		/// </value>
		public long Offset { get; set; }

		/// <summary>
		/// Retrieves the runtime member.
		/// </summary>
		public RuntimeMember RuntimeMember { get { return runtimeMember; } }

		/// <summary>
		/// Gets the ssa version.
		/// </summary>
		public int SSAVersion { get; private set; }

		/// <summary>
		/// Gets or sets the low operand.
		/// </summary>
		/// <value>
		/// The low operand.
		/// </value>
		public Operand Low { get; private set; }

		/// <summary>
		/// Gets or sets the high operand.
		/// </summary>
		/// <value>
		/// The high operand.
		/// </value>
		public Operand High { get; private set; }

		/// <summary>
		/// Gets the split64 parent.
		/// </summary>
		/// <value>
		/// The split64 parent.
		/// </value>
		public Operand SplitParent { get { return !IsSSA ? parent : null; } private set { parent = value; } }

		/// <summary>
		/// Determines if the operand is a constant variable.
		/// </summary>
		public bool IsConstant { get { return (operandType & OperandType.Constant) == OperandType.Constant; } }

		/// <summary>
		/// Determines if the operand is a symbol operand.
		/// </summary>
		public bool IsSymbol { get { return (operandType & OperandType.Symbol) == OperandType.Symbol; } }

		/// <summary>
		/// Determines if the operand is a label operand.
		/// </summary>
		public bool IsLabel { get { return (operandType & OperandType.Label) == OperandType.Label; } }

		/// <summary>
		/// Determines if the operand is a register.
		/// </summary>
		public bool IsRegister { get { return IsVirtualRegister || IsCPURegister; } }

		/// <summary>
		/// Determines if the operand is a virtual register operand.
		/// </summary>
		public bool IsVirtualRegister { get { return (operandType & OperandType.VirtualRegister) == OperandType.VirtualRegister; } }

		/// <summary>
		/// Determines if the operand is a cpu register operand.
		/// </summary>
		public bool IsCPURegister { get { return (operandType & OperandType.CPURegister) == OperandType.CPURegister; } }

		/// <summary>
		/// Determines if the operand is a memory operand.
		/// </summary>
		public bool IsMemoryAddress { get { return (operandType & OperandType.MemoryAddress) == OperandType.MemoryAddress; } }

		/// <summary>
		/// Determines if the operand is a stack local operand.
		/// </summary>
		public bool IsStackLocal { get { return (operandType & OperandType.StackLocal) == OperandType.StackLocal; } }

		/// <summary>
		/// Determines if the operand is a runtime member operand.
		/// </summary>
		public bool IsRuntimeMember { get { return (operandType & OperandType.RuntimeMember) == OperandType.RuntimeMember; } }

		/// <summary>
		/// Determines if the operand is a local variable operand.
		/// </summary>
		public bool IsParameter { get { return (operandType & OperandType.Parameter) == OperandType.Parameter; } }

		/// <summary>
		/// Determines if the operand is a ssa operand.
		/// </summary>
		public bool IsSSA { get { return (operandType & OperandType.SSA) == OperandType.SSA; } }

		/// <summary>
		/// Gets a value indicating whether this instance is split64 child.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is split64 child; otherwise, <c>false</c>.
		/// </value>
		public bool IsSplitChild { get { return parent != null && !IsSSA; } }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the sequence.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public int Sequence { get { return this.index; } }

		/// <summary>
		/// Gets the index.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public int Index { get { return this.index; } }

		/// <summary>
		/// Gets the value as long integer.
		/// </summary>
		public long ValueAsLongInteger
		{
			get
			{
				if (Value is int)
					return (long)(int)Value;
				else if (Value is short)
					return (long)(short)Value;
				else if (Value is sbyte)
					return (long)(sbyte)Value;
				else if (Value is long)
					return (long)Value;
				else if (Value is uint)
					return (long)(uint)Value;
				else if (Value is byte)
					return (long)(byte)Value;
				else if (Value is ushort)
					return (long)(ushort)Value;
				else if (Value is ulong)
					return (long)(ulong)Value;

				else if (Value == null)
					return 0;	// REVIEW

				throw new CompilationException("Not an integer");
			}
		}

		/// <summary>
		/// Returns the stack type of the operand.
		/// </summary>
		public StackTypeCode StackType { get { return StackTypeFromSigType(sigType); } }

		/// <summary>
		/// Gets a value indicating whether this instance is floating point.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is floating point; otherwise, <c>false</c>.
		/// </value>
		public bool IsFloatingPoint { get { return (Type.Type == CilElementType.R4 || Type.Type == CilElementType.R8); } }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Operand"/>.
		/// </summary>
		/// <param name="type">The type of the operand.</param>
		private Operand(SigType type)
		{
			this.sigType = type;
			definitions = new List<int>();
			uses = new List<int>();
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Operand"/>.
		/// </summary>
		/// <param name="type">The type of the operand.</param>
		private Operand(SigType type, OperandType operandType)
		{
			this.sigType = type;
			this.operandType = operandType;
			definitions = new List<int>();
			uses = new List<int>();
		}

		#endregion Construction

		#region Static Factory Constructors

		/// <summary>
		/// Creates a new constant <see cref="Operand"/> for the given integral value.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static Operand CreateConstant(SigType sigType, object value)
		{
			Operand operand = new Operand(sigType, OperandType.Constant);
			operand.Value = value;
			return operand;
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand"/> for the given integral value.
		/// </summary>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>A new Operand representing the value <paramref name="value"/>.</returns>
		public static Operand CreateConstant(uint value)
		{
			return CreateConstant(BuiltInSigType.UInt32, value);
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand"/> for the given integral value.
		/// </summary>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>A new Operand representing the value <paramref name="value"/>.</returns>
		public static Operand CreateConstant(int value)
		{
			return CreateConstant(BuiltInSigType.Int32, value);
		}

		/// <summary>
		/// Gets the null constant <see cref="Operand"/>.
		/// </summary>
		/// <returns></returns>
		public static Operand GetNull()
		{
			return CreateConstant(BuiltInSigType.Object, null);
		}

		/// <summary>
		/// Creates a new symbol <see cref="Operand"/> for the given symbol name.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static Operand CreateSymbol(SigType sigType, string name)
		{
			Operand operand = new Operand(sigType, OperandType.Symbol);
			operand.Name = name;
			return operand;
		}

		/// <summary>
		/// Creates a new symbol <see cref="Operand"/> for the given symbol name.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		public static Operand CreateSymbolFromMethod(RuntimeMethod method)
		{
			return CreateSymbol(BuiltInSigType.IntPtr, method.FullName);
		}

		/// <summary>
		/// Creates a new virtual register <see cref="Operand"/>.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateVirtualRegister(SigType sigType, int index)
		{
			Operand operand = new Operand(sigType, OperandType.VirtualRegister);
			operand.index = index;
			return operand;
		}

		/// <summary>
		/// Creates a new virtual register <see cref="Operand" />.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="index">The index.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static Operand CreateVirtualRegister(SigType sigType, int index, string name)
		{
			Operand operand = new Operand(sigType, OperandType.VirtualRegister);
			operand.Name = name;
			operand.index = index;
			return operand;
		}

		/// <summary>
		/// Creates a new physical register <see cref="Operand"/>.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		public static Operand CreateCPURegister(SigType sigType, Register register)
		{
			Operand operand = new Operand(sigType, OperandType.CPURegister);
			operand.register = register;
			return operand;
		}

		/// <summary>
		/// Creates a new memory address <see cref="Operand"/>.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="offsetBase">The base register.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public static Operand CreateMemoryAddress(SigType sigType, Operand offsetBase, long offset)
		{
			Operand operand = new Operand(sigType, OperandType.MemoryAddress);
			operand.OffsetBase = offsetBase;
			operand.Offset = offset;
			return operand;
		}

		/// <summary>
		/// Creates a new symbol <see cref="Operand"/> for the given symbol name.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		public static Operand CreateLabel(SigType sigType, string label)
		{
			Operand operand = new Operand(sigType, OperandType.MemoryAddress | OperandType.Label);
			operand.Name = label;
			operand.Offset = 0;
			return operand;
		}

		/// <summary>
		/// Creates a new runtime member <see cref="Operand"/>.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="member">The member.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public static Operand CreateRuntimeMember(SigType type, RuntimeMember member, int offset)
		{
			Operand operand = new Operand(type, OperandType.MemoryAddress | OperandType.RuntimeMember);
			operand.Offset = offset;
			operand.runtimeMember = member;
			return operand;
		}

		/// <summary>
		/// Creates a new runtime member <see cref="Operand"/>.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		public static Operand CreateRuntimeMember(RuntimeField field)
		{
			Operand operand = new Operand(field.SigType, OperandType.MemoryAddress | OperandType.RuntimeMember);
			operand.Offset = 0;
			operand.runtimeMember = field;
			return operand;
		}

		/// <summary>
		/// Creates a new local variable <see cref="Operand"/>.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="register">The register.</param>
		/// <param name="param">The param.</param>
		/// <returns></returns>
		public static Operand CreateParameter(SigType type, Register register, RuntimeParameter param, int index)
		{
			Operand operand = new Operand(type, OperandType.MemoryAddress | OperandType.Parameter);
			operand.register = register;
			operand.index = index; // param.Position;

			//operand.sequence = index;
			operand.Offset = param.Position * 4; // FIXME: 4 is platform dependent!
			return operand;
		}

		/// <summary>
		/// Creates the stack local.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="register">The register.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateStackLocal(SigType sigType, Register register, int index)
		{
			Operand operand = new Operand(sigType, OperandType.StackLocal | OperandType.MemoryAddress);
			operand.register = register;
			operand.index = index;
			return operand;
		}

		/// <summary>
		/// Creates the SSA <see cref="Operand"/>.
		/// </summary>
		/// <param name="ssaOperand">The ssa operand.</param>
		/// <param name="ssaVersion">The ssa version.</param>
		/// <returns></returns>
		public static Operand CreateSSA(Operand ssaOperand, int ssaVersion)
		{
			Operand operand = new Operand(ssaOperand.sigType, ssaOperand.operandType | OperandType.SSA);
			operand.SSAParent = ssaOperand;
			operand.SSAVersion = ssaVersion;
			return operand;
		}

		/// <summary>
		/// Creates the low 32 bit portion of a 64-bit <see cref="Operand"/>.
		/// </summary>
		/// <param name="longOperand">The long operand.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateLowSplitForLong(Operand longOperand, int offset, int index)
		{
			Debug.Assert(longOperand.Type.Type == CilElementType.U8 || longOperand.Type.Type == CilElementType.I8);

			Debug.Assert(longOperand.SplitParent == null);
			Debug.Assert(longOperand.Low == null);

			Operand operand;

			if (longOperand.IsConstant)
			{
				operand = new Operand(BuiltInSigType.UInt32, OperandType.Constant);
				operand.Value = longOperand.ValueAsLongInteger & uint.MaxValue;
			}
			else if (longOperand.IsRuntimeMember)
			{
				operand = new Operand(BuiltInSigType.UInt32, OperandType.MemoryAddress | OperandType.RuntimeMember);
				operand.runtimeMember = longOperand.RuntimeMember;
				operand.OffsetBase = longOperand.OffsetBase;
				operand.Offset = longOperand.Offset + offset;
				operand.Register = longOperand.Register;
			}
			else if (longOperand.IsMemoryAddress)
			{
				operand = new Operand(BuiltInSigType.UInt32, OperandType.MemoryAddress);
				operand.OffsetBase = longOperand.OffsetBase;
				operand.Offset = longOperand.Offset + offset;
				operand.Register = longOperand.Register;
			}
			else
			{
				operand = new Operand(BuiltInSigType.UInt32, OperandType.VirtualRegister);
			}

			operand.parent = longOperand;

			Debug.Assert(longOperand.Low == null);
			longOperand.Low = operand;

			operand.index = index;

			//operand.sequence = index;
			return operand;
		}

		/// <summary>
		/// Creates the high 32 bit portion of a 64-bit <see cref="Operand"/>.
		/// </summary>
		/// <param name="longOperand">The long operand.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateHighSplitForLong(Operand longOperand, int offset, int index)
		{
			Debug.Assert(longOperand.Type.Type == CilElementType.U8 || longOperand.Type.Type == CilElementType.I8);

			Debug.Assert(longOperand.SplitParent == null);
			Debug.Assert(longOperand.High == null);

			Operand operand;

			if (longOperand.IsConstant)
			{
				operand = new Operand(BuiltInSigType.UInt32, OperandType.Constant);
				operand.Value = ((uint)((ulong)(longOperand.ValueAsLongInteger) >> 32)) & uint.MaxValue;
			}
			else if (longOperand.IsRuntimeMember)
			{
				operand = new Operand(BuiltInSigType.UInt32, OperandType.MemoryAddress | OperandType.RuntimeMember);
				operand.runtimeMember = longOperand.RuntimeMember;
				operand.OffsetBase = longOperand.OffsetBase;
				operand.Offset = longOperand.Offset + offset;
				operand.Register = longOperand.Register;
			}
			else if (longOperand.IsMemoryAddress)
			{
				operand = new Operand(BuiltInSigType.UInt32, OperandType.MemoryAddress);
				operand.OffsetBase = longOperand.OffsetBase;
				operand.Offset = longOperand.Offset + offset;
				operand.Register = longOperand.Register;
			}
			else
			{
				operand = new Operand(BuiltInSigType.UInt32, OperandType.VirtualRegister);
			}

			operand.parent = longOperand;

			//operand.SplitParent = longOperand;

			Debug.Assert(longOperand.High == null);
			longOperand.High = operand;

			operand.index = index;
			return operand;
		}

		#endregion Static Factory Constructors

		#region Object Overrides

		/// <summary>
		/// Returns a string representation of <see cref="Operand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			if (IsSSA)
			{
				string ssa = SSAParent.ToString();
				int pos = ssa.IndexOf(' ');

				if (pos < 0)
					return ssa + "<" + SSAVersion + ">";
				else
					return ssa.Substring(0, pos) + "<" + SSAVersion + ">" + ssa.Substring(pos);
			}

			StringBuilder s = new StringBuilder();

			if (Name != null)
			{
				s.Append(Name);
				s.Append(' ');
			}

			if (IsVirtualRegister)
			{
				s.AppendFormat("V_{0}", index);
			}
			else if (IsStackLocal && Name == null)
			{
				s.AppendFormat("T_{0}", index);
			}
			else if (IsParameter && Name == null)
			{
				s.AppendFormat("P_{0}", index);
			}

			if (IsSplitChild)
			{
				s.Append(' ');

				s.Append("(" + parent.ToString() + ")");

				if (parent.High == this)
					s.Append("/high");
				else
					s.Append("/low");
			}

			if (IsConstant)
			{
				s.Append(' ');

				if (Value == null)
					s.Append("const null");
				else
					s.AppendFormat("const {0}", Value);
			}

			if (IsRuntimeMember)
			{
				s.Append(' ');
				s.Append(runtimeMember.ToString());
			}

			if (IsCPURegister)
			{
				s.AppendFormat(" {0}", register);
			}
			else if (IsMemoryAddress)
			{
				s.Append(' ');
				if (OffsetBase != null)
				{
					if (Offset > 0)
						s.AppendFormat("[{0}+{1:X}h]", OffsetBase.ToString(), Offset);
					else
						s.AppendFormat("[{0}-{1:X}h]", OffsetBase.ToString(), -Offset);
				}
				else if (Register != null)
				{
					if (Offset > 0)
						s.AppendFormat("[{0}+{1:X}h]", Register.ToString(), Offset);
					else
						s.AppendFormat("[{0}-{1:X}h]", Register.ToString(), -Offset);
				}
				else if (IsRuntimeMember && IsSplitChild)
				{
					if (Offset > 0)
						s.AppendFormat("+{0:X}h", Offset);
					else
						s.AppendFormat("-{0:X}h", -Offset);
				}
			}

			if (sigType is PtrSigType)
			{
				s.AppendFormat(" [{0}-{1}]", sigType, (sigType as PtrSigType).ElementType);
			}
			else if (sigType is RefSigType)
			{
				s.AppendFormat(" [{0}-{1}]", sigType, (sigType as RefSigType).ElementType);
			}
			else
			{
				s.AppendFormat(" [{0}]", sigType);
			}

			return s.ToString().Replace("  ", " ").Trim();
		}

		#endregion Object Overrides

		#region Static Methods

		/// <summary>
		/// Retrieves the stack type from a sig type.
		/// </summary>
		/// <param name="type">The signature type to convert to a stack type code.</param>
		/// <returns>The equivalent stack type code.</returns>
		public static StackTypeCode StackTypeFromSigType(SigType type)
		{
			switch (type.Type)
			{
				case CilElementType.Void: return StackTypeCode.Unknown;
				case CilElementType.Boolean: return StackTypeCode.Int32;
				case CilElementType.Char: return StackTypeCode.Int32;
				case CilElementType.I1: return StackTypeCode.Int32;
				case CilElementType.U1: return StackTypeCode.Int32;
				case CilElementType.I2: return StackTypeCode.Int32;
				case CilElementType.U2: return StackTypeCode.Int32;
				case CilElementType.I4: return StackTypeCode.Int32;
				case CilElementType.U4: return StackTypeCode.Int32;
				case CilElementType.I8: return StackTypeCode.Int64;
				case CilElementType.U8: return StackTypeCode.Int64;
				case CilElementType.R4: return StackTypeCode.F;
				case CilElementType.R8: return StackTypeCode.F;
				case CilElementType.I: return StackTypeCode.N;
				case CilElementType.U: return StackTypeCode.N;
				case CilElementType.Ptr: return StackTypeCode.Ptr;
				case CilElementType.ByRef: return StackTypeCode.Ptr;
				case CilElementType.Object: return StackTypeCode.O;
				case CilElementType.String: return StackTypeCode.O;
				case CilElementType.ValueType: return StackTypeCode.O;
				case CilElementType.Type: return StackTypeCode.O;
				case CilElementType.Class: return StackTypeCode.O;
				case CilElementType.GenericInst: return StackTypeCode.O;
				case CilElementType.Array: return StackTypeCode.O;
				case CilElementType.SZArray: return StackTypeCode.O;
				case CilElementType.Var: return StackTypeCode.O;
				default:
					throw new NotSupportedException(String.Format(@"Can't transform SigType of CilElementType.{0} to StackTypeCode.", type.Type));
			}
		}

		/// <summary>
		/// Sigs the type of the type from stack.
		/// </summary>
		/// <param name="typeCode">The type code.</param>
		/// <returns></returns>
		public static SigType SigTypeFromStackType(StackTypeCode typeCode)
		{
			switch (typeCode)
			{
				case StackTypeCode.Int32: return BuiltInSigType.Int32;
				case StackTypeCode.Int64: return BuiltInSigType.Int64;
				case StackTypeCode.F: return BuiltInSigType.Double;
				case StackTypeCode.O: return BuiltInSigType.Object;
				case StackTypeCode.N: return BuiltInSigType.IntPtr;
				default:
					throw new NotSupportedException(@"Can't convert stack type code to SigType.");
			}
		}

		public static SigType NormalizeSigType(SigType type)
		{
			switch (type.Type)
			{
				case CilElementType.Boolean: return BuiltInSigType.UInt32;
				case CilElementType.Char: return BuiltInSigType.UInt32;
				case CilElementType.I1: return BuiltInSigType.Int32;
				case CilElementType.I2: return BuiltInSigType.Int32;
				case CilElementType.U1: return BuiltInSigType.UInt32;
				case CilElementType.U2: return BuiltInSigType.UInt32;
				case CilElementType.U4: return BuiltInSigType.UInt32;
				case CilElementType.U8: return BuiltInSigType.UInt64;
				//case CilElementType.R4: return BuiltInSigType.Int64;
				default: return type;
			}
		}

		#endregion Static Methods
	}
}