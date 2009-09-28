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

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="codeStream">The code stream.</param>
		public override void Emit(Context ctx, System.IO.Stream codeStream)
		{
			codeStream.WriteByte(0x90);
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
