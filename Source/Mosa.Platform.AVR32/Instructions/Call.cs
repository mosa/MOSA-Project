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

			if (context.Operand1.IsSymbol)
			{
				emitter.Call(context.Operand1);
			}
			else
			{
				if (context.Operand1.IsMemoryAddress)
				{
					emitter.EmitRegisterOperandWithK16(0x101, (byte)context.Operand1.Base.RegisterCode, (ushort)context.Operand1.Offset);
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
