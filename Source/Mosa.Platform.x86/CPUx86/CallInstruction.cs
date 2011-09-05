/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platform.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 call instruction.
	/// </summary>
	public sealed class CallInstruction : BaseInstruction
	{
		private static readonly OpCode RegCall = new OpCode(new byte[] { 0xFF }, 2);
		private static readonly byte[] LabelCall = new byte[] { 0xE8 };

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			if (ctx.OperandCount == 0)
			{
				emitter.EmitBranch(LabelCall, ctx.Branch.Targets[0]);
				return;
			}

			Operand destinationOperand = ctx.Operand1;
			SymbolOperand destinationSymbol = destinationOperand as SymbolOperand;

			if (destinationSymbol != null)
			{
				emitter.WriteByte(0xE8);
				emitter.Call(destinationSymbol);
			}
			else
			{
				RegisterOperand registerOperand = destinationOperand as RegisterOperand;
				emitter.Emit(RegCall, registerOperand);
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

		#endregion // Methods
	}
}
