// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// BitCopyR4To32
/// </summary>
[Transform("x64.IR")]
public sealed class BitCopyR4To32 : BaseIRTransform
{
	public BitCopyR4To32() : base(IRInstruction.BitCopyR4To32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		//context.SetInstruction(X64.Movdssi64, result, operand1); // TODO
	}
}
