// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadZeroExtend8x32
	/// </summary>
	public sealed class LoadZeroExtend8x32 : BaseTransform
	{
		public LoadZeroExtend8x32() : base(IRInstruction.LoadZeroExtend8x32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderLoadStoreOperands(context);

			context.SetInstruction(X64.MovzxLoad8, context.Result, context.Operand1, context.Operand2);
		}
	}
}
