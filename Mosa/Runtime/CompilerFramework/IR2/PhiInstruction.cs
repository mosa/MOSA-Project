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
using System.Diagnostics;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IR2
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
		/// Initializes a new instance of PhiInstruction.
		/// </summary>
		public PhiInstruction()
		{
		}

		#endregion // Construction

		#region Instruction Overrides

		/// <summary>
		/// Gibt einen <see cref="T:System.String"/> zurück, der den aktuellen <see cref="T:System.Object"/> darstellt.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// Ein <see cref="T:System.String"/>, der den aktuellen <see cref="T:System.Object"/> darstellt.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("IR phi ; {0} = phi(", instruction.Result);
			
			if (instruction.Operand1 != null)
				builder.AppendFormat("{0}, ", instruction.Operand1);
			if (instruction.Operand2 != null)
				builder.AppendFormat("{0}, ", instruction.Operand2);
			if (instruction.Operand3 != null)
				builder.AppendFormat("{0}, ", instruction.Operand3);
			
			builder.Remove(builder.Length - 2, 2);
			builder.Append(')');
			return builder.ToString();
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.PhiInstruction(context);
		}

		#endregion // Instruction Overrides
	}
}
