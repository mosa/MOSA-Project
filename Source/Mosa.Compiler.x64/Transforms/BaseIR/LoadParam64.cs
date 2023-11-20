// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadParam64
/// </summary>
public sealed class LoadParam64 : BaseIRTransform
{
	public LoadParam64() : base(IR.LoadParam64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovLoad64, context.Result, transform.StackFrame, context.Operand1);
	}
}
