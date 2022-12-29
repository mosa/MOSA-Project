// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Tweak
{
	/// <summary>
	/// Popcnt32
	/// </summary>
	public sealed class Popcnt32 : BaseTransformation
	{
		public Popcnt32() : base(X86.Popcnt32, TransformationType.Manual | TransformationType.Tranformation)
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
