// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// StoreParamObject
	/// </summary>
	public sealed class StoreParamObject : BaseTransform
	{
		public StoreParamObject() : base(IRInstruction.StoreParamObject, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X86.MovStore32, null, transform.StackFrame, context.Operand1, context.Operand2);
		}
	}
}
