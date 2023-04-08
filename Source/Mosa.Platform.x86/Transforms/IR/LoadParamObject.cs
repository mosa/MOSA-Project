// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// LoadParamObject
/// </summary>
[Transform("x86.IR")]
public sealed class LoadParamObject : BaseIRTransform
{
	public LoadParamObject() : base(IRInstruction.LoadParamObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X86.MovLoad32, context.Result, transform.StackFrame, context.Operand1);
	}
}
