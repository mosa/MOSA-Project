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

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class ArithmeticOverflowInstruction : BinaryInstruction
	{
		#region Static data members

		/// <summary>
		/// Generic operand validation table. Not used for add and sub.
		/// </summary>
		private static StackTypeCode[][] _operandTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

		/// <summary>
		/// Operand validation table for the add instruction.
		/// </summary>
		private static StackTypeCode[][] _addovfunTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

		/// <summary>
		/// Operand validation table for the sub instruction.
		/// </summary>
		private static StackTypeCode[][] _subovfunTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

		#endregion // Static data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticOverflowInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ArithmeticOverflowInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(CILVisitor vistor, Context context)
		{
			vistor.ArithmeticOverflow(context);
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(ref InstructionData instruction, IMethodCompiler compiler)
		{
			base.Validate(ref instruction, compiler);

			StackTypeCode result = StackTypeCode.Unknown;
			switch (_opcode) {
				case OpCode.Add_ovf_un:
					result = _addovfunTable[(int)instruction.Operand1.StackType][(int)instruction.Operand2.StackType];
					break;

				case OpCode.Sub_ovf_un:
					result = _subovfunTable[(int)instruction.Operand1.StackType][(int)instruction.Operand2.StackType];
					break;

				default:
					result = _operandTable[(int)instruction.Operand1.StackType][(int)instruction.Operand2.StackType];
					break;
			}

			if (StackTypeCode.Unknown == result)
				throw new InvalidOperationException(@"Invalid operand types passed to " + _opcode);

			instruction.Result = compiler.CreateTemporary(Operand.SigTypeFromStackType(result));
		}

		#endregion Methods

	}
}
