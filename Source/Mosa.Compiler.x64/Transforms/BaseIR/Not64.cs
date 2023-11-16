// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Not64
/// </summary>
[Transform]
public sealed class Not64 : BaseIRTransform
{
	public Not64() : base(IR.Not64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.Mov64, context.Result, context.Operand1);
	}
}
