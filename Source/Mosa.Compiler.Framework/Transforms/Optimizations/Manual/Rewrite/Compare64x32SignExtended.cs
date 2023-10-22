// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Rewrite;

/// <summary>
/// Compare64x32SignExtended
/// </summary>
public sealed class Compare64x32SignExtended : BaseTransform
{
	public Compare64x32SignExtended() : base(IRInstruction.Compare64x32, TransformType.Manual | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.SignExtend32x64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.SignExtend32x64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;

		var cond = context.ConditionCode;

		context.SetInstruction(IRInstruction.Compare32x32, cond, result, t1, t2);
	}
}
