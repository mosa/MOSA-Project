// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// Call
/// </summary>
[Transform("ARM32.IR")]
public sealed class Call : BaseIRTransform
{
	public Call() : base(Framework.IR.Call, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		if (context.Result?.IsInt64 == true)
		{
			transform.SplitOperand(context.Result, out _, out _);
		}

		foreach (var operand in context.Operands)
		{
			if (operand.IsInt64)
			{
				transform.SplitOperand(operand, out _, out _);
			}
		}
	}
}
