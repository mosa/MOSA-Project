// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.ARMv6
{
	/// <summary>
	/// ARMv6 Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Platform.BasePlatformInstruction" />
	public abstract class ARMv6Instruction : BasePlatformInstruction
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

		/// <summary>
		/// Gets the name of the instruction family.
		/// </summary>
		/// <value>
		/// The name of the instruction family.
		/// </value>
		public override string FamilyName { get { return "ARMv6"; } }

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			Emit(node, emitter as ARMv6CodeEmitter);
		}

		/// <summary>
		/// Emits the specified node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected virtual void Emit(InstructionNode node, ARMv6CodeEmitter emitter)
		{
		}

		#endregion Methods

		#region Methods - Helpers

		/// <summary>
		/// Emits the data processing instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		/// <param name="opcode">The opcode.</param>
		/// <exception cref="CompilerException"></exception>
		protected void EmitDataProcessingInstruction(InstructionNode node, ARMv6CodeEmitter emitter, byte opcode)
		{
			if (node.Operand2.IsCPURegister && node.Operand3.IsShift)
			{
				emitter.EmitInstructionWithRegister(node.ConditionCode, opcode, node.UpdateStatus, node.Operand1.Register.Index, node.Result.Register.Index, node.Operand3.ShiftType, node.Operand2.Register.Index);
			}
			else if (node.Operand2.IsConstant && node.Operand3.IsConstant)
			{
				emitter.EmitInstructionWithImmediate(node.ConditionCode, opcode, node.UpdateStatus, node.Operand1.Register.Index, node.Result.Register.Index, (int)node.Operand2.ConstantSignedLongInteger, (int)node.Operand3.ConstantSignedLongInteger);
			}
			else
			{
				throw new CompilerException();
			}
		}

		/// <summary>
		/// Emits the multiply instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected void EmitMultiplyInstruction(InstructionNode node, ARMv6CodeEmitter emitter)
		{
			if (!node.Operand3.IsCPURegister)
			{
				emitter.EmitMultiply(node.ConditionCode, node.UpdateStatus, node.Operand1.Register.Index, node.Result.Register.Index, node.Operand2.Register.Index);
			}
			else
			{
				emitter.EmitMultiplyWithAccumulate(node.ConditionCode, node.UpdateStatus, node.Operand1.Register.Index, node.Result.Register.Index, node.Operand2.Register.Index, node.Operand3.Register.Index);
			}
		}

		protected void EmitMemoryLoad(InstructionNode node, ARMv6CodeEmitter emitter)
		{
			if (node.Operand2.IsConstant)
			{
				emitter.EmitSingleDataTransfer(
					node.ConditionCode,
					Indexing.Post,
					OffsetDirection.Up,
					TransferSize.Word,
					WriteBack.NoWriteBack,
					TransferType.Load,
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
					TransferType.Load,
					node.Operand1.Index,
					node.Result.Index,
					node.Operand2.ShiftType,
					node.Operand3.Index
				  );
			}
		}

		protected void EmitMemoryStore(InstructionNode node, ARMv6CodeEmitter emitter)
		{
			if (node.Operand2.IsConstant)
			{
				emitter.EmitSingleDataTransfer(
					node.ConditionCode,
					Indexing.Post,
					OffsetDirection.Up,
					TransferSize.Word,
					WriteBack.NoWriteBack,
					TransferType.Store,
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
					TransferType.Store,
					node.Operand1.Index,
					node.Result.Index,
					node.Operand2.ShiftType,
					node.Operand3.Index
				  );
			}
		}

		#endregion Methods - Helpers
	}
}
