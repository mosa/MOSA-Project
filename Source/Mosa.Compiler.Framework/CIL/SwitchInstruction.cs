/*
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
	/// Intermediate representation for the IL switch instruction.
	/// </summary>
	public sealed class SwitchInstruction : UnaryBranchInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SwitchInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public SwitchInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Determines flow behavior of this instruction.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public override FlowControl FlowControl { get { return FlowControl.Switch; } }

		#endregion // Properties

		#region Methods Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Retrieve the number of branch targets
			uint count = decoder.DecodeUInt();

			ctx.AllocateBranchTargets(count + 1);

			// Populate the array
			for (uint i = 0; i < count; i++)
				ctx.BranchTargets[i] = decoder.DecodeInt();
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Switch(context);
		}

		#endregion // Methods Overrides

	}
}
