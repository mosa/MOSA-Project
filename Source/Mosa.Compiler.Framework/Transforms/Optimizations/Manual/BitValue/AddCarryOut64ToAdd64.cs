// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.BitValue;

[Transform()]
public sealed class AddCarryOut64ToAdd64 : BaseTransform
{
	public AddCarryOut64ToAdd64() : base(IR.AddCarryOut64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 20;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsBitValueSignBitCleared64(context.Operand1))
			return false;

		if (!IsBitValueSignBitCleared64(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		context.SetInstruction(IR.Add32, result, t1, t2);
		context.AppendInstruction(IR.Move32, result2, Operand.Constant64_0);
	}
}
