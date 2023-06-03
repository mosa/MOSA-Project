// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.Optimizations.Manual.Special;

[Transform("x64.Optimizations.Manual.Special")]
public sealed class Deadcode : BaseTransform
{
	public Deadcode() : base(TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.ResultCount != 1)
			return false;

		if (context.Result.IsCPURegister)
			return false;

		if (context.Instruction.IsCall)
			return false;

		if (context.StatusRegister == StatusRegister.Set)
			return false;

		if (context.Result.IsUsed)
			return false;

		if (context.Instruction.IsIOOperation

			//|| context.Instruction.IsMemoryRead
			|| context.Instruction.IsMemoryWrite
			|| context.Instruction.HasUnspecifiedSideEffect)
			return false;

		// Check is split child, if so check is parent in use (Manual.Return for example)
		if (context.Result.HasParent && context.Result.Parent.IsUsed)
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
		context.Empty();
	}
}
