// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected virtual void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			// TODO: Check x86 Implementation
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		public void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			Emit(node, emitter as MachineCodeEmitter);
		}

		/// <summary>
		/// Emits the data processing instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		/// <param name="opcode">The opcode.</param>
		/// <exception cref="InvalidCompilerException"></exception>
		protected void EmitDataProcessingInstruction(InstructionNode node, MachineCodeEmitter emitter, byte opcode)
		{
			if (node.Operand2.IsRegister && node.Operand3.IsShift)
			{
				emitter.EmitInstructionWithRegister(node.ConditionCode, opcode, node.UpdateStatus, node.Operand1.Register.Index, node.Result.Register.Index, node.Operand3.ShiftType, node.Operand2.Register.Index);
			}
			else if (node.Operand2.IsConstant && node.Operand3.IsConstant)
			{
				emitter.EmitInstructionWithImmediate(node.ConditionCode, opcode, node.UpdateStatus, node.Operand1.Register.Index, node.Result.Register.Index, (int)node.Operand2.ConstantSignedLongInteger, (int)node.Operand3.ConstantSignedLongInteger);
			}
			else
			{
				throw new InvalidCompilerException();
			}
		}

		/// <summary>
		/// Emits the multiply instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected void EmitMultiplyInstruction(InstructionNode node, MachineCodeEmitter emitter)
		{
			if (!node.Operand3.IsRegister)
			{
				emitter.EmitMultiply(node.ConditionCode, node.UpdateStatus, node.Operand1.Register.Index, node.Result.Register.Index, node.Operand2.Register.Index);
			}
			else
			{
				emitter.EmitMultiplyWithAccumulate(node.ConditionCode, node.UpdateStatus, node.Operand1.Register.Index, node.Result.Register.Index, node.Operand2.Register.Index, node.Operand3.Register.Index);
			}
		}

		protected void EmitMemoryLoadStore(InstructionNode node, MachineCodeEmitter emitter, TransferType transferType)
		{
			if (node.Operand2.IsConstant)
			{
				emitter.EmitSingleDataTransfer(
					node.ConditionCode,
					Indexing.Post,
					OffsetDirection.Up,
					TransferSize.Word,
					WriteBack.NoWriteBack,
					transferType,
					node.Operand1.Index,
					node.Result.Index,
					(uint)node.Operand2.ConstantUnsignedLongInteger
				);
			}
			else
			{
				emitter.EmitSingleDataTransfer(
					  node.ConditionCode,
					  Indexing.Post,
					  OffsetDirection.Up,
					  TransferSize.Word,
					  WriteBack.NoWriteBack,
					  transferType,
					  node.Operand1.Index,
					  node.Result.Index,
					  node.Operand2.ShiftType,
					  node.Operand3.Index
				  );
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

		#endregion Operand Overrides
	}
}
