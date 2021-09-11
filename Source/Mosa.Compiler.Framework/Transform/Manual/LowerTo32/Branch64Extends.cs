﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class Branch64Extends : BaseTransformation
	{
		public Branch64Extends() : base(IRInstruction.Branch64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!transformContext.LowerTo32)
				return false;

			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (!(context.Operand1.Definitions[0].Instruction == IRInstruction.ZeroExtend32x64 || context.Operand1.Definitions[0].Instruction == IRInstruction.SignExtend32x64))
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (!(context.Operand2.Definitions[0].Instruction == IRInstruction.ZeroExtend32x64 || context.Operand2.Definitions[0].Instruction == IRInstruction.SignExtend32x64))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;
			var conditionCode = context.ConditionCode;
			var target = context.BranchTargets[0];

			context.SetInstruction(IRInstruction.Branch32, conditionCode, null, t1, t2, target);
		}
	}
}
