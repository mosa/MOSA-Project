/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Used to represent labelled literal data in the instruction stream.
	/// </summary>
	public abstract class LiteralInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of Literalcontext.
		/// </summary>
		protected LiteralInstruction()
		{
		}

		#endregion // Construction

		#region Instruction Overrides

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context context)
		{
			LiteralData data = (LiteralData)context.Other;
			return String.Format("IR.literal {0} {1} ; L_{2}", data.Type, data.Data, data.Label);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.LiteralInstruction(context);
		}

		#endregion // Instruction Overrides
	}
}
