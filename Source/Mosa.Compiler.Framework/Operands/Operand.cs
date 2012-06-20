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
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Operands
{
	/// <summary>
	/// Operand class
	/// </summary>
	public class Operand
	{

		#region Constants

		public static Operand I4_0 = CreateConstant(BuiltInSigType.Int32, (int)0);
		public static Operand I4_1 = CreateConstant(BuiltInSigType.Int32, (int)1);
		public static Operand I4_2 = CreateConstant(BuiltInSigType.Int32, (int)2);
		public static Operand I4_3 = CreateConstant(BuiltInSigType.Int32, (int)3);
		public static Operand I4_4 = CreateConstant(BuiltInSigType.Int32, (int)4);
		public static Operand I4_5 = CreateConstant(BuiltInSigType.Int32, (int)5);
		public static Operand I4_6 = CreateConstant(BuiltInSigType.Int32, (int)6);
		public static Operand I4_7 = CreateConstant(BuiltInSigType.Int32, (int)7);
		public static Operand I4_8 = CreateConstant(BuiltInSigType.Int32, (int)8);
		public static Operand I4_16 = CreateConstant(BuiltInSigType.Int32, (int)16);
		public static Operand I4_32 = CreateConstant(BuiltInSigType.Int32, (int)32);
		public static Operand I4_64 = CreateConstant(BuiltInSigType.Int32, (int)64);
		public static Operand I4_N1 = CreateConstant(BuiltInSigType.Int32, (int)-1);

		public static Operand U1_0 = CreateConstant(BuiltInSigType.Byte, 0);
		public static Operand U1_1 = CreateConstant(BuiltInSigType.Byte, 1);

		public static Operand U4_0 = CreateConstant(BuiltInSigType.UInt32, (int)0);
		public static Operand U4_0xFFFFFFFF = CreateConstant(BuiltInSigType.UInt32, (uint)(0xFFFFFFFF));
		public static Operand Obj_Null = CreateConstant(BuiltInSigType.Object, null);

		#endregion

		#region Data members

		[Flags]
		protected enum OperandType { Undefined = 0, Constant = 1, StackLocal = 2, Parameter = 4, LocalVariable = 8, Symbol = 16, Register = 32, CPURegister = 64, SSA = 128, RuntimeMember = 256, Memory = 512, VirtualRegister = 1024 };

		/// <summary>
		/// 
		/// </summary>
		private readonly OperandType operandType;

		/// <summary>
		/// The namespace of the operand.
		/// </summary>
		protected readonly SigType sigType;

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
		protected string name;

		/// <summary>
		/// Holds the index
		/// </summary>
		private int index;

		/// <summary>
		/// The register, where the operand is stored.
		/// </summary>
		private Register register;

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
		/// Determines if the operand is a register.
		/// </summary>
		public virtual bool IsRegister { get { return (operandType & OperandType.Register) == OperandType.Register; } }

		/// <summary>
		/// Determines if the operand is a stack local variable.
		/// </summary>
		public virtual bool IsStackLocal { get { return false; } }

		/// <summary>
		/// Determines if the operand is a constant variable.
		/// </summary>
		public virtual bool IsConstant { get { return (operandType & OperandType.Constant) == OperandType.Constant; } }

		/// <summary>
		/// Determines if the operand is a symbol operand.
		/// </summary>
		public virtual bool IsSymbol { get { return (operandType & OperandType.Symbol) == OperandType.Symbol; } }

		/// <summary>
		/// Determines if the operand is a virtual register operand.
		/// </summary>
		public virtual bool IsVirtualRegister { get { return (operandType & OperandType.VirtualRegister) == OperandType.VirtualRegister; } }

		/// <summary>
		/// Determines if the operand is a cpu register operand.
		/// </summary>
		public virtual bool IsCPURegister { get { return (operandType & OperandType.CPURegister) == OperandType.CPURegister; } }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return this.name; }
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
		protected Operand(SigType type)
		{
			this.sigType = type;
			definitions = new List<int>();
			uses = new List<int>();
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Operand"/>.
		/// </summary>
		/// <param name="type">The type of the operand.</param>
		protected Operand(SigType type, OperandType operandType)
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
			switch (value)
			{
				case 0: return I4_0;
				case 1: return I4_1;
				case 2: return I4_2;
				case 3: return I4_3;
				case 4: return I4_4;
				case 5: return I4_5;
				case 6: return I4_6;
				case 7: return I4_7;
				case 8: return I4_8;
				case 16: return I4_16;
				case 32: return I4_32;
				case 64: return I4_64;
				case -1: return I4_N1;
				default: return CreateConstant(BuiltInSigType.Int32, value);
			}
		}

		/// <summary>
		/// Gets the null constant <see cref="Operand"/>.
		/// </summary>
		/// <returns></returns>
		public static Operand GetNull()
		{
			return Obj_Null;
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
			return CreateSymbol(BuiltInSigType.IntPtr, method.ToString());
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
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateCPURegister(SigType sigType, Register register)
		{
			Operand operand = new Operand(sigType, OperandType.Register | OperandType.CPURegister);
			operand.register = register;
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
			if (IsConstant)
			{
				if (value == null)
					return String.Format("const null [{0}]", sigType);
				else
					return String.Format("const {0} [{1}]", value, sigType);
			}
			if (IsSymbol)
			{
				return String.Format("{0} [{1}]", name, sigType);
			}
			if (IsVirtualRegister)
			{
				return String.Format("V_{0} [{1}]", index, sigType);
			}
			if (IsCPURegister)
			{
				return String.Format("{0} [{1}]", register, sigType);
			}

			return String.Format("[{0}]", sigType);
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
			SigType result = null;
			switch (typeCode)
			{
				case StackTypeCode.Int32: result = BuiltInSigType.Int32; break;
				case StackTypeCode.Int64: result = BuiltInSigType.Int64; break;
				case StackTypeCode.F: result = BuiltInSigType.Double; break;
				case StackTypeCode.O: result = BuiltInSigType.Object; break;
				case StackTypeCode.N: result = BuiltInSigType.IntPtr; break;
				default:
					throw new NotSupportedException(@"Can't convert stack type code to SigType.");
			}
			return result;
		}

		#endregion // Static Methods

	}
}

