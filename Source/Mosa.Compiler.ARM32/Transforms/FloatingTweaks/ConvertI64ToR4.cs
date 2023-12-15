// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.FloatingTweaks;

/// <summary>
/// ConvertI64ToR4
/// </summary>
public sealed class ConvertI64ToR4 : BaseTransform
{
	public ConvertI64ToR4() : base(IR.ConvertI64ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		transform.SplitOperand(operand1, out _, out _);

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.AllocateR8();

		var multiply = Operand.CreateConstant((float)uint.MaxValue);

		context.SetInstruction(IR.GetHigh32, v1, operand1);
		context.AppendInstruction(IR.ConvertI32ToR4, v2, v1);
		context.AppendInstruction(IR.MulR4, result, v2, multiply);
	}
}
