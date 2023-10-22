// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// StoreParam64
/// </summary>
[Transform("x64.IR")]
public sealed class StoreParam64 : BaseIRTransform
{
	public StoreParam64() : base(IRInstruction.StoreParam64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovStore64, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
