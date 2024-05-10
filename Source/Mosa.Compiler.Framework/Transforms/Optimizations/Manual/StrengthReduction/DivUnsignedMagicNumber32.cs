// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// DivUnsignedMagicNumber32
/// </summary>
public sealed class DivUnsignedMagicNumber32 : BaseTransform
{
	public DivUnsignedMagicNumber32() : base(IR.DivUnsigned32, TransformType.Manual | TransformType.Optimization, 100)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IsResolvedConstant(context.Operand1))
			return false;

		if (context.Operand2.ConstantUnsigned32 <= 2)
			return false;

		if (IsPowerOfTwo32(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var n = context.Operand1;
		var d = context.Operand2.ConstantUnsigned32;

		var (M, s, a) = DivisionTwiddling.GetMagicNumber(d);

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.MulHu32, v1, n, Operand.CreateConstant32(M));

		if (!a)
		{
			context.AppendInstruction(IR.ShiftRight32, result, v1, Operand.CreateConstant32(s));
		}
		else
		{
			var v2 = transform.VirtualRegisters.Allocate32();
			var v3 = transform.VirtualRegisters.Allocate32();
			var v4 = transform.VirtualRegisters.Allocate32();

			context.AppendInstruction(IR.Sub32, v2, n, v1);
			context.AppendInstruction(IR.ShiftRight32, v3, v2, Operand.Constant32_1);
			context.AppendInstruction(IR.Add32, v4, v3, v1);
			context.AppendInstruction(IR.ShiftRight32, result, v4, Operand.CreateConstant32(s - 1));
		}
	}
}
