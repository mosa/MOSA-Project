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
	public class DirectMultiplicationInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectMultiplicationInstruction"/> class.
		/// </summary>
		public DirectMultiplicationInstruction()
		{
		}

		#endregion // Construction

		#region IPlatformInstruction Overrides

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="codeStream">The code stream.</param>
		public override void Emit(ref InstructionData instruction, System.IO.Stream codeStream)
		{
			codeStream.WriteByte(0x90);
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
			return String.Format(@"x86.mul edx:eax, {0} ; edx:eax = eax * {0}", instruction.Operand1.ToString());
		}

		#endregion // IPlatformInstruction Overrides

	}
}
