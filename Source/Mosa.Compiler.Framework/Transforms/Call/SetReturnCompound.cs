// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnCompound
/// </summary>
public sealed class SetReturnCompound : BaseTransform
{
	public SetReturnCompound() : base(IR.SetReturnCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var offset = Operand.CreateConstant32(transform.Architecture.OffsetOfFirstParameter);
		context.SetInstruction(IR.StoreCompound, null, transform.StackFrame, offset, context.Operand1);
	}
}
