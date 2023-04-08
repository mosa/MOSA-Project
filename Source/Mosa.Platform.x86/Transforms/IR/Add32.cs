// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// Add32
/// </summary>
[Transform("x86.IR")]
public sealed class Add32 : BaseIRTransform
{
	public Add32() : base(IRInstruction.Add32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X86.Add32);
	}
}
