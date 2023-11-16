// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadParamSignExtend16x32
/// </summary>
[Transform]
public sealed class LoadParamSignExtend16x32 : BaseIRTransform
{
	public LoadParamSignExtend16x32() : base(IR.LoadParamSignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovsxLoad16, context.Result, transform.StackFrame, context.Operand1);
	}
}
