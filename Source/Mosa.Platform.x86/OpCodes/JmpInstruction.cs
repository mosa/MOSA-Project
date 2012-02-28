/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.x86.OpCodes
{
	/// <summary>
	/// Representation the x86 jump instruction.
	/// </summary>
	public sealed class JmpInstruction : BaseInstruction
	{

		#region Data Members

		private static readonly byte[] JMP = new byte[] { 0xE9 };
		private static readonly OpCode JmpReg = new OpCode(new byte[] { 0xFF }, 4);

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 1; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			if (ctx.Operand1 is RegisterOperand)
				emitter.Emit(JmpReg, ctx.Operand1);
			else 
				emitter.EmitBranch(JMP, ctx.Branch.Targets[0]);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Add(context);
		}

		#endregion // Methods

	}
}
