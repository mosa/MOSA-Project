// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// LoadSignExtend16x64AddressFold
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class LoadSignExtend16x64AddressFold : BaseTransform
{
	public LoadSignExtend16x64AddressFold() : base(Framework.IR.LoadSignExtend16x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.AddressOf)
			return false;

		if (!IsParameter(context.Operand1.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;

		context.SetInstruction(Framework.IR.LoadParamSignExtend16x64, result, t1);
	}
}
