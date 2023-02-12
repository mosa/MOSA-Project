// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class ThrowOverflow : BaseTransform
{
	public ThrowOverflow() : base(IRInstruction.ThrowOverflow, TransformType.Manual | TransformType.Transform, true)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var method = transform.Compiler.InternalRuntimeType.FindMethodByName("ThrowOverflowException");
		var symbolOperand = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);

		context.SetInstruction(IRInstruction.CallStatic, null, symbolOperand);
	}
}
