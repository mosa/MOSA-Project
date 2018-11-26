// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class OptimizationStage : BaseTransformationStage
	{
		private Counter LeaSubstitutionCount = new Counter("X86.Optimizations.LeaSubstitution");

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.Add32, Add32);
			AddVisitation(X86.Sub32, Sub32);
		}

		protected override void Initialize()
		{
			base.Initialize();

			Register(LeaSubstitutionCount);
		}

		#region Visitation Methods

		public void Add32(InstructionNode node)
		{
			// This transformation can reduces restrictions placed on the register allocator.
			// The LEA does not change any of the status flags, however, the add instruction some flags (carry, zero, etc.)
			// Therefore, this transformation can only occur if the status flags are unused later.
			// A search is required to determine if a status flag is used
			// The search may not be conclusive; when so, the transformation is not made.

			if (node.Operand1.IsVirtualRegister && !node.Operand2.IsCPURegister)
			{
				if (AreStatusFlagsUsed(node) != TriState.No)
					return;

				node.SetInstruction(X86.Lea32, node.Result, node.Operand1, node.Operand2);
				LeaSubstitutionCount++;
				return;
			}
		}

		public void Sub32(InstructionNode node)
		{
			if (node.Operand1.IsVirtualRegister && node.Operand2.IsResolvedConstant)
			{
				if (AreStatusFlagsUsed(node) != TriState.No)
					return;

				var constant = CreateConstant(-node.Operand2.ConstantSignedInteger);

				node.SetInstruction(X86.Lea32, node.Result, node.Operand1, constant);
				LeaSubstitutionCount++;
				return;
			}
		}

		#endregion Visitation Methods

		private enum TriState { Yes, No, Unknown };

		private static TriState AreStatusFlagsUsed(InstructionNode start)
		{
			var first = start.Instruction as X86Instruction;

			var zeroModified = first.IsZeroFlagModified && !first.IsZeroFlagUndefined;
			var carryModified = first.IsCarryFlagModified && !first.IsCarryFlagUndefined;
			var signModified = first.IsSignFlagModified && !first.IsSignFlagUndefined;
			var overflowModified = first.IsOverflowFlagSet && !first.IsOverflowFlagUndefined;
			var parityModified = first.IsParityFlagModified && !first.IsParityFlagUndefined;

			// if none are modified (or not undefined), then they can't be used later
			if (!zeroModified && !carryModified && !signModified && !overflowModified && !parityModified)
				return TriState.No;

			var at = start;

			while (true)
			{
				at = at.Next;

				if (at.IsEmptyOrNop)
					continue;

				if (at.IsBlockEndInstruction)
					return TriState.Unknown;

				if (at.Instruction == IRInstruction.StableObjectTracking
					|| at.Instruction == IRInstruction.UnstableObjectTracking
					|| at.Instruction == IRInstruction.Kill
					|| at.Instruction == IRInstruction.KillAll
					|| at.Instruction == IRInstruction.KillAllExcept
					|| at.Instruction == IRInstruction.Gen)
					continue;

				if (at.Instruction.FlowControl != FlowControl.Next)
					return TriState.Unknown; // Flow direction changed

				var x86Instruction = at.Instruction as X86Instruction;

				if (x86Instruction == null)
					return TriState.Unknown; // Unknown IR instruction

				if ((zeroModified && x86Instruction.IsZeroFlagUsed)
					|| (carryModified && x86Instruction.IsCarryFlagUsed)
					|| (signModified && x86Instruction.IsSignFlagUsed)
					|| (overflowModified && x86Instruction.IsOverflowFlagUsed)
					|| (parityModified && x86Instruction.IsParityFlagUsed))
					return TriState.Yes;

				if (zeroModified && (x86Instruction.IsZeroFlagCleared || x86Instruction.IsZeroFlagSet || x86Instruction.IsZeroFlagUndefined || x86Instruction.IsZeroFlagModified))
					zeroModified = false;

				if (carryModified && (x86Instruction.IsCarryFlagCleared || x86Instruction.IsCarryFlagSet || x86Instruction.IsCarryFlagUndefined || x86Instruction.IsCarryFlagModified))
					carryModified = false;

				if (signModified && (x86Instruction.IsSignFlagCleared || x86Instruction.IsSignFlagSet || x86Instruction.IsSignFlagUndefined || x86Instruction.IsSignFlagModified))
					signModified = false;

				if (overflowModified && (x86Instruction.IsOverflowFlagCleared || x86Instruction.IsOverflowFlagSet || x86Instruction.IsOverflowFlagUndefined || x86Instruction.IsOverflowFlagModified))
					overflowModified = false;

				if (parityModified && (x86Instruction.IsParityFlagCleared || x86Instruction.IsParityFlagSet || x86Instruction.IsParityFlagUndefined || x86Instruction.IsParityFlagModified))
					parityModified = false;

				if (!zeroModified && !carryModified && !signModified && !overflowModified && !parityModified)
					return TriState.No;
			}
		}
	}
}
