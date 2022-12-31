// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadSignExtend32x64
	/// </summary>
	public sealed class LoadSignExtend32x64 : BaseTransform
	{
		public LoadSignExtend32x64() : base(IRInstruction.LoadSignExtend32x64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderOperands(context);

			context.SetInstruction(X64.MovzxLoad32, context.Result, context.Operand1, context.Operand2);
		}
	}
}
