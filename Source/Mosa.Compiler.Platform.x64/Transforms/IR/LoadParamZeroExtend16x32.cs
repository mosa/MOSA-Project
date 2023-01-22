// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadParamZeroExtend16x32
	/// </summary>
	public sealed class LoadParamZeroExtend16x32 : BaseTransform
	{
		public LoadParamZeroExtend16x32() : base(IRInstruction.LoadParamZeroExtend16x32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.MovzxLoad16, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
