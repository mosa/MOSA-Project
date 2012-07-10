/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Intermediate representation of a IL binary logic instruction.
	/// </summary>
	public sealed class BinaryLogicInstruction : BinaryInstruction
	{

		#region Operand Table

		/// <summary>
		/// Operand table according to ISO/IEC 23271:2006 (E), Partition III, 1.5, Table 5.
		/// </summary>
		private static readonly StackTypeCode[][] _opTable = new StackTypeCode[][] {
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
		};

		#endregion // Operand Table

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryLogicInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BinaryLogicInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, BaseMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

			var stackTypeForOperand1 = ctx.Operand1.StackType;
			var stackTypeForOperand2 = ctx.Operand2.StackType;

			if (ctx.Operand1.Type is ValueTypeSigType)
			{
				var op1Type = compiler.Method.Module.GetType ((ctx.Operand1.Type as ValueTypeSigType).Token);
				if (op1Type.BaseType.FullName == "System.Enum")
					stackTypeForOperand1 = this.FromSigType (op1Type.Fields[0].SignatureType.Type);
			}

			if (ctx.Operand2.Type is ValueTypeSigType)
			{
				var op2Type = compiler.Method.Module.GetType ((ctx.Operand2.Type as ValueTypeSigType).Token);
				if (op2Type.BaseType.FullName == "System.Enum")
					stackTypeForOperand2 = this.FromSigType (op2Type.Fields[0].SignatureType.Type);
			}

			var result = _opTable[(int)stackTypeForOperand1][(int)stackTypeForOperand2];

			if (result == StackTypeCode.Unknown)
				throw new InvalidOperationException (@"Invalid stack result of instruction: " + result.ToString () + " (" + ctx.Operand1.ToString () + ")");

			ctx.Result = compiler.CreateVirtualRegister(Operand.SigTypeFromStackType(result));
		}

		private StackTypeCode FromSigType (CilElementType type)
		{
			switch(type)
			{
				case CilElementType.I1: goto case CilElementType.U4;
				case CilElementType.I2: goto case CilElementType.U4;
				case CilElementType.I4: goto case CilElementType.U4;
				case CilElementType.U1: goto case CilElementType.U4;
				case CilElementType.U2: goto case CilElementType.U4;
				case CilElementType.U4: return StackTypeCode.Int32;
			}

			throw new NotSupportedException ();
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.BinaryLogic(context);
		}

		#endregion Methods

	}
}
