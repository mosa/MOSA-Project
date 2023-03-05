// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.Compound;

/// <summary>
/// StoreCompound
/// </summary>
public sealed class StoreCompound : BaseTransform
{
	public StoreCompound() : base(IRInstruction.StoreCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CompoundHelper.Copy(transform, context, context.Operand3.Type, context.Operand1, context.Operand2, transform.StackFrame, context.Operand3);
	}
}
