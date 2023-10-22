// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Add32
/// </summary>
[Transform("x64.IR")]
public sealed class Add32 : BaseIRTransform
{
	public Add32() : base(IRInstruction.Add32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Add32);
	}
}
