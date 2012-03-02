/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.AVR32.Instructions
{
	/// <summary>
	/// 
	/// </summary>
	public class MovInstruction : BaseInstruction
	{

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override uint ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if ((destination is RegisterOperand) && (source is ConstantOperand))
			{
				ConstantOperand constantOperand = (ConstantOperand)source;

				uint value = (uint)constantOperand.Value;

				if (Is21Bit(value))
				{
					return (uint)(0xE0600000 | ((uint)((RegisterOperand)destination).Register.Index << 16));
				}
				else
				{
					// TODO: raise exception
				}

			}

			return 0;
		}


		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IAVR32Visitor visitor, Context context)
		{
			visitor.Mov(context);
		}

		#endregion // Methods

	}
}
