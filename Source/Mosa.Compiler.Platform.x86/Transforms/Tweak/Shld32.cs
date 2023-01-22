// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Shld32
	/// </summary>
	public sealed class Shld32 : BaseTransform
	{
		public Shld32() : base(X86.Shld32, TransformType.Manual | TransformType.Transform)
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
