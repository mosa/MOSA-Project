// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// LoadParam64
/// </summary>
public sealed class LoadParam64 : BaseIRTransform
{
	public LoadParam64() : base(IRInstruction.LoadParam64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.MovLoad64, context.Result, transform.StackFrame, context.Operand1);
	}
}
