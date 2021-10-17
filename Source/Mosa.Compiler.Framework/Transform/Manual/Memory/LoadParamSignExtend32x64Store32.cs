// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class LoadParamSignExtend32x64Store32 : BaseTransformation
	{
		public LoadParamSignExtend32x64Store32() : base(IRInstruction.LoadParamSignExtend32x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam32, out bool immediate);

			if (previous == null)
				return false;

			if (!immediate && !IsSSAForm(previous.Operand2))
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam32, out _);

			context.SetInstruction(IRInstruction.SignExtend32x64, context.Result, previous.Operand2);
		}
	}
}
