/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 call instruction.
	/// </summary>
	public sealed class Call : X86Instruction
	{
		#region Data Member

		private static readonly OpCode RegCall = new OpCode(new byte[] { 0xFF }, 2);
		private static readonly byte[] CALL = new byte[] { 0xE8 };

		#endregion Data Member

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Call"/>.
		/// </summary>
		public Call() :
			base(0, 0)
		{
		}

		#endregion Construction

		#region Properties

		public override FlowControl FlowControl { get { return FlowControl.Call; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.OperandCount == 0)
			{
				emitter.EmitRelativeBranch(CALL, context.Targets[0].Label);
				return;
			}

			if (context.Operand1.IsSymbol)
			{
				emitter.WriteByte(0xE8);
				emitter.EmitCallSite(context.Operand1);
			}
			else
			{
				emitter.Emit(RegCall, context.Operand1);
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Call(context);
		}

		#endregion Methods
	}
}
