// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Cmp32
	/// </summary>
	public sealed class Cmp32 : BaseTransformation
	{
		public Cmp32() : base(X86.Cmp32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsConstant)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.MoveOperand1ToVirtualRegister(context, X86.Mov32);
		}
	}
}
