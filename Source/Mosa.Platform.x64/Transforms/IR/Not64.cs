// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// Not64
/// </summary>
[Transform("x64.IR")]
public sealed class Not64 : BaseIRTransform
{
	public Not64() : base(IRInstruction.Not64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.Mov64, context.Result, context.Operand1);
	}
}
