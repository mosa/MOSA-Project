// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual.Special;

public sealed class Deadcode : BaseTransform
{
	public Deadcode() : base(TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ResultCount != 1)
			return false;

		if (context.Result.IsPhysicalRegister)
			return false;

		if (context.Instruction.IsCall)
			return false;

		if (context.StatusRegister == StatusRegister.Set)
			return false;

		if (context.Result.IsUsed)
			return false;

		if (context.Instruction.IsIOOperation
			|| context.Instruction.IsMemoryWrite
			|| context.Instruction.HasUnspecifiedSideEffect)
			return false;

		// Check is split child, if so check is parent in use (Manual.Return for example)
		if (context.Result.HasParent && context.Result.Parent.IsUsed)
			return false;

		var instruction = context.Instruction;

		if (!instruction.IsPlatformInstruction)
			return false;

		if (instruction.IsCarryFlagModified
			|| instruction.IsOverflowFlagModified
			|| instruction.IsZeroFlagModified
			|| instruction.IsSignFlagModified
			|| instruction.IsParityFlagModified)
		{
			return !AreAnyStatusFlagsUsed(context);
		}

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.Empty();
	}
}
