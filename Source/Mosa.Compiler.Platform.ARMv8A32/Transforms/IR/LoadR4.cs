// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// LoadR4
	/// </summary>
	public sealed class LoadR4 : BaseTransform
	{
		public LoadR4() : base(IRInstruction.LoadR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderLoadStoreOperands(context);

			ARMv8A32TransformHelper.TransformLoad(transform, context, ARMv8A32.Ldf, context.Result, context.Operand1, context.Operand2);
		}
	}
}
