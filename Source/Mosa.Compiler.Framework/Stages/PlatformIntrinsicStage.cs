// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Framework.Transforms.PlatformIntrinsic;

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
		AddTranformations(PlatformIntrinsicTransforms.List);
	}
}
