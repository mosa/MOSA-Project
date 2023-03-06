// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// StoreParamR4
/// </summary>
public sealed class StoreParamR4 : BaseIRTransform
{
	public StoreParamR4() : base(IRInstruction.StoreParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand2 = MoveConstantToFloatRegister(transform, context, operand2);

		context.SetInstruction(X86.MovssStore, null, transform.StackFrame, operand1, operand2);
	}
}
