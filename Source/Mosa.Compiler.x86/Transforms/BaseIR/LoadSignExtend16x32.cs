// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadSignExtend16x32
/// </summary>
public sealed class LoadSignExtend16x32 : BaseIRTransform
{
	public LoadSignExtend16x32() : base(IR.LoadSignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovsxLoad16, context.Result, context.Operand1, context.Operand2);
	}
}
