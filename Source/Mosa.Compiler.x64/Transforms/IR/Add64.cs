// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Add64
/// </summary>
[Transform("x64.IR")]
public sealed class Add64 : BaseIRTransform
{
	public Add64() : base(Framework.IR.Add64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Add64);
	}
}
