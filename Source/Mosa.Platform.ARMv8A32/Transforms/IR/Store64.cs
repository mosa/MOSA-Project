// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Store64
/// </summary>
public sealed class Store64 : BaseIRTransform
{
	public Store64() : base(IRInstruction.Store64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Operand1, out var baseLow, out var baseHigh);
		transform.SplitLongOperand(context.Operand2, out var lowOffset, out var highOffset);
		transform.SplitLongOperand(context.Operand3, out var valueLow, out var valueHigh);

		TransformStore(transform, context, ARMv8A32.Str32, baseLow, lowOffset, valueLow);
		TransformStore(transform, context.InsertAfter(), ARMv8A32.Str32, baseLow, highOffset, valueHigh);
	}
}
