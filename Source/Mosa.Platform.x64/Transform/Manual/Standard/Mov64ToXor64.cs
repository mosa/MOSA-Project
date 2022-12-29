// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x64.Transform.Manual.Standard
{
	public sealed class Mov64ToXor64 : BaseTransformation
	{
		public Mov64ToXor64() : base(X64.Mov64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsConstantZero)
				return false;

			if (AreStatusFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(X64.Xor64, context.Result, context.Result, context.Result);
		}
	}
}
