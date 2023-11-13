// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// BitCopyR4To32
/// </summary>
[Transform("x64.BaseIR")]
public sealed class BitCopyR4To32 : BaseIRTransform
{
	public BitCopyR4To32() : base(IR.BitCopyR4To32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		//context.SetInstruction(X64.Movdssi64, result, operand1); // TODO
	}
}
