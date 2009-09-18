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
	public class NegInstruction : UnaryArithmeticInstruction
	{
		#region Data members

		/// <summary>
		/// Holds the typecode validation table From ISO/IEC 23271:2006 (E),
		/// Partition III, §1.5, Table 3.
		/// </summary>
		private static StackTypeCode[] _typeCodes = new StackTypeCode[] {
			StackTypeCode.Unknown,
			StackTypeCode.Int32,
			StackTypeCode.Int64,
			StackTypeCode.N,
			StackTypeCode.Unknown,
			StackTypeCode.Unknown,
			StackTypeCode.Unknown
		};

		#endregion // Data members
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NegInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public NegInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(ref InstructionData instruction, IMethodCompiler compiler)
		{
			base.Validate(ref instruction, compiler);

			// Validate the operand
			StackTypeCode result = _typeCodes[(int)instruction.Operand1.StackType];
			if (StackTypeCode.Unknown == result)
				throw new InvalidOperationException(@"Invalid operand to Neg instruction.");

			instruction.Result = compiler.CreateTemporary(instruction.Operand1.Type);
		}

		#endregion Methods

	}
}
