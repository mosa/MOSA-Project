// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// DivSigned32ByPowerOfTwo
/// </summary>
[Transform("IR.Optimizations.Manual.StrengthReduction")]
public sealed class DivSigned32ByPowerOfTwo : BaseTransform
{
	public DivSigned32ByPowerOfTwo() : base(IRInstruction.DivSigned32, TransformType.Auto | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IsResolvedConstant(context.Operand1))
			return false;

		if (context.Operand2.ConstantSigned32 <= 0)
			return false;

		if (!IsPowerOfTwo32(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var n = context.Operand1;
		var d = context.Operand2.ConstantUnsigned32;

		var k = GetPowerOfTwo(d);

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();

		// shrsi t,n,k-1
		context.SetInstruction(IRInstruction.ArithShiftRight32, v1, n, Operand.CreateConstant32(k - 1));
		// shri t, t,32 - k
		context.AppendInstruction(IRInstruction.ShiftRight32, v2, v1, Operand.CreateConstant(32 - k));
		// add t,n,t
		context.AppendInstruction(IRInstruction.Add32, v3, v2, n);
		// shrsi q,t,k
		context.AppendInstruction(IRInstruction.ArithShiftRight32, result, v3, Operand.CreateConstant(k));
	}
}
