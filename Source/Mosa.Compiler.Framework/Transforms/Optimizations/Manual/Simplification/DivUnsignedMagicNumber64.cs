// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

/// <summary>
/// DivUnsignedMagicNumber64
/// </summary>
[Transform]
public sealed class DivUnsignedMagicNumber64 : BaseTransform
{
	public DivUnsignedMagicNumber64() : base(IR.DivUnsigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (transform.Is32BitPlatform)
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IsResolvedConstant(context.Operand1))
			return false;

		if (context.Operand2.ConstantUnsigned64 <= 2)
			return false;

		if (IsPowerOfTwo64(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var n = context.Operand1;
		var d = context.Operand2.ConstantUnsigned32;

		var (M, s, a) = DivisionTwiddling.GetMagicNumber(d);

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(IR.MulHu64, v1, n, Operand.CreateConstant64(M));

		if (!a)
		{
			context.AppendInstruction(IR.ShiftRight64, result, v1, Operand.CreateConstant32(s));
		}
		else
		{
			var v2 = transform.VirtualRegisters.Allocate64();
			var v3 = transform.VirtualRegisters.Allocate64();
			var v4 = transform.VirtualRegisters.Allocate64();

			context.AppendInstruction(IR.Sub64, v2, n, v1);
			context.AppendInstruction(IR.ShiftRight64, v3, v2, Operand.Constant32_1);
			context.AppendInstruction(IR.Add64, v4, v3, v1);
			context.AppendInstruction(IR.ShiftRight64, result, v4, Operand.CreateConstant32(s - 1));
		}
	}
}
