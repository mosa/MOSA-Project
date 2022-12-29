// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Manual.Standard
{
	public sealed class Cmp32ToTest32 : BaseTransformation
	{
		public Cmp32ToTest32() : base(X86.Cmp32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (!context.Operand2.IsConstantZero)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(X86.Test32, null, context.Operand1, context.Operand1);
		}
	}
}
