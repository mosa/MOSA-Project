// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// An abstract intermediate representation of the end of the try block.
	/// </summary>
	public sealed class TryEnd : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="TryEnd"/>.
		/// </summary>
		public TryEnd() :
			base(0, 0)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets a value indicating whether to [ignore during code generation].
		/// </summary>
		/// <value>
		/// <c>true</c> if [ignore during code generation]; otherwise, <c>false</c>.
		/// </value>
		public override bool IgnoreDuringCodeGeneration { get { return true; } }

		/// <summary>
		/// Gets a value indicating whether to [ignore instruction's basic block].
		/// </summary>
		/// <value>
		/// <c>true</c> if [ignore instruction basic block]; otherwise, <c>false</c>.
		/// </value>
		public override bool IgnoreInstructionBasicBlockTargets { get { return true; } }

		#endregion Properties

		#region Instruction Overrides

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.TryEnd(context);
		}

		#endregion Instruction Overrides
	}
}
