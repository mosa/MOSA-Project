// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

public sealed class Deadcode : BaseTransform
{
	public Deadcode() : base(TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.ResultCount is 0 or > 2)
			return false;

		if (!context.Result.IsDefinedOnce)
			return false;

		if (context.Result.IsUsed)
			return false;

		if (context.ResultCount == 2 && !context.Result2.IsDefinedOnce)
			return false;

		if (context.ResultCount == 2 && context.Result2.IsUsed)
			return false;

		if (!context.Instruction.IsIRInstruction)
			return false;

		if (context.Instruction == IRInstruction.CallDynamic
			|| context.Instruction == IRInstruction.CallInterface
			|| context.Instruction == IRInstruction.CallDirect
			|| context.Instruction == IRInstruction.CallStatic
			|| context.Instruction == IRInstruction.CallVirtual
			|| context.Instruction == IRInstruction.NewObject
			|| context.Instruction == IRInstruction.SetReturnObject     // should not be necessary
			|| context.Instruction == IRInstruction.SetReturnManagedPointer
			|| context.Instruction == IRInstruction.SetReturn32
			|| context.Instruction == IRInstruction.SetReturn64
			|| context.Instruction == IRInstruction.SetReturnR4
			|| context.Instruction == IRInstruction.SetReturnR8
			|| context.Instruction == IRInstruction.SetReturnCompound
			|| context.Instruction == IRInstruction.IntrinsicMethodCall)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetNop();
	}
}
