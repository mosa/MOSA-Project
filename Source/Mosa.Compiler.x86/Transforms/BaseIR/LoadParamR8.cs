// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadParamR8
/// </summary>
[Transform("x86.BaseIR")]
public sealed class LoadParamR8 : BaseIRTransform
{
	public LoadParamR8() : base(IR.LoadParamR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR8);

		context.SetInstruction(X86.MovsdLoad, context.Result, transform.StackFrame, context.Operand1);
	}
}
