// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
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
			context.SetInstruction(X86.MovLoad32, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
