// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// StoreParam16
	/// </summary>
	public sealed class StoreParam16 : BaseTransform
	{
		public StoreParam16() : base(IRInstruction.StoreParam16, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X86.MovStore16, null, transform.StackFrame, context.Operand1, context.Operand2);
		}
	}
}
