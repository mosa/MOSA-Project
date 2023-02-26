// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class ThrowIndexOutOfRange : BaseTransform
{
	public ThrowIndexOutOfRange() : base(IRInstruction.ThrowIndexOutOfRange, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var method = transform.Compiler.InternalRuntimeType.FindMethodByName("ThrowIndexOutOfRangeException");
		var symbolOperand = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);

		context.SetInstruction(IRInstruction.CallStatic, null, symbolOperand);
	}
}
