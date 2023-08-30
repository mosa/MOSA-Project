// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadSignExtend32x64
/// </summary>
[Transform("x64.IR")]
public sealed class LoadSignExtend32x64 : BaseIRTransform
{
	public LoadSignExtend32x64() : base(IRInstruction.LoadSignExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovzxLoad32, context.Result, context.Operand1, context.Operand2);
	}
}
