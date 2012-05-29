/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>   
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.AVR32.Instructions
{
	/// <summary>
	/// Call Instruction
	/// </summary>
	public class Call : AVR32Instruction
	{
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
				// TODO:
				//emitter.EmitBranch(LabelCall, context.BranchTargets[0]);
				return;
			}

			Operand destinationOperand = context.Operand1;
			SymbolOperand destinationSymbol = destinationOperand as SymbolOperand;

			if (destinationSymbol != null)
			{
				emitter.Call(destinationSymbol);
			}
			else
			{
				if (destinationOperand is MemoryOperand)
				{
					//RegisterOperand register = destinationOperand as RegisterOperand;
					//emitter.EmitSingleRegisterInstructions(0x11, (byte)register.Register.RegisterCode);
					MemoryOperand memory = destinationOperand as MemoryOperand;
					emitter.EmitRegisterOperandWithK16(0x101, (byte)memory.Base.RegisterCode, (ushort)memory.Offset);
				}
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IAVR32Visitor visitor, Context context)
		{
			visitor.Call(context);
		}

		#endregion // Methods

	}
}
