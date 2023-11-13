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

		if (context.Instruction == Framework.IR.CallDynamic
			|| context.Instruction == Framework.IR.CallInterface
			|| context.Instruction == Framework.IR.CallDirect
			|| context.Instruction == Framework.IR.CallStatic
			|| context.Instruction == Framework.IR.CallVirtual
			|| context.Instruction == Framework.IR.NewObject
			|| context.Instruction == Framework.IR.SetReturnObject     // should not be necessary
			|| context.Instruction == Framework.IR.SetReturnManagedPointer
			|| context.Instruction == Framework.IR.SetReturn32
			|| context.Instruction == Framework.IR.SetReturn64
			|| context.Instruction == Framework.IR.SetReturnR4
			|| context.Instruction == Framework.IR.SetReturnR8
			|| context.Instruction == Framework.IR.SetReturnCompound
			|| context.Instruction == Framework.IR.IntrinsicMethodCall)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetNop();
	}
}
