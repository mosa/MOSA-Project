// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

public sealed class Or64SignExtend16x64SignExtend16x64 : BaseTransform
{
	public Or64SignExtend16x64SignExtend16x64() : base(IR.Or64, TransformType.Auto | TransformType.Optimization, 90)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.SignExtend16x64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.SignExtend16x64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(IR.Or64, v1, t1, t2);
		context.AppendInstruction(IR.SignExtend16x64, result, v1);
	}
}

public sealed class Or64SignExtend16x64SignExtend16x64_v1 : BaseTransform
{
	public Or64SignExtend16x64SignExtend16x64_v1() : base(IR.Or64, TransformType.Auto | TransformType.Optimization, 90)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.SignExtend16x64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.SignExtend16x64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(IR.Or64, v1, t2, t1);
		context.AppendInstruction(IR.SignExtend16x64, result, v1);
	}
}
