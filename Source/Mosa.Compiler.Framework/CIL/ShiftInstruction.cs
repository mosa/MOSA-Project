/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
		private static readonly StackTypeCode[][] _operandTable = new StackTypeCode[][] {
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
		};

		#endregion // Operand Table

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ShiftInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ShiftInstruction(OpCode opcode)
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

			StackTypeCode result = _operandTable[(int)ctx.Operand1.StackType][(int)ctx.Operand2.StackType];
			Debug.Assert(StackTypeCode.Unknown != result, @"Can't shift with the given stack operands.");
			if (StackTypeCode.Unknown == result)
				throw new InvalidOperationException(@"Invalid stack state for pairing (" + ctx.Operand1.StackType + ", " + ctx.Operand2.StackType + ")");

			ctx.Result = compiler.CreateVirtualRegister(Operand.SigTypeFromStackType(result));
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
