// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadSignExtend8x64
/// </summary>
[Transform("x64.IR")]
public sealed class LoadSignExtend8x64 : BaseIRTransform
{
	public LoadSignExtend8x64() : base(Framework.IR.LoadSignExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.MovsxLoad8);
	}
}
