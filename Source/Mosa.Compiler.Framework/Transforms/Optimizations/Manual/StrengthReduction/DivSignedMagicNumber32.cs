// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

// TODO: Not working for all cases

/// <summary>
/// DivSignedMagicNumber32
/// </summary>
public sealed class DivSignedMagicNumber32 : BaseTransform
{
	public DivSignedMagicNumber32() : base(IR.DivSigned32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IsResolvedConstant(context.Operand1))
			return false;

		if (Math.Abs(context.Operand2.ConstantSigned32) <= 2)
			return false;

		if (IsPowerOfTwo32(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var n = context.Operand1;
		var d = context.Operand2.ConstantSigned32;

		var (M, s) = DivisionTwiddling.GetMagicNumber(d);

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();
		var v4 = transform.VirtualRegisters.Allocate32();

		//mulhs q, M, n ; q = floor(M * n / 2 * *32).
		context.SetInstruction(IR.MulHs32, v1, n, Operand.CreateConstant32(M));

		//sub q, q, n ; q = floor(M * n / 2 * *32) - n.
		if (d > 0 && M < 0)
			context.AppendInstruction(IR.Add32, v2, v1, n);
		else if (d < 0 && M > 0)
			context.AppendInstruction(IR.Sub32, v2, v1, n);
		else
			context.AppendInstruction(IR.Move32, v2, v1);

		//shrsi q, q, s
		if (s > 0)
			context.AppendInstruction(IR.ArithShiftRight32, v3, v2, Operand.CreateConstant32(s));
		else
			context.AppendInstruction(IR.Move32, v3, v2);

		//shri t, q,31 ; Add 1 to q if
		context.AppendInstruction(IR.ShiftRight32, v4, n, Operand.Constant32_31);

		//add q,q,t ; q is negative(n is positive).
		context.AppendInstruction(IR.Add32, result, v3, v4);
	}
}
