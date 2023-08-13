// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.ARM64.Transforms.Optimizations.Auto;
using Mosa.Platform.ARM64.Transforms.Optimizations.Manual;

namespace Mosa.Platform.ARM64.Stages;

/// <summary>
/// ARM64 Optimization Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class OptimizationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARM64." + GetType().Name;

	public OptimizationStage()
		: base(true, false)
	{
		AddTranforms(AutoTransforms.List);
		AddTranforms(ManualTransforms.List);
	}
}
