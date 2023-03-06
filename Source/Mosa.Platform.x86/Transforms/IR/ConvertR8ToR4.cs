// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// ConvertR8ToR4
/// </summary>
public sealed class ConvertR8ToR4 : BaseIRTransform
{
	public ConvertR8ToR4() : base(IRInstruction.ConvertR8ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X86.Cvtsd2ss, result, operand1);
	}
}
