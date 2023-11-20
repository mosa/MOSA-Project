// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadParamObject
/// </summary>
public sealed class LoadParamObject : BaseIRTransform
{
	public LoadParamObject() : base(IR.LoadParamObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovLoad32, context.Result, transform.StackFrame, context.Operand1);
	}
}
