// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParamSignExtend16x64
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParamSignExtend16x64 : BaseIRTransform
{
	public LoadParamSignExtend16x64() : base(IRInstruction.LoadParamSignExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovsxLoad16, context.Result, transform.StackFrame, context.Operand1);
	}
}
