// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Tweak
{
	/// <summary>
	/// Tzcnt32
	/// </summary>
	public sealed class Tzcnt32 : BaseTransformation
	{
		public Tzcnt32() : base(X86.Tzcnt32, TransformationType.Manual | TransformationType.Tranformation)
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
