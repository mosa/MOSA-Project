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
	/// Represents a unary instruction, which performs an operation on the operand and places
	/// the result on the stack.
	/// </summary>
	public class UnaryArithmeticInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryArithmeticInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public UnaryArithmeticInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion // Construction

		#region Methods Overrides

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(ref InstructionData instruction, IMethodCompiler compiler)
		{
			base.Validate(ref instruction, compiler);

			// Simple result is the same type as the unary argument
			instruction.Result = compiler.CreateTemporary(instruction.Operand1.Type);
		}


		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor vistor, Context context)
		{
			vistor.UnaryArithmetic(context);
		}

		#endregion // Methods Overrides
	}
}
