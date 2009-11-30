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
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Used in the single static assignment form of the instruction stream to
	/// automatically select the appropriate value of a variable depending on the
	/// incoming edge.
	/// </summary>
	public sealed class PhiInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of Phicontext.
		/// </summary>
		public PhiInstruction()
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.PhiInstruction(context);
		}

		/// <summary>
		/// Determines whether [contains] [the specified CTX].
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified CTX]; otherwise, <c>false</c>.
		/// </returns>
		public static bool Contains(Context ctx, StackOperand operand)
		{
			PhiData phiData = ctx.Other as PhiData;

			if (phiData == null)
				return false;

			List<Operand> operands = phiData.Operands as List<Operand>;
			return operands.Contains(operand);
		}

		/// <summary>
		/// Adds the value.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="edge">The edge.</param>
		/// <param name="op">The op.</param>
		public static void AddValue(Context ctx, BasicBlock edge, StackOperand op)
		{
			PhiData phiData = ctx.Other as PhiData;

			if (phiData == null) {
				phiData = new PhiData();
				ctx.Other = phiData;
			}

			List<BasicBlock> blocks = phiData.Blocks as List<BasicBlock>;

			Debug.Assert(blocks.Count < 255, @"Maximum number of operands in PHI exceeded.");

			blocks.Add(edge);
			phiData.Operands.Add(op);
		}

		#endregion // Methods
	}
}
