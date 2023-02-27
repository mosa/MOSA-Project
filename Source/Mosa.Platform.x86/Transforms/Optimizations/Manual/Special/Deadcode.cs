// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Optimizations.Manual.Special;

public sealed class Deadcode : BaseTransform
{
	public Deadcode() : base(TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.ResultCount != 1)
			return false;

		if (context.Result.IsCPURegister)
			return false;

		if (context.Instruction.FlowControl == FlowControl.Call)
			return false;

		if (context.StatusRegister == StatusRegister.Set)
			return false;

		if (context.Result.Uses.Count != 0)
			return false;

		if (context.Instruction.IsIOOperation

			//|| context.Instruction.IsMemoryRead
			|| context.Instruction.IsMemoryWrite
			|| context.Instruction.HasUnspecifiedSideEffect)
			return false;

		// Check is split child, if so check is parent in use (Manual.Return for example)
		if (context.Result.HasLongParent && context.Result.LongParent.Uses.Count != 0)
			return false;

		var instruction = context.Instruction;

		if (!instruction.IsPlatformInstruction)
			return false;

		//if (!AreStatusFlagUsed(context))
		//	return false;

		if (instruction.IsCarryFlagModified
			|| instruction.IsOverflowFlagModified
			|| instruction.IsZeroFlagModified
			|| instruction.IsSignFlagModified
			|| instruction.IsParityFlagModified)
		{
			return !AreStatusFlagUsed(context);
		}

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetNop();
	}
}
