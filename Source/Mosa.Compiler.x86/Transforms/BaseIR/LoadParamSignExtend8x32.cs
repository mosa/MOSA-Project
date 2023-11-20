// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadParamSignExtend8x32
/// </summary>
public sealed class LoadParamSignExtend8x32 : BaseIRTransform
{
	public LoadParamSignExtend8x32() : base(IR.LoadParamSignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovsxLoad8, context.Result, transform.StackFrame, context.Operand1);
	}
}
