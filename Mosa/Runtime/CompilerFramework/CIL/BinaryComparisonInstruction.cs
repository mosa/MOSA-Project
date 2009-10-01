/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class BinaryComparisonInstruction : BinaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryComparisonInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BinaryComparisonInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction
		
		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Set the result
			ctx.Result = decoder.Compiler.CreateTemporary(new SigType(CilElementType.I4));
		}

		/// <summary>
		/// Returns a formatted representation of the opcode.
		/// </summary>
		/// <returns>The code as a string value.</returns>
		public override string ToString(Context ctx)
		{
			string result, op;
			bool un = false;
			switch (_opcode) {
				case OpCode.Ceq:
					op = @"==";
					break;

				case OpCode.Cgt:
					op = @">";
					break;

				case OpCode.Cgt_un:
					op = @">";
					un = true;
					break;

				case OpCode.Clt:
					op = @"<";
					break;

				case OpCode.Clt_un:
					op = @"<";
					un = true;
					break;

				default:
					throw new InvalidOperationException(@"Invalid opcode.");
			}

			if (un)
				result = String.Format(@"{4} ; {0} = unchecked({1} {2} {3})", ctx.Result, ctx.Operand1, op, ctx.Operand2, base.ToString());
			else
				result = String.Format(@"{4} ; {0} = ({1} {2} {3})", ctx.Result, ctx.Operand1, op, ctx.Operand2, base.ToString());

			return result;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.BinaryComparison(context);
		}

		#endregion Methods

	}
}
