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
	public class LdargaInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdargaInstruction"/> class.
		/// </summary>
		public LdargaInstruction(OpCode opCode)
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
			Debug.Assert(OpCode.Ldarga == opcode || OpCode.Ldarga_s == opcode);
			if (OpCode.Ldarga != opcode && OpCode.Ldarga_s != opcode)
				throw new ArgumentException(@"Invalid opcode.", @"code");

			ushort argIdx;

			// Opcode specific handling 
			if (opcode == OpCode.Ldarga_s) {
				byte arg;
				decoder.Decode(out arg);
				argIdx = arg;
			}
			else {
				decoder.Decode(out argIdx);
			}

			instruction.Operand1 = decoder.Compiler.GetParameterOperand(argIdx);
			instruction.Result = decoder.Compiler.CreateTemporary(new RefSigType(instruction.Operand1.Type));
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return ToString();
		}

		#endregion // ICILInstruction Overrides

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "CIL nop";
		}

		#endregion // Operand Overrides
	}
}
