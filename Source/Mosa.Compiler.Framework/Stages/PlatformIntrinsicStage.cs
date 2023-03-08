// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.PlatformIntrinsic;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Platform Intrinsic Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformStage" />
public class PlatformIntrinsicStage : BaseTransformStage
{
	public PlatformIntrinsicStage()
		: base(true, false)
	{
		AddTranforms(PlatformIntrinsicTransforms.List);
	}
}
