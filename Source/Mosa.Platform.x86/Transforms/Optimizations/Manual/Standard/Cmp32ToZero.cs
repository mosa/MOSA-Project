// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Optimizations.Manual.Standard
{
	public sealed class Cmp32ToZero : BaseTransformation
	{
		public Cmp32ToZero() : base(X86.Cmp32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (!context.Operand2.IsConstantZero)
				return false;

			var previous = GetPreviousNode(context);

			if (previous == null)
				return false;

			if (previous.ResultCount != 1)
				return false;

			if (previous.Instruction.IsMemoryRead)
				return false;

			if (previous.Result != context.Operand1)
				return false;

			if (!previous.Instruction.IsZeroFlagModified)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.Empty();
		}
	}
}
