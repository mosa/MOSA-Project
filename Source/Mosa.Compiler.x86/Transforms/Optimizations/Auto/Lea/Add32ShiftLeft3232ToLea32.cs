// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Auto.Lea;

[Transform()]
public sealed class Add32ShiftLeft3232ToLea32 : BaseTransform
{
	public Add32ShiftLeft3232ToLea32() : base(IR.Add32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 55;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.ShiftLeft32)
			return false;

		if (IsConstant(context.Operand2.Definitions[0].Operand1))
			return false;

		if (!IsUnsignedBetween32(context.Operand2.Definitions[0].Operand2, 1, 4))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var c1 = Operand.CreateConstant(0);

		context.SetInstruction(X86.Lea32, result, t1, t2, t3, c1);
	}
}
