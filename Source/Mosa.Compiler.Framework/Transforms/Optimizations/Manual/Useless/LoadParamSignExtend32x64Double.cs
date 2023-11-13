// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Useless;

/// <summary>
/// LoadParamSignExtend32x64Double
/// </summary>
[Transform("IR.Optimizations.Manual.Useless")]
public sealed class LoadParamSignExtend32x64Double : BaseTransform
{
	public LoadParamSignExtend32x64Double() : base(Framework.IR.SignExtend32x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 85;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.LoadParamSignExtend32x64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		context.SetInstruction(Framework.IR.Move32, result, operand1);
	}
}
