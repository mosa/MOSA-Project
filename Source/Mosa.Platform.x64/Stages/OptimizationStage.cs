// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// X86 Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.BaseTransformationStage" />
	public sealed class OptimizationStage : BaseTransformationStage
	{
		private Counter LeaSubstitutionCount = new Counter("X86.OptimizationStage.LeaSubstitution");

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X64.Add64, Add64);
			AddVisitation(X64.Sub64, Sub64);
		}

		protected override void Initialize()
		{
			base.Initialize();

			Register(LeaSubstitutionCount);
		}

		#region Visitation Methods

		public void Add64(Context context)
		{
			// This transformation can reduces restrictions placed on the register allocator.
			// The LEA does not change any of the status flags, however, the add instruction normally updates the status flags (carry, zero, etc.)
			// Therefore, this transformation can only occur if the status flags are unused later.
			// A search is required to determine if a status flag is used
			// The search may not be conclusive; when so, the transformation is not made.

			if (context.Operand1.IsVirtualRegister && !context.Operand2.IsCPURegister)
			{
				if (AreStatusFlagsUsed(context.Node) != TriState.No)
					return;

				context.SetInstruction(X64.Lea64, context.Result, context.Operand1, context.Operand2);
				LeaSubstitutionCount++;
				return;
			}
		}

		public void Sub64(Context context)
		{
			if (context.Operand1.IsVirtualRegister && context.Operand2.IsResolvedConstant)
			{
				if (AreStatusFlagsUsed(context.Node) != TriState.No)
					return;

				var constant = CreateConstant(-context.Operand2.ConstantSigned32);

				context.SetInstruction(X64.Lea64, context.Result, context.Operand1, constant);
				LeaSubstitutionCount++;
				return;
			}
		}

		#endregion Visitation Methods

		public enum TriState { Yes, No, Unknown };

		public static TriState AreStatusFlagsUsed(InstructionNode start)
		{
			var first = start.Instruction as X64Instruction;

			var zeroModified = first.IsZeroFlagModified && !first.IsZeroFlagUndefined;
			var carryModified = first.IsCarryFlagModified && !first.IsCarryFlagUndefined;
			var signModified = first.IsSignFlagModified && !first.IsSignFlagUndefined;
			var overflowModified = first.IsOverflowFlagSet && !first.IsOverflowFlagUndefined;
			var parityModified = first.IsParityFlagModified && !first.IsParityFlagUndefined;

			return AreStatusFlagsUsed(start.Next, zeroModified, carryModified, signModified, overflowModified, parityModified);
		}

		public static TriState AreStatusFlagsUsed(InstructionNode start, bool zeroModified, bool carryModified, bool signModified, bool overflowModified, bool parityModified)
		{
			// if none are modified (or not undefined), then they can't be used later
			if (!zeroModified && !carryModified && !signModified && !overflowModified && !parityModified)
				return TriState.No;

			for (var at = start; ; at = at.Next)
			{
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

				var instruction = at.Instruction as X64Instruction;

				if (instruction == null)
					return TriState.Unknown; // Unknown IR instruction

				if ((zeroModified && instruction.IsZeroFlagUsed)
					|| (carryModified && instruction.IsCarryFlagUsed)
					|| (signModified && instruction.IsSignFlagUsed)
					|| (overflowModified && instruction.IsOverflowFlagUsed)
					|| (parityModified && instruction.IsParityFlagUsed))
					return TriState.Yes;

				if (zeroModified && (instruction.IsZeroFlagCleared || instruction.IsZeroFlagSet || instruction.IsZeroFlagUndefined || instruction.IsZeroFlagModified))
					zeroModified = false;

				if (carryModified && (instruction.IsCarryFlagCleared || instruction.IsCarryFlagSet || instruction.IsCarryFlagUndefined || instruction.IsCarryFlagModified))
					carryModified = false;

				if (signModified && (instruction.IsSignFlagCleared || instruction.IsSignFlagSet || instruction.IsSignFlagUndefined || instruction.IsSignFlagModified))
					signModified = false;

				if (overflowModified && (instruction.IsOverflowFlagCleared || instruction.IsOverflowFlagSet || instruction.IsOverflowFlagUndefined || instruction.IsOverflowFlagModified))
					overflowModified = false;

				if (parityModified && (instruction.IsParityFlagCleared || instruction.IsParityFlagSet || instruction.IsParityFlagUndefined || instruction.IsParityFlagModified))
					parityModified = false;

				if (!zeroModified && !carryModified && !signModified && !overflowModified && !parityModified)
					return TriState.No;
			}
		}
	}
}
