/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Operand class
	/// </summary>
	public sealed class Operand
	{

		#region Data members

		[Flags]
		private enum OperandType { Undefined = 0, Constant = 1, StackLocal = 2, Parameter = 4, LocalVariable = 8, Symbol = 16, Register = 32, CPURegister = 64, SSA = 128, RuntimeMember = 256, MemoryAddress = 512, VirtualRegister = 1024, Label = 2048 };

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
		/// Constant value.
		/// </summary>
		private object value;

		/// <summary>
		/// Holds the name 
		/// </summary>
		private string name;

		/// <summary>
		/// Holds the index
		/// </summary>
		private int index;

		/// <summary>
		/// The register, where the operand is stored.
		/// </summary>
		private Register register;

		/// <summary>
		/// Holds the address offset if used together with a base register or the absolute address, if register is null.
		/// </summary>
		private IntPtr offset;

		/// <summary>
		/// Holds the runtime member.
		/// </summary>
		private RuntimeMember runtimeMember;

		/// <summary>
		/// Holds the base operand
		/// </summary>
		private Operand ssaOperand;

		/// <summary>
		/// 
		/// </summary>
		private int ssaVersion;

		#endregion // Data members

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
		public object Value { get { return value; } }

		/// <summary>
		/// Returns a list of instructions, which use this operand.
		/// </summary>
		public List<int> Uses { get { return uses; } }

		/// <summary>
		/// Retrieves the register, where the operand is located.
		/// </summary>
		public Register Register { get { return register; } }

		/// <summary>
		/// Retrieves the base register, where the operand is located.
		/// </summary>
		public Register Base { get { return register; } }

		/// <summary>
		/// Retrieves the runtime member.
		/// </summary>
		public RuntimeMember RuntimeMember { get { return runtimeMember; } }

		/// <summary>
		/// Gets the operand.
		/// </summary>
		public Operand SsaOperand { get { return ssaOperand; } }

		/// <summary>
		/// Gets the ssa version.
		/// </summary>
		public int SsaVersion { get { return ssaVersion; } }

		/// <summary>
		/// Gets the offset.
		/// </summary>
		public IntPtr Offset { get { return offset; } set { offset = value; } }

		/// <summary>
		/// Determines if the operand is a register.
		/// </summary>
		public bool IsRegister { get { return (operandType & OperandType.Register) == OperandType.Register; } }

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
		public bool IsLocalVariable { get { return (operandType & OperandType.LocalVariable) == OperandType.LocalVariable; } }

		/// <summary>
		/// Determines if the operand is a local variable operand.
		/// </summary>
		public bool IsParameter { get { return (operandType & OperandType.Parameter) == OperandType.Parameter; } }

		/// <summary>
		/// Determines if the operand is a stack temp operand. 
		/// </summary>
		public bool IsStackTemp { get { return IsStackLocal && !IsLocalVariable && !IsParameter; } }

		/// <summary>
		/// Determines if the operand is a ssa operand.
		/// </summary>
		public bool IsSSA { get { return (operandType & OperandType.SSA) == OperandType.SSA; } }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get { return this.name; } }

		/// <summary>
		/// Gets the sequence.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public int Sequence { get { return this.index; } }

		/// <summary>
		/// Gets the value as long integer.
		/// </summary>
		public long ValueAsLongInteger
		{
			get
			{
				if (value is int)
					return (long)(int)value;
				else if (value is short)
					return (long)(short)value;
				else if (value is sbyte)
					return (long)(sbyte)value;
				else if (value is long)
					return (long)value;
				else if (value is uint)
					return (long)(uint)value;
				else if (value is byte)
					return (long)(byte)value;
				else if (value is ushort)
					return (long)(ushort)value;
				else if (value is ulong)
					return (long)(ulong)value;

				throw new CompilationException("Not an integer");
			}
		}
		/// <summary>
		/// Returns the stack type of the operand.
		/// </summary>
		public StackTypeCode StackType { get { return StackTypeFromSigType(sigType); } }

		#endregion // Properties

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

		#endregion // Construction

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
			operand.value = value;
			return operand;
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
			operand.name = name;
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
			Operand operand = new Operand(sigType, OperandType.Register | OperandType.VirtualRegister);
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
			Operand operand = new Operand(sigType, OperandType.Register | OperandType.CPURegister);
			operand.register = register;
			return operand;
		}

		/// <summary>
		/// Creates a new memory address <see cref="Operand"/>.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="baseRegister">The base register.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public static Operand CreateMemoryAddress(SigType sigType, Register baseRegister, IntPtr offset)
		{
			Operand operand = new Operand(sigType, OperandType.MemoryAddress);
			operand.register = baseRegister;
			operand.offset = offset;
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
			operand.name = label;
			operand.offset = IntPtr.Zero;
			return operand;
		}

		/// <summary>
		/// Creates a new stack address <see cref="Operand"/>.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="baseRegister">The base register.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateStackLocal(SigType sigType, Register baseRegister, int index)
		{
			Operand operand = new Operand(sigType, OperandType.MemoryAddress | OperandType.StackLocal);
			operand.register = baseRegister;
			operand.index = index;
			operand.offset = new IntPtr(-index * 4);
			return operand;
		}

		/// <summary>
		/// Creates a new stack address <see cref="Operand"/>.
		/// </summary>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="baseRegister">The base register.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateStackLocalTemp(SigType sigType, Register baseRegister, int index)
		{
			Operand operand = new Operand(sigType, OperandType.MemoryAddress | OperandType.StackLocal | OperandType.VirtualRegister);
			operand.register = baseRegister;
			operand.index = index;
			operand.offset = new IntPtr(-index * 4);
			return operand;
		}
		/// <summary>
		/// Creates a new runtime member <see cref="Operand"/>.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="member">The member.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public static Operand CreateRuntimeMember(SigType type, RuntimeMember member, IntPtr offset)
		{
			Operand operand = new Operand(type, OperandType.MemoryAddress | OperandType.RuntimeMember);
			operand.offset = offset;
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
			Operand operand = new Operand(field.SignatureType, OperandType.MemoryAddress | OperandType.RuntimeMember);
			operand.offset = IntPtr.Zero;
			operand.runtimeMember = field;
			return operand;
		}

		/// <summary>
		/// Creates a new local variable <see cref="Operand"/>.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="register">The register.</param>
		/// <param name="index">The index.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static Operand CreateLocalVariable(SigType type, Register register, int index, string name)
		{
			Operand operand = new Operand(type, OperandType.MemoryAddress | OperandType.StackLocal | OperandType.LocalVariable);
			operand.name = name;
			operand.register = register;
			operand.index = index;
			operand.offset = new IntPtr(-index * 4);
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
			operand.offset = new IntPtr(param.Position * 4);
			return operand;
		}

		/// <summary>
		/// Creates the SSA <see cref="Operand"/>.
		/// </summary>
		/// <param name="baseOperand">The base operand.</param>
		/// <param name="ssaVersion">The ssa version.</param>
		/// <returns></returns>
		public static Operand CreateSSA(Operand baseOperand, int ssaVersion)
		{
			Operand operand = new Operand(baseOperand.sigType, baseOperand.operandType | OperandType.SSA);
			operand.ssaOperand = baseOperand;
			operand.ssaVersion = ssaVersion;
			return operand;
		}

		#endregion // Static Factory Constructors

		#region Methods

		/// <summary>
		/// Replaces this operand in all uses and defs with the given operand.
		/// </summary>
		/// <param name="replacement">The replacement operand.</param>
		/// <param name="instructionSet">The instruction set.</param>
		public void Replace(Operand replacement, InstructionSet instructionSet)
		{

			// Iterate all definition sites first
			foreach (int index in Definitions.ToArray())
			{
				Context ctx = new Context(instructionSet, index);

				if (ctx.Result != null)
				{
					// Is this the operand?
					if (ReferenceEquals(ctx.Result, this))
					{
						ctx.Result = replacement;
					}

				}
			}

			// Iterate all use sites
			foreach (int index in Uses.ToArray())
			{
				Context ctx = new Context(instructionSet, index);

				int opIdx = 0;
				foreach (Operand r in ctx.Operands)
				{
					// Is this the operand?
					if (ReferenceEquals(r, this))
					{
						ctx.SetOperand(opIdx, replacement);
					}

					opIdx++;
				}
			}
		}

		#endregion // Methods

		#region Object Overrides

		/// <summary>
		/// Returns a string representation of <see cref="Operand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			if (IsSSA)
			{
				string ssa = ssaOperand.ToString();
				int pos = ssa.IndexOf(' ');

				if (pos < 0)
					return ssa + "<" + ssaVersion + ">";
				else
					return ssa.Substring(0, pos) + "<" + ssaVersion + ">" + ssa.Substring(pos);
			}

			StringBuilder s = new StringBuilder();

			if (name != null)
			{
				s.Append(name);
			}

			if (IsVirtualRegister)
			{
				s.AppendFormat("V_{0}", index);
			}
			else if (IsLocalVariable && name == null)
			{
				s.AppendFormat("L_{0}", index);
			}
			else if (IsStackLocal && name == null)
			{
				s.AppendFormat("T_{0}", index);
			}
			else if (IsParameter && name == null)
			{
				s.AppendFormat("P_{0}", index);
			}
			if (IsRuntimeMember)
			{
				s.Append(runtimeMember.ToString());
			}

			if (s.Length != 0)
				s.Append(' ');

			if (IsConstant)
			{
				if (value == null)
					s.Append("const null");
				else
					s.AppendFormat("const {0}", value);
			}
			else if (IsCPURegister)
			{
				s.AppendFormat("{0}", register);
			}
			else if (IsMemoryAddress)
			{
				if (register == null)
				{
					s.AppendFormat("[0x{0:X}]", offset.ToInt32());
				}
				else
				{
					if (offset.ToInt32() > 0)
						s.AppendFormat("[{0}+0x{1:X}]", register, offset.ToInt32());
					else
						s.AppendFormat("[{0}-0x{1:X}]", register, -offset.ToInt32());
				}
			}

			if (s.Length != 0)
				if (s[s.Length - 1] != ' ')
					s.Append(' ');

			s.AppendFormat("[{0}]", sigType);
			return s.ToString();
		}

		#endregion // Object Overrides

		#region Static Methods

		/// <summary>
		/// Retrieves the stack type from a sig type.
		/// </summary>
		/// <param name="type">The signature type to convert to a stack type code.</param>
		/// <returns>The equivalent stack type code.</returns>
		public static StackTypeCode StackTypeFromSigType(SigType type)
		{
			StackTypeCode result = StackTypeCode.Unknown;
			switch (type.Type)
			{
				case CilElementType.Void:
					break;

				case CilElementType.Boolean: result = StackTypeCode.Int32; break;
				case CilElementType.Char: result = StackTypeCode.Int32; break;
				case CilElementType.I1: result = StackTypeCode.Int32; break;
				case CilElementType.U1: result = StackTypeCode.Int32; break;
				case CilElementType.I2: result = StackTypeCode.Int32; break;
				case CilElementType.U2: result = StackTypeCode.Int32; break;
				case CilElementType.I4: result = StackTypeCode.Int32; break;
				case CilElementType.U4: result = StackTypeCode.Int32; break;
				case CilElementType.I8: result = StackTypeCode.Int64; break;
				case CilElementType.U8: result = StackTypeCode.Int64; break;
				case CilElementType.R4: result = StackTypeCode.F; break;
				case CilElementType.R8: result = StackTypeCode.F; break;
				case CilElementType.I: result = StackTypeCode.N; break;
				case CilElementType.U: result = StackTypeCode.N; break;
				case CilElementType.Ptr: result = StackTypeCode.Ptr; break;
				case CilElementType.ByRef: result = StackTypeCode.Ptr; break;
				case CilElementType.Object: result = StackTypeCode.O; break;
				case CilElementType.String: result = StackTypeCode.O; break;
				case CilElementType.ValueType: result = StackTypeCode.O; break;
				case CilElementType.Type: result = StackTypeCode.O; break;
				case CilElementType.Class: result = StackTypeCode.O; break;
				case CilElementType.GenericInst: result = StackTypeCode.O; break;
				case CilElementType.Array: result = StackTypeCode.O; break;
				case CilElementType.SZArray: result = StackTypeCode.O; break;
				case CilElementType.Var: result = StackTypeCode.O; break;

				default:
					throw new NotSupportedException(String.Format(@"Can't transform SigType of CilElementType.{0} to StackTypeCode.", type.Type));
			}

			return result;
		}

		/// <summary>
		/// Sigs the type of the type From stack.
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

		#endregion // Static Methods

	}
}

