// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.x64.Transforms.Optimizations.Auto;
using Mosa.Platform.x64.Transforms.Optimizations.Manual;

namespace Mosa.Platform.x64.Stages;

/// <summary>
/// x64 Optimization Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class OptimizationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "x64." + GetType().Name;

	public OptimizationStage()
		: base(true, false)
	{
		AddTranformations(AutoTransforms.List);
		AddTranformations(ManualTransforms.List);
	}
}
