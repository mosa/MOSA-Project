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
	public class BreakInstruction : CILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BreakInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BreakInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Method

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(CILVisitor vistor, Context context)
		{
			vistor.Break(context);
		}

		#endregion // Method
	}
}
