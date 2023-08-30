// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// StoreParam64
/// </summary>
[Transform("ARM32.IR")]
public sealed class StoreParam64 : BaseIRTransform
{
	public StoreParam64() : base(IRInstruction.StoreParam64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Operand1, out var lowOffset, out var highOffset);
		transform.SplitOperand(context.Operand2, out var valueLow, out var valueHigh);

		TransformStore(transform, context, ARM32.Str32, transform.StackFrame, lowOffset, valueLow);
		TransformStore(transform, context.InsertAfter(), ARM32.Str32, transform.StackFrame, highOffset, valueHigh);
	}
}
