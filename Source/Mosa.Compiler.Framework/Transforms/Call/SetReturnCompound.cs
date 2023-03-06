// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Call;

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
		var OffsetOfFirstParameterOperand = transform.CreateConstant32(transform.Architecture.OffsetOfFirstParameter);
		context.SetInstruction(IRInstruction.StoreCompound, null, transform.StackFrame, OffsetOfFirstParameterOperand, context.Operand1);
	}
}
