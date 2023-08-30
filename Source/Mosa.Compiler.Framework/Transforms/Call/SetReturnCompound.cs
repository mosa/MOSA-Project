// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnCompound
/// </summary>
public sealed class SetReturnCompound : BaseTransform
{
	public SetReturnCompound() : base(IRInstruction.SetReturnCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var offset = Operand.CreateConstant32(transform.Architecture.OffsetOfFirstParameter);
		context.SetInstruction(IRInstruction.StoreCompound, null, transform.StackFrame, offset, context.Operand1);
	}
}
