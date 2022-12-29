// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Tweak
{
	/// <summary>
	/// Cmp32
	/// </summary>
	public sealed class Cmp32 : BaseTransformation
	{
		public Cmp32() : base(X86.Cmp32, TransformationType.Manual | TransformationType.Tranformation)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsConstant)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.MoveOperand1ToVirtualRegister(context, X86.Mov32);
		}
	}
}
