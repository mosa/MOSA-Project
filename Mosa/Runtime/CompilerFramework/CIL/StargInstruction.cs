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
	public class StargInstruction : StoreInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StargInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StargInstruction(OpCode opcode)
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
			// Decode the base first
			base.Decode(ref instruction, decoder);

			ushort argIdx;

			// Opcode specific handling 
			if (_opcode == OpCode.Starg_s) {
				byte arg;
				decoder.Decode(out arg);
				argIdx = arg;
			}
			else {
				decoder.Decode(out argIdx);
			}

			// The argument is the result
			instruction.Result = decoder.Compiler.GetParameterOperand(argIdx);

			// FIXME: Do some type compatibility checks
			// See verification for this instruction and
			// verification types.
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor vistor, Context context)
		{
			vistor.Starg(context);
		}

		#endregion Methods

	}
}
