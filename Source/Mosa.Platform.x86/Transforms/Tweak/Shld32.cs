// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Shld32
	/// </summary>
	public sealed class Shld32 : BaseTransformation
	{
		public Shld32() : base(X86.Shld32, TransformationType.Manual | TransformationType.Tranformation)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsConstant && !context.Operand2.IsConstant)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.MoveOperand1And2ToVirtualRegisters(context, X86.Mov32);
		}
	}
}
