// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadZeroExtend16x32
/// </summary>
[Transform("x86.BaseIR")]
public sealed class LoadZeroExtend16x32 : BaseIRTransform
{
	public LoadZeroExtend16x32() : base(IR.LoadZeroExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovzxLoad16, context.Result, context.Operand1, context.Operand2);
	}
}
