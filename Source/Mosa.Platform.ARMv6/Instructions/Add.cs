/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Marcelo Caetano (marcelocaetano) <marcelo.caetano@ymail.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Add instruction: Add
	/// ARMv6-M provides register and small immediate versions only.
	/// </summary>
	public class Add : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Add"/>.
		/// </summary>
		public Add() :
			base(1, 3)
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
			if (context.Operand2.IsRegister && context.Operand3.IsShift)
			{
				emitter.EmitDataProcessingInstructionWithRegister(context.ConditionCode, Bits.b0100, context.UpdateStatusFlags, context.Operand1.Register.Index, context.Result.Register.Index, context.Operand2.Register.Index, context.Operand3.ShiftType);
			}
			else if (context.Operand2.IsConstant && context.Operand3.IsConstant)
			{
				emitter.EmitDataProcessingInstructionWithImmediate(context.ConditionCode, Bits.b0100, context.UpdateStatusFlags, context.Operand1.Register.Index, context.Result.Register.Index, (int)context.Operand2.ConstantSignedInteger, (int)context.Operand3.ConstantSignedInteger);
			}
			else
			{
				throw new InvalidCompilerException();
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IARMv6Visitor visitor, Context context)
		{
			visitor.Add(context);
		}

		#endregion Methods
	}
}