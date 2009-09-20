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
	public class DupInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NopInstruction"/> class.
		/// </summary>
		public DupInstruction(OpCode opcode)
			: base(opcode,2)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Validates the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(ref InstructionData instruction, IMethodCompiler compiler)
		{
			base.Validate(ref instruction, compiler);

			instruction.Result = instruction.Operand1;
			instruction.Result2 = instruction.Operand1;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor vistor, Context context)
		{
			vistor.Dup(context);
		}

		#endregion Methods

	}
}
