// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// StoreParam32
/// </summary>
public sealed class StoreParam32 : BaseIRTransform
{
	public StoreParam32() : base(IR.StoreParam32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovStore32, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
