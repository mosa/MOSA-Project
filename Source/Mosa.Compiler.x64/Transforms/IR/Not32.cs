// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Not32
/// </summary>
[Transform("x64.IR")]
public sealed class Not32 : BaseIRTransform
{
	public Not32() : base(Framework.IR.Not32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.Not32, context.Result, context.Operand1);
	}
}
