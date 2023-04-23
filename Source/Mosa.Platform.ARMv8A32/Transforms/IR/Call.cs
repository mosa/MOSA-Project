// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Call
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class Call : BaseIRTransform
{
	public Call() : base(IRInstruction.Call, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		if (context.Result?.IsInt64 == true)
		{
			transform.SplitLongOperand(context.Result, out _, out _);
		}

		foreach (var operand in context.Operands)
		{
			if (operand.IsInt64)
			{
				transform.SplitLongOperand(operand, out _, out _);
			}
		}
	}
}
