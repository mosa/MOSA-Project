// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadParamR4
/// </summary>
[Transform]
public sealed class LoadParamR4 : BaseIRTransform
{
	public LoadParamR4() : base(IR.LoadParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR4);

		context.SetInstruction(X86.MovssLoad, context.Result, transform.StackFrame, context.Operand1);
	}
}
