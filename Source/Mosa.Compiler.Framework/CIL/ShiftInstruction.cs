/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Intermediate representation for IL shift instructions.
	/// </summary>
	public sealed class ShiftInstruction : BinaryInstruction
	{
		#region Operand Table

		/// <summary>
		/// This operand table conforms to ISO/IEC 23271:2006 (E), Partition III, §1.5, Table 6.
		/// </summary>
		private static readonly StackTypeCode[][] operandTable = new StackTypeCode[][] {
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
		};

		#endregion Operand Table

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ShiftInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ShiftInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Resolve(Context ctx, BaseMethodCompiler compiler)
		{
			base.Resolve(ctx, compiler);

            var stackTypeForOperand1 = TypeSystem.GetStackType(ctx.Operand1.Type);
            var stackTypeForOperand2 = TypeSystem.GetStackType(ctx.Operand2.Type);

            if (ctx.Operand1.Type.IsEnum)
            {
                stackTypeForOperand1 = TypeSystem.GetStackType(ctx.Operand1.Type.Fields[0].Type);
            }

            if (ctx.Operand2.Type.IsEnum)
            {
                stackTypeForOperand2 = TypeSystem.GetStackType(ctx.Operand2.Type.Fields[0].Type);
            }

            var result = operandTable[(int)stackTypeForOperand1][(int)stackTypeForOperand2];

			Debug.Assert(StackTypeCode.Unknown != result, @"Can't shift with the given virtualLocal operands.");
			if (StackTypeCode.Unknown == result)
				throw new InvalidOperationException(@"Invalid virtualLocal state for pairing (" + TypeSystem.GetStackType(ctx.Operand1.Type) + ", " + TypeSystem.GetStackType(ctx.Operand2.Type) + ")");

			ctx.Result = compiler.CreateVirtualRegister(compiler.TypeSystem.GetType(result));
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Shift(context);
		}

		#endregion Methods
	}
}