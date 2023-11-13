// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// IfThenElse32
/// </summary>
[Transform("x86.BaseIR")]
public sealed class IfThenElse32 : BaseIRTransform
{
	public IfThenElse32() : base(IR.IfThenElse32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		if (operand2.IsConstant && operand3.IsConstant)
		{
			var v1 = transform.VirtualRegisters.Allocate(result);

			context.SetInstruction(X86.Cmp32, null, operand1, Operand.Constant32_0);
			context.AppendInstruction(X86.Mov32, result, operand2);                                     // true
			context.AppendInstruction(X86.Mov32, v1, operand3);                                         // true
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, result, result, v1);             // false
		}
		else if (operand2.IsConstant && !operand3.IsConstant)
		{
			context.SetInstruction(X86.Cmp32, null, operand1, Operand.Constant32_0);
			context.AppendInstruction(X86.Mov32, result, operand2);                                 // true
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, result, result, operand3);       // false
		}
		else if (!operand2.IsConstant && operand3.IsConstant)
		{
			context.SetInstruction(X86.Cmp32, null, operand1, Operand.Constant32_0);
			context.AppendInstruction(X86.Mov32, result, operand3);                                     // true
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, result, result, operand2);    // false
		}
		else if (!operand2.IsConstant && !operand3.IsConstant)
		{
			context.SetInstruction(X86.Cmp32, null, operand1, Operand.Constant32_0);
			context.AppendInstruction(X86.Mov32, result, operand2);                                     // true
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, result, result, operand3);       // false
		}
	}
}
