// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

/// <summary>
/// DivUnsignedMagicNumber64
/// </summary>
[Transform("IR.Optimizations.Manual.Simplification")]
public sealed class DivUnsignedMagicNumber64 : BaseTransform
{
	public DivUnsignedMagicNumber64() : base(IRInstruction.DivUnsigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
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

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var operand1 = context.Operand1;
		var c = context.Operand2.ConstantUnsigned32;

		var (M, s, a) = DivisionTwiddling.GetMagicNumber(c);

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(IRInstruction.MulHu64, v1, operand1, Operand.CreateConstant64(M));

		if (!a)
		{
			context.AppendInstruction(IRInstruction.ShiftRight64, result, v1, Operand.CreateConstant32(s));
		}
		else
		{
			var v2 = transform.VirtualRegisters.Allocate64();
			var v3 = transform.VirtualRegisters.Allocate64();
			var v4 = transform.VirtualRegisters.Allocate64();

			context.AppendInstruction(IRInstruction.Sub64, v2, operand1, v1);
			context.AppendInstruction(IRInstruction.ShiftRight64, v3, v2, Operand.Constant32_1);
			context.AppendInstruction(IRInstruction.Add64, v4, v3, v1);
			context.AppendInstruction(IRInstruction.ShiftRight64, result, v4, Operand.CreateConstant32(s - 1));
		}
	}
}