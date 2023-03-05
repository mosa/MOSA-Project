// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Framework.Transforms.StaticLoad;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Static Load Optimization Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformStage" />
public sealed class StaticLoadOptimizationStage : BaseTransformStage
{
	public StaticLoadOptimizationStage()
		: base(true, false, 1)
	{
		AddTranformations(StaticLoadTransforms.List);
	}
}
