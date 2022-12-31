// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadParamZeroExtend8x32
	/// </summary>
	public sealed class LoadParamZeroExtend8x32 : BaseTransform
	{
		public LoadParamZeroExtend8x32() : base(IRInstruction.LoadParamZeroExtend8x32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.MovzxLoad8, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
