// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Useless;

/// <summary>
/// LoadParamZeroExtend16x32Double
/// </summary>
public sealed class LoadParamZeroExtend16x32Double : BaseTransform
{
	public LoadParamZeroExtend16x32Double() : base(IR.ZeroExtend16x32, TransformType.Manual | TransformType.Optimization, 85)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.LoadParamZeroExtend16x32)
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
