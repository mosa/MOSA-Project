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
	/// St Instruction
	/// Format supported:
	///     st.w    Rp[disp], Rs
	///     st.w    Rp[disp], Rs
	/// </summary>
	public class St : AVR32Instruction
	{

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Result.IsMemoryAddress && context.Operand1.IsRegister)
			{
				Operand destination = context.Result;

				if (IsBetween(destination.Offset.ToInt32(), 0, 60))
				{
					emitter.EmitTwoRegistersWithK4((byte)destination.Base.RegisterCode, (byte)context.Operand1.Register.RegisterCode, (sbyte)destination.Offset.ToInt32());
				}
				else if (IsBetween(destination.Offset.ToInt32(), -32768, 32767))
				{
					emitter.EmitTwoRegistersAndK16(0x14, (byte)destination.Base.RegisterCode, (byte)context.Operand1.Register.RegisterCode, (short)destination.Offset.ToInt32());
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
			visitor.St(context);
		}

		#endregion // Methods

	}
}
