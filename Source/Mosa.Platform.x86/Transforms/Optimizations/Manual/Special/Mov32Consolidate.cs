// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Optimizations.Manual.Special;

public sealed class Mov32Consolidate : BaseTransform
{
	public Mov32Consolidate() : base(X86.Mov32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Result.IsVirtualRegister)
			return false;

		if (context.Result.Definitions.Count != 2)
			return false;

		if (!IsSSAForm(context.Operand1))
			return false;

		if (context.Operand1.Uses.Count != 1)
			return false;

		if (context.Block != context.Operand1.Definitions[0].Block)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.Operand1.Definitions[0].Result = context.Result;

		context.SetNop();
	}
}
