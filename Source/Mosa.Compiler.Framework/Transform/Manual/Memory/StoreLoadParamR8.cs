// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class StoreLoadParamR8 : BaseTransformation
	{
		public StoreLoadParamR8() : base(IRInstruction.StoreParamR8)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParamR8, out _);

			if (previous == null)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetNop();
		}
	}
}
