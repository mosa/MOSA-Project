/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.CompilerFramework;
using System.Diagnostics;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 cvtsi2sd instruction.
	/// </summary>
	public sealed class Cvtsi2sdInstruction : TwoOperandInstruction
	{

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Cvtsi2sd(context);
		}

		#endregion // Methods
	}
}
