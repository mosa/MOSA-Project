// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

public sealed class LoadZeroExtend16x32AddressFold : BaseTransform
{
	public LoadZeroExtend16x32AddressFold() : base(IR.LoadZeroExtend16x32, TransformType.Auto | TransformType.Optimization, 95)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsConstantZero)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.AddressOf)
			return false;

		if (!IsParameter(context.Operand1.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;

		context.SetInstruction(IR.LoadParamZeroExtend16x32, result, t1);
	}
}
