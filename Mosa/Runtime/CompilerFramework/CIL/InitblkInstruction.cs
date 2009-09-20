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
	public class InitblkInstruction : NaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InitblkInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public InitblkInstruction(OpCode opcode)
			: base(opcode, 3)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(CILVisitor vistor, Context context)
		{
			vistor.Initblk(context);
		}

		#endregion // Methods

	}
}
