// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// StoreParam32
/// </summary>
public sealed class StoreParam32 : BaseTransform
{
	public StoreParam32() : base(IRInstruction.StoreParam32, TransformType.Manual | TransformType.Transform)
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
