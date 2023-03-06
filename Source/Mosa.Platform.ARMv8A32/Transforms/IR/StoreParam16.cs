// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// StoreParam16
/// </summary>
public sealed class StoreParam16 : BaseIRTransform
{
	public StoreParam16() : base(IRInstruction.StoreParam16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARMv8A32.Str16, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
