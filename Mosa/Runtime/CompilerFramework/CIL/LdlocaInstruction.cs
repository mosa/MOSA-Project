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
	public class LdlocaInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdlocaInstruction"/> class.
		/// </summary>
		public LdlocaInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion // Construction

		#region ICILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="opcode">The opcode of the load.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, OpCode opcode, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, opcode, decoder);

			ushort locIdx;

			// Opcode specific handling 
			if (opcode == OpCode.Ldloca_s) {
				byte loc;
				decoder.Decode(out loc);
				locIdx = loc;
			}
			else {
				decoder.Decode(out locIdx);
			}

			instruction.Operand1  = decoder.Compiler.GetLocalOperand(locIdx);
			instruction.Result = decoder.Compiler.CreateTemporary(new RefSigType(instruction.Operand1.Type));
		}

		#endregion // ICILInstruction Overrides

	}
}
