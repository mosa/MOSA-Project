// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadParamObject
/// </summary>
[Transform]
public sealed class LoadParamObject : BaseIRTransform
{
	public LoadParamObject() : base(IR.LoadParamObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovLoad64, context.Result, transform.StackFrame, context.Operand1);
	}
}
