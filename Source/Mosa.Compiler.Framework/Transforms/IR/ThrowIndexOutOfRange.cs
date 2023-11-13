// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class ThrowIndexOutOfRange : BaseTransform
{
	public ThrowIndexOutOfRange() : base(Framework.IR.ThrowIndexOutOfRange, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var method = transform.Compiler.InternalRuntimeType.FindMethodByName("ThrowIndexOutOfRangeException");
		var symbolOperand = Operand.CreateLabel(method, transform.Is32BitPlatform);

		context.SetInstruction(Framework.IR.CallStatic, null, symbolOperand);
	}
}
