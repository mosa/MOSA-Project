// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Expand;

public sealed class ThrowOverflow : BaseTransform
{
	public ThrowOverflow() : base(IR.ThrowOverflow, TransformType.Manual | TransformType.Transform)
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

		context.SetInstruction(IR.CallStatic, null, symbolOperand);
	}
}
