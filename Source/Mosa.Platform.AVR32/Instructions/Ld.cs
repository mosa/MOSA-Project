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
	/// Ld Instruction
	/// Supported Format:
	///     ld.w Rd, Rp[disp] 5 bits
	///     ld.w Rd, Rp[disp] 16 bits
	/// </summary>
	public class Ld : AVR32Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Ld"/>.
		/// </summary>
		public Ld() :
			base(0, 0)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Result.IsRegister && context.Operand1.IsMemoryAddress)
			{
				long displacement = context.Operand1.Displacement;

				if (IsBetween(displacement, 0, 124))
				{
					emitter.EmitDisplacementLoadWithK5Immediate((byte)context.Result.Register.RegisterCode, (sbyte)displacement, (byte)context.Operand1.EffectiveOffsetBase.RegisterCode);
				}
				else
					if (IsBetween(displacement, -32768, 32767))
					{
						emitter.EmitTwoRegistersAndK16(0x0F, (byte)context.Operand1.EffectiveOffsetBase.RegisterCode, (byte)context.Result.Register.RegisterCode, (short)displacement);
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
			visitor.Ld(context);
		}

		#endregion Methods
	}
}