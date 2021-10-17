// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleStore32 : BaseTransformation
	{
		public DoubleStore32() : base(IRInstruction.Store32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsResolvedConstant)
				return false;

			var next = GetNextNodeUntil(context, IRInstruction.Store32, transformContext.Window, context.Operand1);

			if (next == null)
				return false;

			if (!next.Operand2.IsResolvedConstant)
				return false;

			if (next.Operand1 != context.Operand1)
				return false;

			if (next.Operand2.ConstantUnsigned64 != context.Operand2.ConstantUnsigned64)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetNop();
		}
	}
}
