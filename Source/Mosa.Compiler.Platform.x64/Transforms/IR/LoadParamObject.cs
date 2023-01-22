// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadParamObject
	/// </summary>
	public sealed class LoadParamObject : BaseTransform
	{
		public LoadParamObject() : base(IRInstruction.LoadParamObject, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.MovLoad64, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
