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
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class AddInstruction : BaseInstruction
	{

		#region Data Members

		private static OpCode R_C = new OpCode(new byte[] { 0x81 }, 0);
		private static OpCode R_R = new OpCode(new byte[] { 0x03 });
		private static OpCode R_M = new OpCode(new byte[] { 0x03 });
		private static OpCode M_R = new OpCode(new byte[] { 0x01 });
		private static OpCode R_M_U8 = new OpCode(new byte[] { 0x02 }); // Add r/m8 to r8
		private static OpCode M_R_U8 = new OpCode(new byte[] { 0x00 }); // Add r8 to r/m8

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AddInstruction"/> class.
		/// </summary>
		public AddInstruction()
		{
		}

		#endregion // Construction

		#region Methods

		private static OpCode Add(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return R_C;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return R_R;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				/*if (IsUByte(dest))
					return R_M_U8;
				else*/
				return R_M;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				/*if (IsByte(dest) || IsByte(src))
					return M_R_U8;
				else if (IsChar(dest) || IsChar(src) || IsShort(dest) || IsShort(src))
					return M_R_U8;
				else*/
				return M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="codeStream">The code stream.</param>
		public override void Emit(ref InstructionData instruction, System.IO.Stream codeStream)
		{
			OpCode opcode = Add(instruction.Result, instruction.Operand1);
			
			// TODO: Emit it to codeStream
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
			return String.Format(@"x86.add {0}, {1} ; {0} += {1}", instruction.Operand1, instruction.Operand2);
		}

		#endregion // Methods

	}
}
