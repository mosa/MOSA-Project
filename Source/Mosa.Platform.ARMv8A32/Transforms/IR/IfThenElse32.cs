// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// IfThenElse32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class IfThenElse32 : BaseIRTransform
{
	public IfThenElse32() : base(IRInstruction.IfThenElse32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		operand1 = MoveConstantToRegister(transform, context, operand1);
		operand2 = MoveConstantToRegisterOrImmediate(transform, context, operand2);
		operand3 = MoveConstantToRegisterOrImmediate(transform, context, operand3);

		context.SetInstruction(ARMv8A32.Cmp, StatusRegister.Set, null, operand1, transform.Constant32_0);
		context.AppendInstruction(ARMv8A32.Mov, ConditionCode.Equal, result, operand2);
		context.AppendInstruction(ARMv8A32.Mov, ConditionCode.NotEqual, result, operand3);
	}
}
