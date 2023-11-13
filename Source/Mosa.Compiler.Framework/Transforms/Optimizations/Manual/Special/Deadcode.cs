// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

public sealed class Deadcode : BaseTransform
{
	public Deadcode() : base(TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
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

		if (context.Instruction == IR.CallDynamic
			|| context.Instruction == IR.CallInterface
			|| context.Instruction == IR.CallDirect
			|| context.Instruction == IR.CallStatic
			|| context.Instruction == IR.CallVirtual
			|| context.Instruction == IR.NewObject
			|| context.Instruction == IR.SetReturnObject     // should not be necessary
			|| context.Instruction == IR.SetReturnManagedPointer
			|| context.Instruction == IR.SetReturn32
			|| context.Instruction == IR.SetReturn64
			|| context.Instruction == IR.SetReturnR4
			|| context.Instruction == IR.SetReturnR8
			|| context.Instruction == IR.SetReturnCompound
			|| context.Instruction == IR.IntrinsicMethodCall)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetNop();
	}
}
