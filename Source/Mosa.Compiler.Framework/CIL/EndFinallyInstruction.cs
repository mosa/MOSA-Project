﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class EndFinallyInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="EndFinallyInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public EndFinallyInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		public override FlowControl FlowControl { get { return FlowControl.EndFinally; } }

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Endfinally(context);
		}

		#endregion Methods
	}
}