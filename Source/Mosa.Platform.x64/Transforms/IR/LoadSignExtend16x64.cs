// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// LoadSignExtend16x64
/// </summary>
public sealed class LoadSignExtend16x64 : BaseIRTransform
{
	public LoadSignExtend16x64() : base(IRInstruction.LoadSignExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.MovsxLoad16);
	}
}
