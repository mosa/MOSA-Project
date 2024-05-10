// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Useless;

/// <summary>
/// LoadParamSignExtend16x64Double
/// </summary>
public sealed class LoadParamSignExtend16x64Double : BaseTransform
{
	public LoadParamSignExtend16x64Double() : base(IR.SignExtend16x64, TransformType.Manual | TransformType.Optimization, 85)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.LoadParamSignExtend16x64)
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
