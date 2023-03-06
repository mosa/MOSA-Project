// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// Compare32x32
/// </summary>
public sealed class Compare32x32 : BaseIRTransform
{
	public Compare32x32() : base(IRInstruction.Compare32x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var condition = context.ConditionCode;
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.AllocateVirtualRegister32();

		context.SetInstruction(X86.Cmp32, null, operand1, operand2);
		context.AppendInstruction(X86.Setcc, condition, v1);
		context.AppendInstruction(X86.Movzx8To32, result, v1);
	}
}
