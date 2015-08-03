// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class InitblkInstruction : NaryInstruction
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

		#endregion Construction

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Initblk(context);
		}

		#endregion Methods
	}
}