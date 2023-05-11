// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Phi;

public sealed class Phi32Add32 : BaseTransform
{
	public Phi32Add32() : base(IRInstruction.Phi32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.OperandCount != 2)
			return false;

		if (!context.Operand1.IsConstant)
			return false;

		if (!context.Operand2.IsConstant)
			return false;

		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (!IsSSAForm(context.Result))
			return false;

		if (context.Result.Uses.Count != 1)
			return false;

		var ctx = context.Result.Uses[0];

		if (ctx.Instruction != IRInstruction.Add32)
			return false;

		if (!ctx.Operand2.IsResolvedConstant)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var ctx = context.Result.Uses[0];
		var result = ctx.Result;
		var value = ctx.Operand2.ConstantUnsigned32;

		ctx.SetNop();

		context.Operand1 = Operand.CreateConstant(context.Operand1.ConstantUnsigned32 + value);
		context.Operand2 = Operand.CreateConstant(context.Operand2.ConstantUnsigned32 + value);
		context.Result = result;
	}
}
