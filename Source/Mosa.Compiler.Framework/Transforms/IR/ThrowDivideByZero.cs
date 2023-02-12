// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class ThrowDivideByZero : BaseTransform
{
	public ThrowDivideByZero() : base(IRInstruction.ThrowDivideByZero, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var method = transform.Compiler.InternalRuntimeType.FindMethodByName("ThrowDivideByZeroException");
		var symbolOperand = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);

		context.SetInstruction(IRInstruction.CallStatic, null, symbolOperand);
	}
}
