// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParamR4
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParamR4 : BaseIRTransform
{
	public LoadParamR4() : base(IRInstruction.LoadParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR4);

		context.SetInstruction(X64.MovssLoad, context.Result, transform.StackFrame, context.Operand1);
	}
}
