// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class LoadSignExtend32x64Store32 : BaseTransformation
	{
		public LoadSignExtend32x64Store32() : base(IRInstruction.LoadSignExtend32x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			var previous = GetPreviousNodeUntil(context, IRInstruction.Store32, out bool immediate);

			if (previous == null)
				return false;

			if (!immediate && !IsSSAForm(previous.Operand3))
				return false;

			if (!previous.Operand2.IsResolvedConstant)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			if (previous.Operand2.ConstantUnsigned32 != context.Operand2.ConstantUnsigned32)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.Store32, out _);

			context.SetInstruction(IRInstruction.SignExtend32x64, context.Result, previous.Operand3);
		}
	}
}
