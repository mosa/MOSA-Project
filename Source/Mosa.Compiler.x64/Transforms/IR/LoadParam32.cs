// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParam32
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParam32 : BaseIRTransform
{
	public LoadParam32() : base(Framework.IR.LoadParam32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovLoad32, context.Result, transform.StackFrame, context.Operand1);
	}
}
