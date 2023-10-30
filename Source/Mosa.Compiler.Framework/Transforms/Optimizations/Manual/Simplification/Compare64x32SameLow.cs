// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

public sealed class Compare64x32SameLow : BaseTransform
{
	public Compare64x32SameLow() : base(IRInstruction.Compare64x32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.To64)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand1.ConstantUnsigned32 != 0)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.To64)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand1.ConstantUnsigned32 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1.Definitions[0].Operand2;
		var operand2 = context.Operand2.Definitions[0].Operand2;

		context.SetInstruction(IRInstruction.Compare32x32, context.ConditionCode, context.Result, operand1, operand2);
	}
}
