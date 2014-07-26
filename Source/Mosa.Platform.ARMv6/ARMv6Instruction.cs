/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ARMv6Instruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ARMv6Instruction" /> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		protected ARMv6Instruction(byte resultCount, byte operandCount)
			: base(resultCount, operandCount)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected virtual uint ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			throw new System.Exception("opcode not implemented for this instruction");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected virtual void Emit(Context context, MachineCodeEmitter emitter)
		{
			// TODO: Check x86 Implementation
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		public void Emit(Context context, BaseCodeEmitter emitter)
		{
			Emit(context, emitter as MachineCodeEmitter);
		}

		/// <summary>
		/// Emits the data processing instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		/// <param name="opcode">The opcode.</param>
		/// <exception cref="InvalidCompilerException"></exception>
		protected void EmitDataProcessingInstruction(Context context, MachineCodeEmitter emitter, byte opcode)
		{
			if (context.Operand2.IsRegister && context.Operand3.IsShift)
			{
				emitter.EmitInstructionWithRegister(context.ConditionCode, opcode, context.UpdateStatus, context.Operand1.Register.Index, context.Result.Register.Index, context.Operand3.ShiftType, context.Operand2.Register.Index);
			}
			else if (context.Operand2.IsConstant && context.Operand3.IsConstant)
			{
				emitter.EmitInstructionWithImmediate(context.ConditionCode, opcode, context.UpdateStatus, context.Operand1.Register.Index, context.Result.Register.Index, (int)context.Operand2.ConstantSignedInteger, (int)context.Operand3.ConstantSignedInteger);
			}
			else
			{
				throw new InvalidCompilerException();
			}
		}

		#endregion Methods

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "ARMv6." + base.ToString();
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public virtual void Visit(IARMv6Visitor visitor, Context context)
		{
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IVisitor visitor, Context context)
		{
			if (visitor is IARMv6Visitor)
				Visit(visitor as IARMv6Visitor, context);
		}

		#endregion Operand Overrides
	}
}