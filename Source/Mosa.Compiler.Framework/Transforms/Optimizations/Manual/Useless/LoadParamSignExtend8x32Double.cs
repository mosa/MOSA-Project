// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Useless;

/// <summary>
/// LoadParamSignExtend8x32Double
/// </summary>
public sealed class LoadParamSignExtend8x32Double : BaseTransform
{
	public LoadParamSignExtend8x32Double() : base(IR.SignExtend8x32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 85;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.LoadParamSignExtend8x32)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		context.SetInstruction(IR.Move32, result, operand1);
	}
}
