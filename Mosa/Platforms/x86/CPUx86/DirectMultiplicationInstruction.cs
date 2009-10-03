/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public class DirectMultiplicationInstruction : BaseInstruction
	{
		#region Methods

		/// <summary>
		/// Computes the op code.
		/// </summary>
		/// <param name="firstOperand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <param name="thirdOperand">The third operand.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand firstOperand, Operand secondOperand, Operand thirdOperand)
        {
            return new OpCode(new byte[] { 0xF7 }, 4);
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context context)
		{
			return String.Format(@"x86.mul edx:eax, {0} ; edx:eax = eax * {0}", context.Operand2);
		}

		#endregion // Methods

	}
}
