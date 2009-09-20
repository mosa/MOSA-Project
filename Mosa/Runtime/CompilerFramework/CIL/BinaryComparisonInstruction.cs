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
	public class BinaryComparisonInstruction : BinaryInstruction
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
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, decoder);

			// Set the result
			instruction.Result = decoder.Compiler.CreateTemporary(new SigType(CilElementType.I4));
		}

		/// <summary>
		/// Returns a formatted representation of the opcode.
		/// </summary>
		/// <returns>The code as a string value.</returns>
		public override string ToString(ref InstructionData instruction)
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
				result = String.Format(@"{4} ; {0} = unchecked({1} {2} {3})", instruction.Result, instruction.Operand1, op, instruction.Operand2, base.ToString());
			else
				result = String.Format(@"{4} ; {0} = ({1} {2} {3})", instruction.Result, instruction.Operand1, op, instruction.Operand2, base.ToString());

			return result;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(CILVisitor vistor, Context context)
		{
			vistor.BinaryComparison(context);
		}

		#endregion Methods

	}
}
