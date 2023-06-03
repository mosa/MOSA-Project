// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.TruncateTo32;

public sealed class Store32 : BaseTruncateTo32Transform
{
	public Store32() : base(IRInstruction.Store32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!transform.Is32BitPlatform)
			return false;

		return context.Operand1.IsInt64 || context.Operand2.IsInt64 || context.Operand3.IsInt64;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TruncateOperand1Thru3To32(context, transform);
	}
}
