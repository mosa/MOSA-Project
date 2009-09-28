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
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR2
{
	/// <summary>
	/// An abstract intermediate representation of the method epilogue.
	/// </summary>
	/// <remarks>
	/// This instruction is usually derived by the architecture and expanded appropriately
	/// for the calling convention of the method.
	/// </remarks>
	public class EpilogueInstruction : BaseInstruction
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="EpilogueInstruction"/>.
		/// </summary>
		public EpilogueInstruction() :
			base(0, 0)
		{
		}

		#endregion // Construction

		#region Instruction Overrides

		/// <summary>
		/// Returns a string representation of the <see cref="EpilogueInstruction"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context context)
		{
			return @"IR.epilogue";
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.EpilogueInstruction(context);
		}

		#endregion // Instruction Overrides
	}
}
