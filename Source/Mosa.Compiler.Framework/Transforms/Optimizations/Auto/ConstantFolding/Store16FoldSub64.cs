// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// Store16FoldSub64
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class Store16FoldSub64 : BaseTransform
{
	public Store16FoldSub64() : base(IRInstruction.Store16, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Sub64)
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;
		var t4 = context.Operand3;

		var e1 = Operand.CreateConstant(Sub64(To64(t3), To64(t2)));

		context.SetInstruction(IRInstruction.Store16, result, t1, e1, t4);
	}
}
