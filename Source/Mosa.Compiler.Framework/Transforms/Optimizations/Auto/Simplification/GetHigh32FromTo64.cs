// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

/// <summary>
/// GetHigh32FromTo64
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class GetHigh32FromTo64 : BaseTransform
{
	public GetHigh32FromTo64() : base(IRInstruction.GetHigh32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 25;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.To64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand2;

		context.SetInstruction(IRInstruction.Move32, result, t1);
	}
}
