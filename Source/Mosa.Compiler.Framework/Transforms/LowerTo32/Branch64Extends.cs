﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Branch64Extends : BaseLowerTo32Transform
{
	public Branch64Extends() : base(Framework.IR.Branch64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!base.Match(context, transform))
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (!(context.Operand1.Definitions[0].Instruction == Framework.IR.ZeroExtend32x64 || context.Operand1.Definitions[0].Instruction == Framework.IR.SignExtend32x64))
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (!(context.Operand2.Definitions[0].Instruction == Framework.IR.ZeroExtend32x64 || context.Operand2.Definitions[0].Instruction == Framework.IR.SignExtend32x64))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var conditionCode = context.ConditionCode;
		var target = context.BranchTargets[0];

		context.SetInstruction(Framework.IR.Branch32, conditionCode, null, t1, t2, target);
	}
}
