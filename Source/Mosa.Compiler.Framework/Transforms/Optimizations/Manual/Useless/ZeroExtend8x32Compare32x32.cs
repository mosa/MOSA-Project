// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Useless;

/// <summary>
/// ZeroExtend8x32Compare32x32
/// </summary>
[Transform("IR.Optimizations.Manual.Useless")]
public sealed class ZeroExtend8x32Compare32x32 : BaseTransform
{
	public ZeroExtend8x32Compare32x32() : base(IRInstruction.ZeroExtend8x32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override int Priority => 85;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Compare32x32)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		context.SetInstruction(IRInstruction.Move32, result, operand1);
	}
}
