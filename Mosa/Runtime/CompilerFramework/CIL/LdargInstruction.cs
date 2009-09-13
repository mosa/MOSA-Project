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
	public class LdargInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdargInstruction"/> class.
		/// </summary>
		public LdargInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion // Construction

		#region ICILInstruction Overrides

		/// <summary>
		/// Decodes the specified CIL instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="opcode">The opcode of the load.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		/// <remarks>
		/// This method is used by instructions to retrieve immediate operands
		/// From the instruction stream.
		/// </remarks>
		public override void Decode(ref InstructionData instruction, OpCode opcode, IInstructionDecoder decoder)
		{
			ushort argIdx;

			// Opcode specific handling
			switch (opcode) {
				case OpCode.Ldarg:
					decoder.Decode(out argIdx);
					break;

				case OpCode.Ldarg_s: {
						byte arg;
						decoder.Decode(out arg);
						argIdx = arg;
					}
					break;

				case OpCode.Ldarg_0:
					argIdx = 0;
					break;

				case OpCode.Ldarg_1:
					argIdx = 1;
					break;

				case OpCode.Ldarg_2:
					argIdx = 2;
					break;

				case OpCode.Ldarg_3:
					argIdx = 3;
					break;

				default:
					throw new NotImplementedException();
			}

			// Push the loaded value onto the evaluation stack
			instruction.Result = decoder.Compiler.GetParameterOperand(argIdx);
			instruction.Ignore = true;
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
			return "CIL Ldarg";
		}

		#endregion // Operand Overrides
	}
}
