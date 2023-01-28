// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// SignExtend32x64
/// </summary>
public sealed class SignExtend32x64 : BaseTransform
{
	public SignExtend32x64() : base(IRInstruction.SignExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Movsx32To64);
	}
}
