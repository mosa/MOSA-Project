// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// To64
/// </summary>
[Transform("ARM32.IR")]
public sealed class To64 : BaseIRTransform
{
	public To64() : base(IRInstruction.To64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);

		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand1 = MoveConstantToRegisterOrImmediate(transform, context, operand1);
		operand2 = MoveConstantToRegisterOrImmediate(transform, context, operand2);

		context.SetInstruction(ARM32.Mov, resultLow, operand1);
		context.AppendInstruction(ARM32.Mov, resultHigh, operand2);
	}
}
