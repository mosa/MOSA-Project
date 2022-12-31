// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Shrd32
	/// </summary>
	public sealed class Shrd32 : BaseTransform
	{
		public Shrd32() : base(X86.Shrd32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsConstant && !context.Operand2.IsConstant)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.MoveOperand1And2ToVirtualRegisters(context, X86.Mov32);
		}
	}
}
