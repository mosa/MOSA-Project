// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadParamSignExtend16x64
	/// </summary>
	public sealed class LoadParamSignExtend16x64 : BaseTransform
	{
		public LoadParamSignExtend16x64() : base(IRInstruction.LoadParamSignExtend16x64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.MovsxLoad16, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
