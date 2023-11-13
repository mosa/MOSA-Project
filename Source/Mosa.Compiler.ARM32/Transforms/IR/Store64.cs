// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// Store64
/// </summary>
[Transform("ARM32.IR")]
public sealed class Store64 : BaseIRTransform
{
	public Store64() : base(Framework.IR.Store64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Operand1, out var baseLow, out var baseHigh);
		transform.SplitOperand(context.Operand2, out var lowOffset, out var highOffset);
		transform.SplitOperand(context.Operand3, out var valueLow, out var valueHigh);

		TransformStore(transform, context, ARM32.Str32, baseLow, lowOffset, valueLow);
		TransformStore(transform, context.InsertAfter(), ARM32.Str32, baseLow, highOffset, valueHigh);
	}
}
