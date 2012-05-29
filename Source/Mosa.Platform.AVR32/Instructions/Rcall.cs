/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>   
 */

using System;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.AVR32.Instructions
{
	/// <summary>
	/// Rcall Instruction
	/// Supported Format:
	///     rcall PC[disp] 10 bits
	///     rcall PC[disp] 21 bits
	/// </summary>
	public class Rcall : AVR32Instruction
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
				int displacement = context.BranchTargets[0];

				if (IsBetween(displacement, -1024, 1022))
				{
					emitter.EmitRelativeJumpAndCall(0x01, context.BranchTargets[0]);
				}
				else
					if (IsBetween(displacement, -2097151, 2097150))
					{
						emitter.EmitNoRegisterAndK21(0x50, context.BranchTargets[0]);
					}
					else
						throw new OverflowException();
			}
			else
				throw new Exception("Not supported combination of operands");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IAVR32Visitor visitor, Context context)
		{
			visitor.Rcall(context);
		}

		#endregion // Methods

	}
}
