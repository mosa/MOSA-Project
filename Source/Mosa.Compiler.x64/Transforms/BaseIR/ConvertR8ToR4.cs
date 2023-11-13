// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ConvertR8ToR4
/// </summary>
[Transform("x64.BaseIR")]
public sealed class ConvertR8ToR4 : BaseIRTransform
{
	public ConvertR8ToR4() : base(Framework.IR.ConvertR8ToR4, TransformType.Manual | TransformType.Transform)
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
