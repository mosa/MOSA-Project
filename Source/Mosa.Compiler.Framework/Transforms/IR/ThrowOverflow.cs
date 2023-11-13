// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class ThrowOverflow : BaseTransform
{
	public ThrowOverflow() : base(Framework.IR.ThrowOverflow, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var method = transform.Compiler.InternalRuntimeType.FindMethodByName("ThrowOverflowException");
		var symbolOperand = Operand.CreateLabel(method, transform.Is32BitPlatform);

		context.SetInstruction(Framework.IR.CallStatic, null, symbolOperand);
	}
}
