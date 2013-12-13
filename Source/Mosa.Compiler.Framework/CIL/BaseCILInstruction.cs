/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Common;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public abstract class BaseCILInstruction : BaseInstruction
	{
		#region Data members

		/// <summary>
		/// Holds the CIL opcode
		/// </summary>
		protected readonly OpCode opcode;

		#endregion Data members

		#region Properties

		/// <summary>
		/// Gets the op code.
		/// </summary>
		/// <value>The op code.</value>
		public OpCode OpCode { get { return opcode; } }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		public BaseCILInstruction(OpCode opCode, byte operandCount)
			: base(0, operandCount)
		{
			this.opcode = opCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public BaseCILInstruction(OpCode opCode, byte operandCount, byte resultCount)
			: base(resultCount, operandCount)
		{
			this.opcode = opCode;
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public virtual void Decode(Context ctx, IInstructionDecoder decoder)
		{
			ctx.SetInstruction(this, DefaultOperandCount, DefaultResultCount);
		}

		/// <summary>
		/// Determines if the IL decoder pushes the results of this instruction onto the IL operand stack.
		/// </summary>
		/// <value><c>true</c> if [push result]; otherwise, <c>false</c>.</value>
		public virtual bool PushResult
		{
			get { return true; }
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public virtual void Visit(ICILVisitor visitor, Context context)
		{
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IVisitor visitor, Context context)
		{
			if (visitor is ICILVisitor)
				Visit(visitor as ICILVisitor, context);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A string representation of the operand.
		/// </returns>
		public override string ToString()
		{
			string code = opcode.ToString();
			int index = code.IndexOf("Instruction");

			if (index > 0)
				code = code.Substring(0, index);

			return "CIL." + code;
		}

		#endregion Methods

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
				//case CilElementType.Var: return StackTypeCode.O;
				default: throw new InvalidCompilerException(String.Format("Can't transform SigType of CilElementType.{0} to StackTypeCode.", type.Type));
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
				default: throw new InvalidCompilerException("Can't convert stack type code to SigType.");
			}
		}

		#endregion Static Methods
	}
}