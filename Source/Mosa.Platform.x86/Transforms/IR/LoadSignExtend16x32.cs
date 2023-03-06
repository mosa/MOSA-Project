// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// LoadSignExtend16x32
/// </summary>
public sealed class LoadSignExtend16x32 : BaseIRTransform
{
	public LoadSignExtend16x32() : base(IRInstruction.LoadSignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovsxLoad16, context.Result, context.Operand1, context.Operand2);
	}
}
