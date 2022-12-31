// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadParam32
	/// </summary>
	public sealed class LoadParam32 : BaseTransform
	{
		public LoadParam32() : base(IRInstruction.LoadParam32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.MovLoad32, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
