// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Auto.Lea;

[Transform()]
public sealed class Lea32Join : BaseTransform
{
	public Lea32Join() : base(X86.Lea32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsConstantZero)
			return false;

		if (!context.Operand3.IsConstantOne)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != X86.Lea32)
			return false;

		if (!IsResolvedConstant(context.Operand4))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand4))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand1.Definitions[0].Operand3;
		var t4 = context.Operand1.Definitions[0].Operand4;
		var t5 = context.Operand4;

		var e1 = Operand.CreateConstant(Add32(To32(t4), To32(t5)));

		context.SetInstruction(X86.Lea32, result, t1, t2, t3, e1);
	}
}
