// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Algebraic;

/// <summary>
/// Signed64AAMinusBB
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Signed64AAMinusBB : BaseTransform
{
	public Signed64AAMinusBB() : base(IRInstruction.Sub64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (context.Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2))
			return false;

		if (!AreSame(context.Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.AllocateVirtualRegister64();
		var v2 = transform.AllocateVirtualRegister64();

		context.SetInstruction(IRInstruction.Add64, v1, t1, t2);
		context.AppendInstruction(IRInstruction.Sub64, v2, t1, t2);
		context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
	}
}
