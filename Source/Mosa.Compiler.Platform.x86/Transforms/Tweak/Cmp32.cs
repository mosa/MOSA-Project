// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Cmp32
	/// </summary>
	public sealed class Cmp32 : BaseTransform
	{
		public Cmp32() : base(X86.Cmp32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsConstant)
				return false;

			if (context.Operand1.IsCPURegister)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.MoveOperand1ToVirtualRegister(context, X86.Mov32);
		}
	}
}
