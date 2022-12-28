// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x64.Transform.Manual.Standard
{
	public sealed class Mov32ToXor32 : BaseTransformation
	{
		public Mov32ToXor32() : base(X64.Mov32)
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
			context.SetInstruction(X64.Xor32, context.Result, context.Result, context.Result);
		}
	}
}
