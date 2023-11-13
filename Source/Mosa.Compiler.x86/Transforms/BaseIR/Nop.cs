// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Nop
/// </summary>
[Transform("x86.BaseIR")]
public sealed class Nop : BaseIRTransform
{
	public Nop() : base(Framework.IR.Nop, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.Empty();

		//context.SetInstruction(X86.Nop);
	}
}
