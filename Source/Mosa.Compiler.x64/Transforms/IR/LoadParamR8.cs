// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParamR8
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParamR8 : BaseIRTransform
{
	public LoadParamR8() : base(Framework.IR.LoadParamR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR8);

		context.SetInstruction(X64.MovsdLoad, context.Result, transform.StackFrame, context.Operand1);
	}
}
