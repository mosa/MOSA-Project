/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 call instruction.
	/// </summary>
	public sealed class CallPointerInstruction : X86Instruction
	{
		private static readonly OpCode RegCall = new OpCode(new byte[] { 0xFF }, 2);
		private static readonly byte[] LabelCall = new byte[] { 0xE8 };

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			Operand destinationOperand = context.Operand1;
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
