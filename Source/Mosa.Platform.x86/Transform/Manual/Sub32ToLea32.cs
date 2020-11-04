// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Manual
{
	// This transformation can reduces restrictions placed on the register allocator.
	// The LEA does not change any of the status flags, however, the add instruction some flags (carry, zero, etc.)
	// Therefore, this transformation can only occur if the status flags are unused later.
	// A search is required to determine if a status flag is used
	// The search may not be conclusive; when so, the transformation is not made.

	public sealed class Sub32ToLea32 : BaseTransformation
	{
		public Sub32ToLea32() : base(X86.Sub32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (!(AreStatusFlagsUsed(context.Node) == TriState.No))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var constant = transformContext.CreateConstant(-context.Operand2.ConstantSigned32);

			context.SetInstruction(X86.Lea32, context.Result, context.Operand1, constant);
		}
	}
}
