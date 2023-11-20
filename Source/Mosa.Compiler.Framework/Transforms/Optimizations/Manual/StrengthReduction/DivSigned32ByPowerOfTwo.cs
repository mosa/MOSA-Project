// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// DivSigned32ByPowerOfTwo
/// </summary>
public sealed class DivSigned32ByPowerOfTwo : BaseTransform
{
	public DivSigned32ByPowerOfTwo() : base(IR.DivSigned32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, Transform transform)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var n = context.Operand1;
		var d = context.Operand2.ConstantUnsigned32;

		var k = GetPowerOfTwo(d);

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();

		// shrsi t,n,k-1
		context.SetInstruction(IR.ArithShiftRight32, v1, n, Operand.CreateConstant32(k - 1));
		// shri t, t,32 - k
		context.AppendInstruction(IR.ShiftRight32, v2, v1, Operand.CreateConstant(32 - k));
		// add t,n,t
		context.AppendInstruction(IR.Add32, v3, v2, n);
		// shrsi q,t,k
		context.AppendInstruction(IR.ArithShiftRight32, result, v3, Operand.CreateConstant(k));
	}
}
