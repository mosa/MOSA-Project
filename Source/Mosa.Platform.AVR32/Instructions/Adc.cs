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
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.AVR32.Instructions
{
	/// <summary>
	/// Adc instruction
	/// Supported Format:
	///     adc Rd, Rx, Ry
	/// </summary>
	public class Adc : AVR32Instruction
	{

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Result is RegisterOperand && context.Operand1 is RegisterOperand && context.Operand2 is RegisterOperand)
			{
				RegisterOperand destination = context.Result as RegisterOperand;
				RegisterOperand firstSource = context.Operand1 as RegisterOperand;
				RegisterOperand secondSource = context.Operand2 as RegisterOperand;
				emitter.EmitThreeRegistersUnshifted(0x04, (byte)firstSource.Register.RegisterCode, (byte)secondSource.Register.RegisterCode, (byte)destination.Register.RegisterCode);
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
			visitor.Adc(context);
		}

		#endregion // Methods

	}
}
