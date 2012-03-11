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
using Mosa.Compiler.Metadata;
using System;

namespace Mosa.Platform.AVR32.Instructions
{
	/// <summary>
	/// Orh Instruction
	/// Supported Format:
	///     orh Rd, Imm 16 bits
	/// </summary>
	public class OrhInstruction : BaseInstruction
	{
		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Result is RegisterOperand && context.Operand1 is ConstantOperand)
			{
				RegisterOperand destination = context.Result as RegisterOperand;
				ConstantOperand immediate = context.Operand1 as ConstantOperand;

				int value = 0;

				if (IsConstantBetween(immediate, 0, 65535, out value))
				{
					emitter.EmitRegisterOperandWithK16(0xA1, (byte)destination.Register.RegisterCode, (ushort)value);
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
			visitor.Orh(context);
		}

		#endregion // Methods

	}
}
