/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR2 = Mosa.Runtime.CompilerFramework.IR2;
using Mosa.Runtime.Metadata;
using System.Diagnostics;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Intermediate representation of the div instruction.
	/// </summary>
	public sealed class DirectDivisionInstruction : OneOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DivInstruction"/> class.
		/// </summary>
		public DirectDivisionInstruction() :
			base()
		{
		}

		#endregion // Construction

		#region OneOperandInstruction Overrides

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
		public override string ToString(Context context)
		{
			return String.Format(@"x86 idiv {0} ; edx:eax /= {0}", context.Operand1);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.DirectDivision(context);
		}

		#endregion // Methods
	}
}
