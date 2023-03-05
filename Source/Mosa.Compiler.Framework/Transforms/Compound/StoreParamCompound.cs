// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.Compound;

/// <summary>
/// StoreParamCompound
/// </summary>
public sealed class StoreParamCompound : BaseTransform
{
	public StoreParamCompound() : base(IRInstruction.StoreParamCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CompoundHelper.Copy(transform, context, context.Operand2.Type, transform.StackFrame, context.Operand1, transform.StackFrame, context.Operand2);
	}
}
