// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Useless;

/// <summary>
/// LoadParamZeroExtend8x64Double
/// </summary>
public sealed class LoadParamZeroExtend8x64Double : BaseTransform
{
	public LoadParamZeroExtend8x64Double() : base(IR.ZeroExtend8x64, TransformType.Manual | TransformType.Optimization, 85)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.LoadParamZeroExtend8x64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		context.SetInstruction(IR.Move64, result, operand1);
	}
}
