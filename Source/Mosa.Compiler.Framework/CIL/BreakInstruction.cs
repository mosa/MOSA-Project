// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class BreakInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BreakInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BreakInstruction(OpCode opcode)
			: base(opcode, 0)
		{
		}

		#endregion Construction

		#region Method

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Break(context);
		}

		#endregion Method
	}
}