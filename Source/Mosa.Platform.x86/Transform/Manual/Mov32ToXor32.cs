// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Manual
{
	public sealed class Mov32ToXor32 : BaseTransformation
	{
		public Mov32ToXor32() : base(X86.Mov32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsConstantZero)
				return false;

			if (!(AreStatusFlagsUsed(context.Node.Next, true, true, true, true, true) == TriState.No))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(X86.Xor32, context.Result, context.Result, context.Result);
		}
	}
}
