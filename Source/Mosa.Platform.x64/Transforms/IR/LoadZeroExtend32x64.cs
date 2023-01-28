// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// LoadZeroExtend32x64
/// </summary>
public sealed class LoadZeroExtend32x64 : BaseTransform
{
	public LoadZeroExtend32x64() : base(IRInstruction.LoadZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.MovzxLoad32);
	}
}
