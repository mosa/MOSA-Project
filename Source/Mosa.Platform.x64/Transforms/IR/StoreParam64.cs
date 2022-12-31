// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// StoreParam64
	/// </summary>
	public sealed class StoreParam64 : BaseTransform
	{
		public StoreParam64() : base(IRInstruction.StoreParam64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.MovStore64, null, transform.StackFrame, context.Operand1, context.Operand2);
		}
	}
}
