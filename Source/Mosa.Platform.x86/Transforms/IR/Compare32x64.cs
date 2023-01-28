// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// Compare32x64
/// </summary>
public sealed class Compare32x64 : BaseTransform
{
	public Compare32x64() : base(IRInstruction.Compare32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		// FIXME!
		//Compare32x64(context);
	}
}
