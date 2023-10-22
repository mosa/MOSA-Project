// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// ConvertR8ToR4
/// </summary>
[Transform("x64.IR")]
public sealed class ConvertR8ToR4 : BaseIRTransform
{
	public ConvertR8ToR4() : base(IRInstruction.ConvertR8ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsFloatingPoint && context.Result.IsFloatingPoint);

		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X64.Cvtsd2ss, result, operand1);
	}
}
